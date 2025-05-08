using BloodShadow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BloodShadow.Core.UnitTests
{
    public static class UnitTestManager
    {
        public static async Task<UnitTestResult[]> StartTest()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            UnitTestResult[][][][] results = new UnitTestResult[assemblies.Length][][][];

            for (int assemblyIndex = 0; assemblyIndex < assemblies.Length; assemblyIndex++)
            {
                Type[] types = assemblies[assemblyIndex].GetTypes();
                results[assemblyIndex] = new UnitTestResult[types.Length][][];
                for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
                {
                    MethodInfo[] methods = types[typeIndex].GetMethods(
                        BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.Static).Where(method =>
                        method.IsDefined(typeof(UnitTestAttribute), true) &&
                        method.ReturnType == typeof(bool)).ToArray();

                    results[assemblyIndex][typeIndex] = new UnitTestResult[methods.Length][];
                    for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++)
                    {
                        UnitTestAttribute[] tests = methods[methodIndex].GetCustomAttributes<UnitTestAttribute>(true).ToArray();
                        results[assemblyIndex][typeIndex][methodIndex] = new UnitTestResult[tests.Length];

                        for (int testIndex = 0; testIndex < tests.Length; testIndex++)
                        {
                            if (CheckParameters(methods[methodIndex].GetParameters(), tests[testIndex].Arguments))
                            {
                                try
                                {
                                    if ((bool)methods[methodIndex].Invoke(null, tests[testIndex].Arguments.ToArray()))
                                    {
                                        results[assemblyIndex][typeIndex][methodIndex][testIndex]
                                            = new UnitTestResult(assemblies[assemblyIndex].GetName().Name, types[typeIndex].Name, methods[methodIndex].Name,
                                            tests[testIndex].Name ?? methods[methodIndex].Name, null, tests[testIndex].SuccessfullyMessage, UnitTestResultEnum.Successful);
                                    }
                                    else
                                    {
                                        results[assemblyIndex][typeIndex][methodIndex][testIndex]
                                            = new UnitTestResult(assemblies[assemblyIndex].GetName().Name, types[typeIndex].Name, methods[methodIndex].Name,
                                            tests[testIndex].Name ?? methods[methodIndex].Name, null, tests[testIndex].FailtureMessage, UnitTestResultEnum.Failure);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    results[assemblyIndex][typeIndex][methodIndex][testIndex]
                                        = new UnitTestResult(assemblies[assemblyIndex].GetName().Name, types[typeIndex].Name, methods[methodIndex].Name,
                                        tests[testIndex].Name ?? methods[methodIndex].Name, ex, tests[testIndex].FailtureMessage, UnitTestResultEnum.Failure);
                                }
                            }
                            else
                            {
                                results[assemblyIndex][typeIndex][methodIndex][testIndex]
                                    = new UnitTestResult(assemblies[assemblyIndex].GetName().Name, types[typeIndex].Name, methods[methodIndex].Name,
                                    tests[testIndex].Name ?? methods[methodIndex].Name,
                                    new Exception("Invalid tests arguments"), tests[testIndex].FailtureMessage, UnitTestResultEnum.Failure);
                            }

                            await Task.Yield();
                        }
                    }
                }
            }

            return results.FromJaggedArray();
        }

        private static bool CheckParameters(IEnumerable<ParameterInfo> parameters, IEnumerable<object> arguments)
        {
            if (parameters == null || arguments == null) { return false; }
            if (!parameters.Any()) { return true; }
            if (parameters.Count() != arguments.Count()) { return false; }
            for (int i = 0; i < parameters.Count(); i++) { if (parameters.ElementAt(i).ParameterType != arguments.ElementAt(i).GetType()) { return false; } }
            return true;
        }
    }
}
