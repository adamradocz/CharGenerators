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
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    [System.Diagnostics.Conditional(""CHARGENERATORS_USAGES"")]
    public class CharExtensionsAttribute : System.Attribute
    {
        /// <summary>
        /// Genereate the switch for the listed characters.
        /// </summary>
        public string OptimizeFor { get; set; }

        /// <summary>
        /// If set to <c>true</c>, the <c>ToStringFast</c> extension method will be generated.
        /// If set to <c>false</c>, the class specific <c>CharToStringFast(char value)</c> method will be generated.
        /// </summary>
        public bool Global { get; set; }
    }
}
#endif
";

    public static string GeneratePrivateHelperClass(ClassToGenerate classToGenerate)
    {
        var sb = new StringBuilder();
        sb.Append(_header);
        if (!string.IsNullOrEmpty(classToGenerate.Namespace))
        {
            sb.Append(@"
namespace ").Append(classToGenerate.Namespace).Append(@"
{");
        }

        sb.Append(@"
    ").Append($@"{classToGenerate.Accessibility.ToString().ToLower()} partial class {classToGenerate.Name}
    {{
        private string CharToStringFast(char value)
            => value switch
            {{");
        foreach (char optimizeFor in classToGenerate.OptimizeFor)
        {
            sb.Append($@"
                {GetSwitchSection(optimizeFor)}");
        }
        sb.Append($@"
                _ => value.ToString()
            }};
    }}");

        if (!string.IsNullOrEmpty(classToGenerate.Namespace))
        {
            sb.Append(@"
}");
        }

        return sb.ToString();
    }

    public static string GenerateExtensionClass(ClassToGenerate classToGenerate)
    {
        var sb = new StringBuilder();
        sb.Append(_header);
        if (!string.IsNullOrEmpty(classToGenerate.Namespace))
        {
            sb.Append(@"
namespace ").Append(classToGenerate.Namespace).Append(@"
{");
        }

        sb.Append(@"
    ").Append($@"public static partial class CharExtensions
    {{
        public static string ToStringFast(this char value)
            => value switch
            {{");
        foreach (char optimizeFor in classToGenerate.OptimizeFor)
        {
            sb.Append($@"
                {GetSwitchSection(optimizeFor)}");
        }
        sb.Append($@"
                _ => value.ToString()
            }};
    }}");
        if (!string.IsNullOrEmpty(classToGenerate.Namespace))
        {
            sb.Append(@"
}");
        }
        return sb.ToString();
    }

    private static string GetSwitchSection(char optimizeFor)
    {
        // Escape the char
        if (optimizeFor == '\'')
        {
            return @$"'\{optimizeFor}' => ""{optimizeFor}"",";
        }

        // Escape the string
        if (optimizeFor == '"')
        {
            return @$"'{optimizeFor}' => ""\{optimizeFor}"",";
        }

        // Escape the char and the string.
        if (optimizeFor == '\\')
        {
            return @$"'\{optimizeFor}' => ""\{optimizeFor}"",";
        }

        return @$"'{optimizeFor}' => ""{optimizeFor}"",";
    }
}
