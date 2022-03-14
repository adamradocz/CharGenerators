using System.Text;

namespace CharGenerators.Extensions;

internal static class StringBuilderExtensions
{
    internal static StringBuilder AppendNamespaceOpening(this StringBuilder stringBuilder, in string nameSpace)
    {
        if (!string.IsNullOrEmpty(nameSpace))
        {
            stringBuilder.Append($@"
namespace {nameSpace}
{{");
        }

        return stringBuilder;
    }

    internal static StringBuilder AppendNamespaceEnding(this StringBuilder stringBuilder, in string nameSpace)
    {
        if (!string.IsNullOrEmpty(nameSpace))
        {
            stringBuilder.Append(@"
}");
        }

        return stringBuilder;
    }

    internal static StringBuilder AppendClassOpening(this StringBuilder stringBuilder, in string accesibility, bool isStatic, in string name)
    {
        string staticKeyword = isStatic ? "static " : string.Empty;

        stringBuilder.Append($@"
    {accesibility} {staticKeyword}partial class {name}
    {{");

        return stringBuilder;
    }

    internal static StringBuilder AppendClassEnding(this StringBuilder stringBuilder)
    {
        return stringBuilder.Append($@"
    }}");
    }

    internal static StringBuilder AppendMethod(this StringBuilder stringBuilder, in string accesibility, bool isStatic, in string returnType, in string nameWithSignature)
    {
        string staticKeyword = isStatic ? "static " : string.Empty;

        return stringBuilder.Append($@"
        {accesibility} {staticKeyword}{returnType} {nameWithSignature}");
    }

    internal static StringBuilder AppendSwitchExpression(this StringBuilder stringBuilder, in string optimizeFor)
    {
        stringBuilder.Append($@"
            => value switch
            {{");
        foreach (char charToOptimize in optimizeFor)
        {
            stringBuilder.AppendSwitchSection(charToOptimize);
        }
        stringBuilder.Append($@"
                _ => value.ToString()
            }};");

        return stringBuilder;
    }

    private static StringBuilder AppendSwitchSection(this StringBuilder stringBuilder, char optimizeFor)
    {
        // Escape the char
        if (optimizeFor == '\'')
        {
            return stringBuilder.Append($@"
                '\{optimizeFor}' => ""{optimizeFor}"",");
        }

        // Escape the string
        if (optimizeFor == '"')
        {
            return stringBuilder.Append($@"
                '{optimizeFor}' => ""\{optimizeFor}"",");
        }

        // Escape the char and the string.
        if (optimizeFor == '\\')
        {
            return stringBuilder.Append($@"
                '\{optimizeFor}' => ""\{optimizeFor}"",");
        }

        return stringBuilder.Append($@"
                '{optimizeFor}' => ""{optimizeFor}"",");
    }
}
