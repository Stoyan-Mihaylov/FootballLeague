using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FootballLeague.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WithTracking<T>(
            this IQueryable<T> query,
            bool isTracked)
            where T : class
            => isTracked ? query : query.AsNoTracking();
    }
}
