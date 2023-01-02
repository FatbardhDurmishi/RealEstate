using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RealEstate.Data.Entities;

namespace RealEstate.Data.Context
{
    public partial class DBRealEstateContext : DbContext
    {
        public DBRealEstateContext()
        {
        }

        public DBRealEstateContext(DbContextOptions<DBRealEstateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Property> Properties { get; set; } = null!;
        public virtual DbSet<PropertyImage> PropertyImages { get; set; } = null!;
        public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionsType> TransactionsTypes { get; set; } = null!;

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasOne(d => d.PropertyTypeNavigation)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.PropertyType)
                    .HasConstraintName("FK__Propertie__Prope__74AE54BC");

                entity.HasOne(d => d.TransactionTypeNavigation)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.TransactionType)
                    .HasConstraintName("FK__Propertie__Trans__72C60C4A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Properties)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Propertie__UserI__73BA3083");
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PropertyImages)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("FK__PropertyI__Prope__778AC167");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.TransactionBuyers)
                    .HasForeignKey(d => d.BuyerId)
                    .HasConstraintName("FK__Transacti__Buyer__2DE6D218");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TransactionOwners)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__Transacti__Owner__2CF2ADDF");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("FK__Transacti__Prope__2EDAF651");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
