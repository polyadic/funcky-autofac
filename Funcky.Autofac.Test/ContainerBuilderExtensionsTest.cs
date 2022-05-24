using Autofac;
using Funcky.Extensions;
using Funcky.Monads;
using Funcky.Xunit;
using Xunit;

namespace Funcky.Autofac.Test
{
    public sealed class ContainerBuilderExtensionsTest
    {
        private interface ILogger
        {
        }

        [Fact]
        public void ResolvingAnOptionWhenTypeIsNotRegisteredReturnsNone()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterOption();
            using var container = containerBuilder.Build();
            FunctionalAssert.IsNone(container.Resolve<Option<ILogger>>());
        }

        [Fact]
        public void ResolvingAnOptionWhenTypeIsRegisteredReturnsInstance()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterOption();
            containerBuilder.RegisterType<NullLogger>().AsImplementedInterfaces();
            using var container = containerBuilder.Build();
            var logger = FunctionalAssert.IsSome(container.Resolve<Option<ILogger>>());
            Assert.True(logger is NullLogger);
        }

        [Fact]
        public void ResolvingAnOptionWhenMultipleTypesForSameServiceAreRegisteredReturnsDefaultService()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterOption();
            containerBuilder.RegisterType<NullLogger>().AsImplementedInterfaces();
            containerBuilder.RegisterType<StandardOutputLogger>().AsImplementedInterfaces();
            using var container = containerBuilder.Build();
            var logger = FunctionalAssert.IsSome(container.Resolve<Option<ILogger>>());
            Assert.True(logger is StandardOutputLogger);
        }

        private class NullLogger : ILogger
        {
        }

        private class StandardOutputLogger : ILogger
        {
        }
    }
}
