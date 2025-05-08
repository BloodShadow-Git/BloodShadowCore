namespace BloodShadow.Core.UnitTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UnitTestAttribute(string name, string successfullyMessage, string failtureMessage, object[] arguments) : Attribute
    {
        private const string SUCCESSFULLYMESSAGE = "Successfully";
        private const string FAILTUREMESSAGE = "Failture";

        public string Name { get; } = name;
        public string SuccessfullyMessage { get; } = successfullyMessage;
        public string FailtureMessage { get; } = failtureMessage;
        public IReadOnlyCollection<object> Arguments { get; } = arguments?.AsReadOnly();

        public UnitTestAttribute(string name, string successfullyMessage, string failMessage) : this(name, successfullyMessage, failMessage, []) { }
        public UnitTestAttribute(string successfullyMessage, string failMessage, object[] arguments) : this(null, successfullyMessage, failMessage, arguments) { }
        public UnitTestAttribute(string successfullyMessage, string failMessage) : this(null, successfullyMessage, failMessage) { }
        public UnitTestAttribute(object[] arguments) : this(null, SUCCESSFULLYMESSAGE, FAILTUREMESSAGE, arguments) { }
        public UnitTestAttribute() : this(SUCCESSFULLYMESSAGE, FAILTUREMESSAGE) { }
    }
}
