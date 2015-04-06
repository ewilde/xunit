using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xunit.Sdk
{
    public class ExecutorTests
    {
        [Fact]
        public void ExecutorRunStartUpAssemblyInvokesStartupMethod()
        {
            var wait = new EventWaitHandle(
                initialState: false, 
                mode: EventResetMode.ManualReset,
                name: "test.xunit.ExecutorTests.startUpCalled");

            var result = GenerateAssembly();

            // Start asserting that the startup method in the generated assembly gets called
            var startUpCalled = false;
            var waitTask = Task.Run(() => startUpCalled = wait.WaitOne(TimeSpan.FromSeconds(10)));

            // Run tests against our generated assembly
            var runAssembly = new Executor.RunAssemblyStartUp(new Executor(result.CompiledAssembly.Location), this);

            // Verify the wait handle was called
            Task.WaitAll(waitTask);
            Assert.True(startUpCalled, "StartUp method not called during test execution");
        }

        [Fact]
        public void ExecutorRunAssemblyShutDownInvokesShutDownMethod()
        {
            var wait = new EventWaitHandle(
                initialState: false, 
                mode: EventResetMode.ManualReset,
                name: "test.xunit.ExecutorTests.shutDownCalled");

            var result = GenerateAssembly();

            // Start asserting that the startup method in the generated assembly gets called
            var shutDownCalled = false;
            var waitTask = Task.Run(() => shutDownCalled = wait.WaitOne(TimeSpan.FromSeconds(10)));

            // Run tests against our generated assembly
            var runAssembly = new Executor.RunAssemblyShutDown(new Executor(result.CompiledAssembly.Location), this);

            // Verify the wait handle was called
            Task.WaitAll(waitTask);
            Assert.True(shutDownCalled, "ShutDown method not called during test execution");
        }

        private static CompilerResults GenerateAssembly()
        {
            var parameters = new CompilerParameters {GenerateExecutable = false, OutputAssembly = "RunAssemblyStartUp.dll"};
            var executingAssembly = Assembly.GetExecutingAssembly();
            parameters.ReferencedAssemblies.Add(executingAssembly.Location);

            foreach (AssemblyName assemblyName in executingAssembly.GetReferencedAssemblies())
            {
                parameters.ReferencedAssemblies.Add(Assembly.Load(assemblyName).Location);
            }

            var result = CodeDomProvider.CreateProvider("CSharp")
                .CompileAssemblyFromSource(parameters, StartUpShutdownTestClassCode);
            return result;
        }

        private const string StartUpShutdownTestClassCode =
            @"
        using System;
        using System.Threading;
        using Xunit;
        public class StartUpShutdownTestClass
        {
            public bool StartUpMethodCalled { get; private set; }

            public bool ShutDownMethodCalled { get; private set; }

            [StartUp]
            public void StartUpMethod()
            {
                using (var wait = new EventWaitHandle(initialState: false, mode: EventResetMode.ManualReset, name: ""test.xunit.ExecutorTests.startUpCalled""))
                {
                    StartUpMethodCalled = true;
                    wait.Set();
                }
            }

            [ShutDown]
            public void ShutDownMethod()
            {
                using (var wait = new EventWaitHandle(initialState: false, mode: EventResetMode.ManualReset, name: ""test.xunit.ExecutorTests.shutDownCalled""))
                {
                    ShutDownMethodCalled = true;
                    wait.Set();
                }                
            }
        }";
    }
}