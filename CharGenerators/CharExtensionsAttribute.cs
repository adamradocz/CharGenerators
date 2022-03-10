namespace CharGenerators
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class CharExtensionsAttribute : System.Attribute
    {
        public string? OptimizeFor { get; set; }

        public bool Global { get; set; }
    }
}
