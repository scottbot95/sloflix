using AutoMapper;
using sloflix.Models;

namespace sloflix.Data
{
  public class AutoMappers : Profile
  {
    public AutoMappers()
    {
      CreateMap<AppUser, AppUserDto>().ForMember(dto => dto.email, map => map.MapFrom(u => u.UserName));
      CreateMap<AppUserDto, AppUser>().ForMember(u => u.UserName, map => map.MapFrom(dto => dto.email));
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
