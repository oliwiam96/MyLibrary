using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyLibrary.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    // dziedziczy po IdentityUser, które zapewnia unikalny email i unikalna nazwe uzytkownika
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime RegistrationTime { get; set; }

        /* public string MyLibraryId { get; set; }
         [ForeignKey("MyLibraryId")]
         public virtual Library MyLibrary { get; set; }*/
        public virtual Library Library { get; set; }

        public virtual ICollection<Reading> Readings { get; set; }

        public virtual ICollection<Reading> Comments { get; set; }

        public virtual ICollection<Friendship> Friendships{ get; set; } // biblioteki, do ktorych mnie zaproszono

        public virtual ICollection<RentalToOutside> RentalsToOutside { get; set; }

        public virtual ICollection<Rental> Rentals { get; set; } //ksiazki, ktore ja pozyczylem od kogos
        //TODO!!! czy to potrzebne, skoro jest user.Library.BookInLibraries.Rentals!!! TODO
        // info o pozyczonych komus sa w ksiazkach w mojej bibliotece

        //public virtual ICollection<Rental> RentalsExternal { get; set; } //ksiazki, ktore ja pozyczylem od kogos spoza systemu

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<MyLibrary.Models.Book> Books { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.Comment> Comments { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.BookInLibrary> BooksInLibrary { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.Friendship> Friendships { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.Library> Libraries { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.Reading> Readings { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.Rental> Rentals { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.RentalToOutside> RentalsToOutside { get; set; }
        public System.Data.Entity.DbSet<MyLibrary.Models.RentalExternal> RentalsExternal { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(c => c.Library)
                .WithRequired(d => d.ApplicationUser);
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Library>()
                .HasMany<BookInLibrary>(l => l.BookInLibrary)
                .WithRequired(b => b.Library)
                .HasForeignKey<int>(l => l.LibraryId);*/
        }
    }
}