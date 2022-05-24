using Autofac.Features.Indexed;
using Funcky.Monads;

namespace Funcky.Extensions
{
    public static class AutofacIndexExtensions
    {
        public static Option<TValue> GetOrNone<TKey, TValue>(this IIndex<TKey, TValue> index, TKey key)
            where TValue : notnull
            => index.TryGetValue(key, out var value)
                ? value
                : Option<TValue>.None();
    }
}
