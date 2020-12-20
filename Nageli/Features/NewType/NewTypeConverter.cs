using System;
using System.Linq.Expressions;
using Tomlyn.Model;

namespace Nageli.Features.NewType
{
    internal sealed class NewTypeConverter<TNewType, TInner> : ITomlConverter<TNewType>
        where TNewType : notnull
    {
        private readonly Func<TInner, TNewType> _constructor;
        private readonly ITomlConverter<TInner> _innerConverter;

        public NewTypeConverter(NewTypeMetadata metadata)
        {
            _constructor = CompileConstructor(metadata);
            _innerConverter = metadata.InnerConverter.AsTomlConverter<TInner>();
        }

        public TNewType ConvertFrom(TomlObject value, ITomlSerializerContext context)
            => _constructor(_innerConverter.ConvertFrom(value, context));

        public TomlObject ConvertTo(TNewType value, ITomlSerializerContext context) => throw new NotImplementedException();

        private static Func<TInner, TNewType> CompileConstructor(NewTypeMetadata metadata)
        {
            var valueParameter = Expression.Parameter(metadata.InnerType, "value");
            var constructorExpression = Expression.Lambda<Func<TInner, TNewType>>(
                Expression.New(metadata.Constructor, valueParameter),
                valueParameter);
            return constructorExpression.Compile();
        }
    }
}
