using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace CharGenerators;

[Generator]
public class CharExtensionsSourceGenerator : IIncrementalGenerator
{
    private const string _charExtensionsAttribute = "CharExtensions";
    private const string _charExtensionsAttributeFullName = $"{nameof(CharGenerators)}.{nameof(CharExtensionsAttribute)}";
    private const string _asciiPrintableCharacters = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource($"{nameof(CharExtensionsAttribute)}.g.cs", SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

        // Do a simple filter for classes
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select classes with attributes
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)) // sect the class with the [CharExtensions] attribute
            .Where(static m => m is not null)!; // filter out attributed classes that we don't care about

        // Combine the selected classes with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        // Generate the source using the compilation and classes
        context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    /// <summary>
    /// Filter syntax to only classes which have one or more attributes.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    static bool IsSyntaxTargetForGeneration(in SyntaxNode node) => node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0;

    /// <summary>
    /// Filter syntax to only classes which have the [CharExtensions] attribute.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    static ClassDeclarationSyntax? GetSemanticTargetForGeneration(in GeneratorSyntaxContext context)
    {
        // We know the node is a ClassDeclarationSyntax thanks to IsSyntaxTargetForGeneration.
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        // Loop through all the attributes on the method.
        foreach (AttributeListSyntax attributeListSyntax in classDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not ISymbol attributeSymbol)
                {
                    // Weird, we couldn't get the symbol, ignore it.
                    continue;
                }

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [CharExtensions] attribute?
                if (fullName == _charExtensionsAttributeFullName)
                {
                    // Return the class.
                    return classDeclarationSyntax;
                }
            }
        }

        // We didn't find the attribute we were looking for.
        return null;
    }

    private static void Execute(in Compilation compilation, in ImmutableArray<ClassDeclarationSyntax> classes, in SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            // Nothing to do yet.
            return;
        }

        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        IEnumerable<ClassDeclarationSyntax> distinctClasses = classes.Distinct();

        // Convert each ClassDeclarationSyntax to an EnumToGenerate.
        List<ClassToGenerate> classesToGenerate = GetTypesToGenerate(compilation, distinctClasses, context.CancellationToken);

        // If there were errors in the ClassDeclarationSyntax, we won't create a
        // ClassToGenerate for it, so make sure we have something to generate.
        foreach (var classToGenerate in classesToGenerate)
        {
            // Generate the source code and add it to the output.
            if (classToGenerate.Global)
            {
                string result = SourceGenerationHelper.GenerateExtensionClass(classToGenerate);
                context.AddSource($"{_charExtensionsAttribute}.g.cs", SourceText.From(result, Encoding.UTF8));
            }
            else
            {
                string result = SourceGenerationHelper.GeneratePrivateHelperClass(classToGenerate);
                context.AddSource($"{classToGenerate.Name}{_charExtensionsAttribute}.g.cs", SourceText.From(result, Encoding.UTF8));
            }
        }
    }

    static List<ClassToGenerate> GetTypesToGenerate(in Compilation compilation, in IEnumerable<ClassDeclarationSyntax> classes, in CancellationToken ct)
    {
        var classesToGenerate = new List<ClassToGenerate>();

        // Get the semantic representation of our marker attribute.
        INamedTypeSymbol? classAttribute = compilation.GetTypeByMetadataName(_charExtensionsAttributeFullName);

        if (classAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return classesToGenerate;
        }

        foreach (ClassDeclarationSyntax classDeclarationSyntax in classes)
        {
            // Stop if we're asked to.
            ct.ThrowIfCancellationRequested();

            // Get the semantic representation of the class syntax.
            SemanticModel semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
            {
                // Something went wrong, bail out.
                continue;
            }

            // Get the full type name of the class.
            string fullyQualifiedName = classSymbol.ToString();
            string name = classSymbol.Name;
            string nameSpace = classSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : classSymbol.ContainingNamespace.ToString();
            string? optimizeFor = null;
            bool global = false;

            foreach (AttributeData attributeData in classSymbol.GetAttributes())
            {
                foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
                {
                    if (namedArgument.Key == "OptimizeFor" && namedArgument.Value.Value?.ToString() is { } optimizeForValue)
                    {
                        optimizeFor = optimizeForValue;
                        continue;
                    }
                    if (namedArgument.Key == "Global" && namedArgument.Value.Value is { } globalValue)
                    {
                        global = (bool)globalValue;
                        continue;
                    }
                }
            }

            if (string.IsNullOrEmpty(optimizeFor))
            {
                optimizeFor = _asciiPrintableCharacters;
            }

            // Create an ClassToGenerate for use in the generation phase
            classesToGenerate.Add(new ClassToGenerate(
                name: name,
                fullyQualifiedName: fullyQualifiedName,
                ns: nameSpace,
                accessibility: classSymbol.DeclaredAccessibility,
                optimizeFor: optimizeFor!,
                global: global));
        }

        return classesToGenerate;
    }
}
