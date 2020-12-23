using System;

namespace Nageli.DefaultImplementations
{
    internal sealed class OpenGenericDefaultImplementationProvider : IDefaultImplementationProvider
    {
        private readonly Type _abstractType;
        private readonly Func<Type[], Type> _createConcreteType;

        public OpenGenericDefaultImplementationProvider(Type abstractType, Func<Type[], Type> createConcreteType)
        {
            if (!abstractType.IsAbstract && !abstractType.IsInterface)
            {
                throw new ArgumentException("Type must be abstract or an interface", nameof(abstractType));
            }

            if (!abstractType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Type must be an open generic", nameof(abstractType));
            }

            _abstractType = abstractType;
            _createConcreteType = createConcreteType;
        }

        public bool HasDefaultImplementation(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == _abstractType;

        public Type GetDefaultImplementation(Type typeToConvert)
            => _createConcreteType(typeToConvert.GetGenericArguments());
    }
}
