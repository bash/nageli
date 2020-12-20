using System;
using System.Collections.Generic;

namespace Nageli
{
    /// <summary>Provides default implementations for interfaces and abstract classes.</summary>
    public interface IDefaultImplementationProvider
    {
        bool HasDefaultImplementation(Type typeToConvert);

        /// <remarks>Note that <paramref name="typeToConvert"/> may be a generic type (e.g. <see cref="IEnumerable{T}"/>).</remarks>
        Type GetDefaultImplementation(Type typeToConvert);
    }
}
