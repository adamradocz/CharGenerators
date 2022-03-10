namespace CharGenerators.Attributes
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
        /// If set to <c>true</c>, the <c>ToStringFast</c> extension method will be created.
        /// If set to <c>false</c>, a <c>private string CharToStringFast(char value)</c> class specific method will be created.
        /// </summary>
        public bool Global { get; set; }
    }
}
