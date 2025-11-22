using AutoMapper;

namespace FootballLeague.Application.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile mapper)
        {
            mapper.CreateMap(typeof(T), GetType())
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public interface IMapFrom<TFirst, TSecond>
    {
        void Mapping(Profile mapper)
        {
            mapper.CreateMap(typeof(TFirst), GetType())
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            mapper.CreateMap(typeof(TSecond), GetType())
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
