using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace slo_flix.Models
{
  public class Movie : Entity
  {
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Summary { get; set; }
    public string PosterPath { get; set; }
    public int TMDbId { get; set; }

    public List<UserRating> UserRatings { get; set; }
  }

  public class MovieDto
  {
    public int id { get; set; }
    public string title { get; set; }
    public string summary { get; set; }
    public string posterPath { get; set; }
    public int tmdbId { get; set; }
  }

  public class UserRating : Entity
  {
    public int UserId { get; set; }
    public User User { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; }

    [Required]
    public int Rating { get; set; }
  }

  public class UserRatingDto
  {
    public int rating { get; set; }
  }
}