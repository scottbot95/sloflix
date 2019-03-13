﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using slo_flix.Models;

namespace slo_flix.Data.Migrations
{
  [DbContext(typeof(DataContext))]
  partial class DataContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

      modelBuilder.Entity("slo_flix.Models.Movie", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("PosterPath");

            b.Property<string>("Summary");

            b.Property<int>("TMDbId");

            b.Property<string>("Title")
                      .IsRequired();

            b.HasKey("Id");

            b.ToTable("Movies");
          });

      modelBuilder.Entity("slo_flix.Models.User", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Email");

            b.Property<string>("Password");

            b.Property<string>("Salt");

            b.HasKey("Id");

            b.ToTable("Users");
          });

      modelBuilder.Entity("slo_flix.Models.UserRating", b =>
          {
            b.Property<int>("MovieId");

            b.Property<int>("UserId");

            b.Property<int>("Rating");

            b.HasKey("MovieId", "UserId");

            b.HasIndex("UserId");

            b.ToTable("UserRatings");
          });

      modelBuilder.Entity("slo_flix.Models.Watchlist", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Name");

            b.HasKey("Id");

            b.ToTable("Watchlists");
          });

      modelBuilder.Entity("slo_flix.Models.WatchlistItem", b =>
          {
            b.Property<int>("MovieId");

            b.Property<int>("WatchlistId");

            b.HasKey("MovieId", "WatchlistId");

            b.HasIndex("WatchlistId");

            b.ToTable("WatchlistItems");
          });

      modelBuilder.Entity("slo_flix.Models.UserRating", b =>
          {
            b.HasOne("slo_flix.Models.Movie", "Movie")
                      .WithMany("UserRatings")
                      .HasForeignKey("MovieId")
                      .OnDelete(DeleteBehavior.Cascade);

            b.HasOne("slo_flix.Models.User", "User")
                      .WithMany()
                      .HasForeignKey("UserId")
                      .OnDelete(DeleteBehavior.Cascade);
          });

      modelBuilder.Entity("slo_flix.Models.WatchlistItem", b =>
          {
            b.HasOne("slo_flix.Models.Movie", "Movie")
                      .WithMany()
                      .HasForeignKey("MovieId")
                      .OnDelete(DeleteBehavior.Cascade);

            b.HasOne("slo_flix.Models.Watchlist", "Watchlist")
                      .WithMany("Movies")
                      .HasForeignKey("WatchlistId")
                      .OnDelete(DeleteBehavior.Cascade);
          });
#pragma warning restore 612, 618
    }
  }
}
