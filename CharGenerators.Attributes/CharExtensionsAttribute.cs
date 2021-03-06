namespace CharGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [System.Diagnostics.Conditional("CHARGENERATORS_USAGES")]
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
