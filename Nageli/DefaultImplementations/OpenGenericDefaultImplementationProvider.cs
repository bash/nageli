using System;

namespace Nageli.DefaultImplementations
{
    internal sealed class OpenGenericDefaultImplementationProvider : IDefaultImplementationProvider
    {
        private readonly Type _abstractType;
        private readonly Func<Type[], Type> _createConcreteType;

        public OpenGenericDefaultImplementationProvider(Type abstractType, Func<Type[], Type> createConcreteType)
        {
            _abstractType = abstractType;
            _createConcreteType = createConcreteType;
        }

        public bool HasDefaultImplementation(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == _abstractType;

        public Type GetDefaultImplementation(Type typeToConvert)
            => _createConcreteType(typeToConvert.GetGenericArguments());
    }
}
