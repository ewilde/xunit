using System;

namespace Xunit
{
    /// <summary>
    /// Attribute that is applied to a method to indicate it should be run once
    /// during a test execution before any tests are executed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StartUpAttribute : Attribute
    {         
    }
}