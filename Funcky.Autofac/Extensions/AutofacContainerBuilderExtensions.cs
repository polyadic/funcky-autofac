using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Funcky.Monads;

namespace Funcky.Extensions
{
    public static class AutofacContainerBuilderExtensions
    {
        private const BindingFlags FromNullableBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

        private static readonly MethodInfo FromNullableMethod =
            typeof(AutofacContainerBuilderExtensions)
                .GetMethod(nameof(FromNullable), FromNullableBindingFlags)!;

        private static readonly Type OptionType = typeof(Option<>);

        public static void RegisterOption(this ContainerBuilder containerBuilder)
            => containerBuilder
                .RegisterGeneric(ResolveOption)
                .As(OptionType)
                .IfNotRegistered(OptionType);

        private static object ResolveOption(IComponentContext context, Type[] genericTypes)
            => ResolveOption(context.ResolveOptional, genericTypes.Single());

        private static object ResolveOption(Func<Type, object?> resolve, Type innerType)
            => ResolveOption(FromNullableMethod.MakeGenericMethod(innerType), resolve(innerType));

        private static object ResolveOption(MethodBase fromNullableMethod, object? innerValue)
            => fromNullableMethod.Invoke(null, new[] { innerValue });

        private static Option<T> FromNullable<T>(T value)
            where T : class
            => Option.FromNullable(value);
    }
}
