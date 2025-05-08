using System;

namespace BloodShadow.Core.UnitTests
{
    public partial class UnitTestResult
    {
        public string AssemblyName { get; }
        public string TypeName { get; }
        public string MethodName { get; }
        public string TestName { get; }
        public Exception? Exception { get; }
        public string ResultMessage { get; }
        public UnitTestResultEnum Result { get; }

        internal UnitTestResult(string assemblyName, string typeName, string methodName, string testName,
            Exception? exception, string resultMessage, UnitTestResultEnum result)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            MethodName = methodName;
            TestName = testName;
            Exception = exception;
            ResultMessage = resultMessage;
            Result = result;
        }
    }
}
