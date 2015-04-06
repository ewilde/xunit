using System.Reflection.Emit;

namespace Xunit.Sdk.Commands.AssemblyCommands
{
    public class AssemblyLifeCycleCommandTests
    {
        [Fact]
        public void ClassWithNoAttributesInvokesNoMethods()
        {
            ClassWithNoStartUpShutDownAttributes.InvokeCount = 0;

            var lifeCycle = (AssemblyLifeCycleCommand) AssemblyLifeCycleCommandFactory.Make(new[]
            {
                typeof(ClassWithNoStartUpShutDownAttributes)
            });
            
            Assert.Null(lifeCycle);
        }

        [Fact]
        public void ClassWithStartUpAndShutDownAttributesInvokesMethods()
        {
            ClassWithStartUpShutDownAttributes.InvokeCount = 0;

            var lifeCycle = (AssemblyLifeCycleCommand)AssemblyLifeCycleCommandFactory.Make(new[]
            {
                typeof(ClassWithStartUpShutDownAttributes)
            });

            lifeCycle.StartUp();
            lifeCycle.ShutDown();

            Assert.Equal(2, ClassWithStartUpShutDownAttributes.InvokeCount);
        }

        [Fact]
        public void ClassWithManyStartUpAndShutDownAttributesInvokesEachMethod()
        {
            ClassWith2StartUpShutDownAttributes.InvokeCount = 0;

            var lifeCycle = (AssemblyLifeCycleCommand)AssemblyLifeCycleCommandFactory.Make(new[]
            {
                typeof(ClassWith2StartUpShutDownAttributes)
            });
            lifeCycle.StartUp();
            lifeCycle.ShutDown();

            Assert.Equal(4, ClassWith2StartUpShutDownAttributes.InvokeCount);
        }

        [Fact]
        public void MultipleClassesWithStartUpAndShutDownAttributesInvokesEachMethod()
        {
            ClassWithStartUpShutDownAttributes.InvokeCount = 0;
            ClassWith2StartUpShutDownAttributes.InvokeCount = 0;

            var lifeCycle = (AssemblyLifeCycleCommand)AssemblyLifeCycleCommandFactory.Make(
                new[]
                {
                    typeof(ClassWithStartUpShutDownAttributes),
                    typeof(ClassWith2StartUpShutDownAttributes)
                });
            lifeCycle.StartUp();
            lifeCycle.ShutDown();

            Assert.Equal(2, ClassWithStartUpShutDownAttributes.InvokeCount);
            Assert.Equal(4, ClassWith2StartUpShutDownAttributes.InvokeCount);
        }
    }

    public class ClassWithNoStartUpShutDownAttributes
    {
        public static int InvokeCount { get; set; }

        public void StartUp()
        {
            InvokeCount += 1;
        }
        public void ShutDown()
        {
            InvokeCount += 1;
        }
    }

    public class ClassWithStartUpShutDownAttributes
    {
        public static int InvokeCount { get; set; }

        [StartUp]
        public void StartUp()
        {
            InvokeCount += 1;
        }

        [ShutDown]
        public void ShutDown()
        {
            InvokeCount += 1;
        }
    }

    public class ClassWith2StartUpShutDownAttributes
    {
        public static int InvokeCount { get; set; }

        [StartUp]
        public void StartUp1()
        {
            InvokeCount += 1;
        }

        [StartUp]
        public void StartUp2()
        {
            InvokeCount += 1;
        }

        [ShutDown]
        public void ShutDown1()
        {
            InvokeCount += 1;
        }

        [ShutDown]
        public void ShutDown2()
        {
            InvokeCount += 1;
        }
    }


    public class ClassWithStartUpAttributes
    {
        public static int InvokeCount { get; set; }

        [StartUp]
        public void StartUp()
        {
            InvokeCount += 1;
        }

        public void ShutDown()
        {
            InvokeCount += 1;
        }
    }

    public class ClassWithShutDownAttributes
    {
        public static int InvokeCount { get; set; }

        public void StartUp()
        {
            InvokeCount += 1;
        }

        [ShutDown]
        public void ShutDown()
        {
            InvokeCount += 1;
        }
    }

}