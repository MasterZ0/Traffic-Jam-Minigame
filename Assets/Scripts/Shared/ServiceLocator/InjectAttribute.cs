using System;

namespace Hasbro.TheGameOfLife.Shared
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {

    }
}
