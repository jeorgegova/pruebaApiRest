using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace pruebaApiRest.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<cstate> cstate { get; set; }

        // Excluye la tabla sysdiagrams del modelo
        [NotMapped]
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<cstate>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<cstate>()
                .HasMany(e => e.users)
                .WithOptional(e => e.cstate)
                .HasForeignKey(e => e.idState);

            modelBuilder.Entity<users>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.password)
                .IsUnicode(false);

            // Configura una clave primaria para la entidad sysdiagrams
            modelBuilder.Entity<sysdiagrams>()
                .HasKey(e => e.diagram_id);
        }
    }
}
