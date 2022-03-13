namespace CharGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    [System.Diagnostics.Conditional("CHARGENERATORS_USAGES")]
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
