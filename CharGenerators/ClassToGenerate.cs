using Microsoft.CodeAnalysis;

namespace CharGenerators;

public readonly struct ClassToGenerate
{
    public readonly string Name;
    public readonly string FullyQualifiedName;
    public readonly string Namespace;
    public readonly Accessibility Accessibility;
    public readonly string OptimizeFor;
    public readonly bool Global;

    public ClassToGenerate(string name, string ns, string fullyQualifiedName, Accessibility accessibility, string optimizeFor, bool global)
    {
        Name = name;
        Namespace = ns;
        Accessibility = accessibility;
        FullyQualifiedName = fullyQualifiedName;
        OptimizeFor = optimizeFor;
        Global = global;
    }
}
