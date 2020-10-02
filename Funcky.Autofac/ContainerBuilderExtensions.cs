using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Funcky.Monads;

namespace Funcky.Autofac
{
    public static class ContainerBuilderExtensions
    {
        private static readonly MethodInfo FromNullableMethod =
            typeof(ContainerBuilderExtensions)
                .GetMethod(nameof(FromNullable), BindingFlags.Static | BindingFlags.NonPublic)!;

        public static void RegisterOption(this ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterGeneric(ResolveOption)
                .As(typeof(Option<>))
                .IfNotRegistered(typeof(Option<>));
        }

        private static object ResolveOption(IComponentContext context, Type[] genericTypes)
        {
            var itemType = genericTypes.Single();
            var fromNullableMethod = FromNullableMethod.MakeGenericMethod(itemType);
            return fromNullableMethod.Invoke(null, new[] { context.ResolveOptional(itemType) });
        }

        private static Option<T> FromNullable<T>(T value)
            where T : class
            => Option.FromNullable(value);
    }
}
