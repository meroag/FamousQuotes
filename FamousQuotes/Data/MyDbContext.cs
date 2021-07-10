﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FamousQuotes.Models;

#nullable disable

namespace FamousQuotes.Data
{
    public partial class MyDbContext : DbContext
    {
        private readonly string _cnString;

        public MyDbContext()
        {
        }

        public MyDbContext(string cnString)
        {
            _cnString = cnString;
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_cnString);
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<QuotesAuthors> QuotesAuthors { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersQuzi> UsersQuzi { get; set; }
        public virtual DbSet<UsersSession> UsersSession { get; set; }
        public virtual DbSet<UsersSettings> UsersSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Authors>(entity =>
            {
                entity.HasKey(e => e.IdAuthors)
                    .HasName("PK__Authors__A687DDF80D7A0286");
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.HasKey(e => e.IdQuotes)
                    .HasName("PK__Quotes__E1B6D85E08B54D69");

                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<QuotesAuthors>(entity =>
            {
                entity.HasKey(e => e.IdQuotesAuthors)
                    .HasName("PK__QuotesAu__1A2D2C0C114A936A");

                entity.HasOne(d => d.IdQuotesNavigation)
                    .WithMany(p => p.QuotesAuthors)
                    .HasForeignKey(d => d.IdQuotes)
                    .HasConstraintName("FK__QuotesAut__IdQuo__1332DBDC");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUsers)
                    .HasName("PK__Users__C781FF1903F0984C");

                entity.Property(e => e.IsEnabled).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UsersQuzi>(entity =>
            {
                entity.HasKey(e => e.IdUsersQuzi)
                    .HasName("PK__UsersQuz__A5C04F9E17036CC0");

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdQuotesNavigation)
                    .WithMany(p => p.UsersQuzi)
                    .HasForeignKey(d => d.IdQuotes)
                    .HasConstraintName("FK__UsersQuzi__IdQuo__1AD3FDA4");

                entity.HasOne(d => d.IdQuotesAuthorsNavigation)
                    .WithMany(p => p.UsersQuzi)
                    .HasForeignKey(d => d.IdQuotesAuthors)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersQuzi__IdQuo__1BC821DD");

                entity.HasOne(d => d.IdUsersNavigation)
                    .WithMany(p => p.UsersQuzi)
                    .HasForeignKey(d => d.IdUsers)
                    .HasConstraintName("FK__UsersQuzi__IdUse__19DFD96B");
            });

            modelBuilder.Entity<UsersSession>(entity =>
            {
                entity.HasKey(e => e.IdUsersSession)
                    .HasName("PK__UsersSes__9A2103612A164134");

                entity.Property(e => e.LoginTime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdUsersNavigation)
                    .WithMany(p => p.UsersSession)
                    .HasForeignKey(d => d.IdUsers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersSess__IdUse__2BFE89A6");
            });

            modelBuilder.Entity<UsersSettings>(entity =>
            {
                entity.HasKey(e => e.IdUsersSettings)
                    .HasName("PK__UsersSet__C407742A1EA48E88");

                entity.Property(e => e.SimpleQuizMode).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdUsersNavigation)
                    .WithMany(p => p.UsersSettings)
                    .HasForeignKey(d => d.IdUsers)
                    .HasConstraintName("FK__UsersSett__IdUse__208CD6FA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}