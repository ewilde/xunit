namespace Xunit.Abstractions
{
    /// <summary>
    /// Interface which describes the ability to executes all the startup and shutdown methods in an assembly.
    /// </summary>
    public interface IAssemblyLifeCycleCommand
    {
        /// <summary>
        /// Execute all the startup methods in an assembly
        /// </summary>
        void StartUp();

        /// <summary>
        /// Execute all the shutdown methods in an assembly
        /// </summary>
        void ShutDown();
    }
}
