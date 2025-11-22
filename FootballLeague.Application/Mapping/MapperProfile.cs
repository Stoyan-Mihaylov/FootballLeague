using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace FootballLeague.Application.Mapping
{
    public class MapperProfile : Profile
    {
        private const string MappingMethodName = "Mapping";

        public MapperProfile()
            => ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly
                .GetExportedTypes()
                .Where(t => t
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(MappingMethodName) ??
                    type.GetInterface("IMapFrom`1")?.GetMethod(MappingMethodName);

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
