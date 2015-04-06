namespace Xunit.Sdk.Commands.AssemblyCommands
{
    public class AssemblyLifeCycleCommandFactoryTests
    {
        [Fact]
        public void NoAttributesShouldReturnNull()
        {
            Assert.Null(AssemblyLifeCycleCommandFactory.Make(new []
            {
                typeof(ClassWithNoStartUpShutDownAttributes)
            }));       
        }

        [Fact]
        public void OnlyStartUpAttributesShouldReturnCommand()
        {
            Assert.NotNull(AssemblyLifeCycleCommandFactory.Make(new []
            {
                typeof(ClassWithNoStartUpShutDownAttributes),
                typeof(ClassWithStartUpAttributes)
            }));       
        }

        [Fact]
        public void OnlyShutDownAttributesShouldReturnCommand()
        {
            Assert.NotNull(AssemblyLifeCycleCommandFactory.Make(new []
            {
                typeof(ClassWithNoStartUpShutDownAttributes),
                typeof(ClassWithShutDownAttributes)
            }));       
        }

        [Fact]
        public void StartUpShutDownAttributesShouldReturnCommand()
        {
            Assert.NotNull(AssemblyLifeCycleCommandFactory.Make(new []
            {
                typeof(ClassWithNoStartUpShutDownAttributes),
                typeof(ClassWithStartUpShutDownAttributes)
            }));       
        }
    }
}