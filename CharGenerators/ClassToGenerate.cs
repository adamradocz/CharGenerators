using Microsoft.CodeAnalysis;

namespace CharGenerators;

public readonly struct ClassToGenerate
{
    public readonly string Name;
    public readonly string Namespace;
    public readonly string FullyQualifiedName;
    public readonly Accessibility Accessibility;
    public readonly string OptimizeFor;
    public readonly bool Global;

    public ClassToGenerate(in string name, in string ns, in string fullyQualifiedName, Accessibility accessibility, in string optimizeFor, bool global)
    {
        Name = name;
        Namespace = ns;
        FullyQualifiedName = fullyQualifiedName;
        Accessibility = accessibility;        
        OptimizeFor = optimizeFor;
        Global = global;
    }
}
