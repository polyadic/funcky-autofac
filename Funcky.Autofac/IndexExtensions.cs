using Autofac.Features.Indexed;
using Funcky.Monads;

namespace Funcky.Autofac
{
    public static class IndexExtensions
    {
        public static Option<TValue> GetOrNone<TKey, TValue>(this IIndex<TKey, TValue> index, TKey key)
            where TValue : notnull
            => index.TryGetValue(key, out var value)
                ? value
                : Option<TValue>.None();
    }
}
