using System;

namespace Nageli.DefaultImplementations
{
    internal sealed class SimpleDefaultImplementationProvider : IDefaultImplementationProvider
    {
        private readonly Type _abstractType;
        private readonly Type _defaultImplementation;

        public SimpleDefaultImplementationProvider(Type abstractType, Type defaultImplementation)
        {
            if (!abstractType.IsAbstract && !abstractType.IsInterface)
            {
                throw new ArgumentException("Type must be abstract or an interface", nameof(abstractType));
            }

            if (defaultImplementation.IsAbstract || defaultImplementation.IsInterface)
            {
                throw new ArgumentException("Type must be a concrete implementation", nameof(defaultImplementation));
            }

            if (!abstractType.IsAssignableFrom(defaultImplementation))
            {
                throw new ArgumentException("The implementation must inherit from the abstract type");
            }

            if (abstractType.IsGenericTypeDefinition || defaultImplementation.IsGenericTypeDefinition)
            {
                throw new NotSupportedException("Open generics are not supported");
            }

            _abstractType = abstractType;
            _defaultImplementation = defaultImplementation;
        }

        public bool HasDefaultImplementation(Type typeToConvert) => typeToConvert == _abstractType;

        public Type GetDefaultImplementation(Type typeToConvert) => _defaultImplementation;
    }
}
