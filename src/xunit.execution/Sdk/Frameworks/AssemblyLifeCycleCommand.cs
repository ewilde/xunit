using System;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
    /// <summary>
    /// An assembly life cycle command that can execute all startup and shutdown methods for an assembly.
    /// </summary>
    public class AssemblyLifeCycleCommand : IAssemblyLifeCycleCommand
    {
        private readonly IEnumerable<IMethodInfo> _startupMethods;
        private readonly IEnumerable<IMethodInfo> _shutDownMethods;
        private readonly Dictionary<string, object> _instances;

        /// <summary>
        /// Creates a new instance of the <see cref="AssemblyLifeCycleCommand"/>.
        /// </summary>
        /// <param name="startupMethods">All of the associated startup methods for a given assembly</param>
        /// <param name="shutDownMethods">All of the associated shutdown methods for a given assembly</param>
        public AssemblyLifeCycleCommand(IEnumerable<IMethodInfo> startupMethods, IEnumerable<IMethodInfo> shutDownMethods)
        {
            _startupMethods = startupMethods;
            _shutDownMethods = shutDownMethods;
            _instances = new Dictionary<string, object>();
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
                var typeName = method.Type.ToString();
                if (!_instances.ContainsKey(typeName))
                {
                    try
                    {
                        _instances.Add(typeName, Activator.CreateInstance(method.Type.ToRuntimeType()));
                    }
                    catch (Exception exception)
                    {
                        //System.Console.WriteLine(exception);
                        // use IMessageAggregator messageAggregator
                        throw;
                    }
                }

                method.ToRuntimeMethod().Invoke(_instances[typeName], null);
            }
        }
    }
}