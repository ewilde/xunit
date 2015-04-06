using System;
using System.Collections.Generic;

namespace Xunit.Sdk
{
    /// <summary>
    /// An assembly life cycle command that can execute all startup and shutdown methods for an assembly.
    /// </summary>
    public class AssemblyLifeCycleCommand : IAssemblyLifeCycleCommand
    {
        private readonly IEnumerable<IMethodInfo> _startupMethods;
        private readonly IEnumerable<IMethodInfo> _shutDownMethods;
        private readonly Dictionary<Type, object> _instances; 

        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyLifeCycleCommand"/>.
        /// </summary>
        /// <param name="startupMethods">All of the associated startup methods for a given assembly</param>
        /// <param name="shutDownMethods">All of the associated shutdown methods for a given assembly</param>
        public AssemblyLifeCycleCommand(IEnumerable<IMethodInfo> startupMethods, IEnumerable<IMethodInfo> shutDownMethods)
        {
            _startupMethods = startupMethods;
            _shutDownMethods = shutDownMethods;
            _instances = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Execute all the startup methods in an assembly
        /// </summary>
        public void StartUp()
        {
            InvokeMethods(_startupMethods);
        }

        /// <summary>
        /// Execute all the shutdown methods in an assembly
        /// </summary>
        public void ShutDown()
        {
            InvokeMethods(_shutDownMethods);
        }

        private void InvokeMethods(IEnumerable<IMethodInfo> methods)
        {
            foreach (var method in methods)
            {
                if (!_instances.ContainsKey(method.Class.Type))
                {
                    try
                    {
                        _instances.Add(method.Class.Type, method.CreateInstance());
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }

                method.Invoke(_instances[method.Class.Type]);
            }
        }
    }
}