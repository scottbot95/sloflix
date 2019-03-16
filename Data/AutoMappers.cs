using System.Linq;
using AutoMapper;
using sloflix.Models;

namespace sloflix.Data
{
  public class AutoMappers : Profile
  {
    public AutoMappers()
    {
      CreateMap<AppUser, AppUserDto>()
        .ForMember(dto => dto.email, map => map.MapFrom(u => u.UserName));
      CreateMap<AppUserDto, AppUser>()
        .ForMember(u => u.UserName, map => map.MapFrom(dto => dto.email));

      CreateMap<UserRating, UserRatingDto>();
      CreateMap<UserRatingDto, UserRating>();

      CreateMap<Movie, MovieDto>();
      CreateMap<MovieDto, Movie>();

      CreateMap<Watchlist, WatchlistDetailsDto>();
      CreateMap<WatchlistDetailsDto, Watchlist>();

      CreateMap<Watchlist, WatchlistSummaryDto>()
        .ForMember(dto => dto.movies, map => map.MapFrom(wl => wl.Movies.Select(li => li.MovieId)));

      CreateMap<WatchlistItem, WatchlistItemDto>();
      CreateMap<WatchlistItemDto, WatchlistItem>();

      CreateMap<WatchlistItem, MovieDto>()
        .ForMember(dto => dto.id, map => map.MapFrom(li => li.Movie.Id))
        .ForMember(dto => dto.posterPath, map => map.MapFrom(li => li.Movie.PosterPath))
        .ForMember(dto => dto.summary, map => map.MapFrom(li => li.Movie.Summary))
        .ForMember(dto => dto.title, map => map.MapFrom(li => li.Movie.Title))
        .ForMember(dto => dto.tmdbId, map => map.MapFrom(li => li.Movie.TMDbId));
    }
  }
}
