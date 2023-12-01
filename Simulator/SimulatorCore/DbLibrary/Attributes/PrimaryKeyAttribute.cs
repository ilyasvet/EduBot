namespace DbLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class PrimaryKeyAttribute : Attribute
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }
        public PrimaryKeyAttribute(string? name, Type? type = null)
        {
            Name = name;
            Type = type;
        }
    }
}
