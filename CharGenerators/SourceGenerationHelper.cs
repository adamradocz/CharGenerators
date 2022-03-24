using CharGenerators.Extensions;
using System.Text;

namespace CharGenerators;

public static class SourceGenerationHelper
{
    private const string _header = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the CharGenerators source generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable";

    public const string Attribute = _header + @"
#if CAHRGENERATORS_EMBED_ATTRIBUTES
namespace CharGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [System.Diagnostics.Conditional(""CHARGENERATORS_USAGES"")]
    public class CharExtensionsAttribute : System.Attribute
    {
        /// <summary>
        /// Genereate the switch for the listed characters.
        /// </summary>
        public string OptimizeFor { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the <c>ToStringFast</c> extension method will be generated.
        /// If set to <c>false</c>, a private <c>CharToStringFast(char value)</c> helper method will be generated.
        /// </summary>
        public bool Global { get; set; }
    }
}
#endif
";

    public static string GeneratePrivateHelperClass(in ClassToGenerate classToGenerate)
    {
        bool isStatic = false;

        var stringBuilder = new StringBuilder();
        stringBuilder
            .Append(_header)
            .AppendNamespaceOpening(classToGenerate.Namespace)
            .AppendClassOpening(classToGenerate.Accessibility.ToString().ToLower(), isStatic, classToGenerate.Name)
            .AppendMethod("private", isStatic, "string", "CharToStringFast(char value)")
            .AppendSwitchExpression(classToGenerate.OptimizeFor)
            .AppendClassEnding()
            .AppendNamespaceEnding(classToGenerate.Namespace);

        return stringBuilder.ToString();
    }

    public static string GenerateExtensionClass(in ClassToGenerate classToGenerate)
    {
        string accessibility = "public";
        bool isStatic = true;

        var stringBuilder = new StringBuilder();
        stringBuilder
            .Append(_header)
            .AppendNamespaceOpening(classToGenerate.Namespace)
            .AppendClassOpening(accessibility, isStatic, "CharExtensions")
            .AppendMethod(accessibility, isStatic, "string", "ToStringFast(this char value)")
            .AppendSwitchExpression(classToGenerate.OptimizeFor)
            .AppendClassEnding()
            .AppendNamespaceEnding(classToGenerate.Namespace);
        return stringBuilder.ToString();
    }
}
