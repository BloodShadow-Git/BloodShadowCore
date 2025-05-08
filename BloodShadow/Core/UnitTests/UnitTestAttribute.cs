using System;
using System.Collections.Generic;

namespace BloodShadow.Core.UnitTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UnitTestAttribute : Attribute
    {
        private const string SUCCESSFULLYMESSAGE = "Successfully";
        private const string FAILTUREMESSAGE = "Failture";

        public string? Name { get; }
        public string SuccessfullyMessage { get; }
        public string FailtureMessage { get; }
        public IReadOnlyCollection<object> Arguments { get; }

        public UnitTestAttribute(string? name, string successfullyMessage, string failMessage) : this(name, successfullyMessage, failMessage, Array.Empty<object>()) { }
        public UnitTestAttribute(string successfullyMessage, string failMessage, object[] arguments) : this(null, successfullyMessage, failMessage, arguments) { }
        public UnitTestAttribute(string successfullyMessage, string failMessage) : this(null, successfullyMessage, failMessage) { }
        public UnitTestAttribute(object[] arguments) : this(null, SUCCESSFULLYMESSAGE, FAILTUREMESSAGE, arguments) { }
        public UnitTestAttribute() : this(SUCCESSFULLYMESSAGE, FAILTUREMESSAGE) { }

        public UnitTestAttribute(string? name, string successfullyMessage, string failtureMessage, object[] arguments)
        {
            Name = name;
            SuccessfullyMessage = successfullyMessage;
            FailtureMessage = failtureMessage;
            Arguments = Array.AsReadOnly(arguments);
        }
    }
}
