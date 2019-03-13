using AutoMapper;
using slo_flix.Models;

namespace slo_flix.Data
{
  public class AutoMappers : Profile
  {
    public AutoMappers()
    {
      CreateMap<User, UserDto>().ForMember(dto => dto.email, map => map.MapFrom(u => u.UserName));
      CreateMap<UserDto, User>().ForMember(u => u.UserName, map => map.MapFrom(dto => dto.email));
      CreateMap<UserRating, UserRatingDto>();
      CreateMap<UserRatingDto, UserRating>();
      CreateMap<Movie, MovieDto>();
      CreateMap<MovieDto, Movie>();
      CreateMap<Watchlist, WatchlistDto>();
      CreateMap<WatchlistDto, Watchlist>();
      CreateMap<WatchlistItem, WatchListItemDto>();
      CreateMap<WatchListItemDto, WatchlistItem>();
    }
  }
}
