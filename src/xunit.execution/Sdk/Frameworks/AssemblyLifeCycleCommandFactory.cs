using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
    /// <summary>
    /// Factory for <see cref="IAssemblyLifeCycleCommand"/> objects, based on the types of a given assembly.
    /// </summary>
    public class AssemblyLifeCycleCommandFactory
    {
        /// <summary>
        /// Creates the assembly life cycle command, which implements <see cref="IAssemblyLifeCycleCommand"/>, 
        /// for a given assembly.
        /// </summary>
        /// <param name="assemblyTypes">Collection of types for a given assembly</param>
        /// <returns>The assembly life cycle, if the collection of types includes class with a startup or shutdown method; null, otherwise</returns>
        public static IAssemblyLifeCycleCommand Make(IEnumerable<ITypeInfo> assemblyTypes)
        {
            var startUpMethods = new List<IMethodInfo>();
            var shutDownUpMethods = new List<IMethodInfo>();
            foreach (var type in assemblyTypes)
            {
                startUpMethods.AddRange(TypeUtility.GetStartUpMethods(type));
                shutDownUpMethods.AddRange(TypeUtility.GetShutDownMethods(type));
            }

            if (startUpMethods.Count == 0 && shutDownUpMethods.Count == 0)
            {
                return null;
            }

            return new AssemblyLifeCycleCommand(startUpMethods, shutDownUpMethods);
        }        
    }
}