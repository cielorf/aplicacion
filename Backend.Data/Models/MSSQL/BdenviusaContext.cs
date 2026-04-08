using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Models.MSSQL;

public partial class BdenviusaContext : DbContext
{
    public BdenviusaContext()
    {
    }

    public BdenviusaContext(DbContextOptions<BdenviusaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Domicilio> Domicilios { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<Sesion> Sesions { get; set; }

    public virtual DbSet<Temporadum> Temporada { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=enviusamex.com;Database=CieloPasantia;User=Cielo;Password=S9rkOjbbk4SnHP1C;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.IdActor).HasName("pk_id_actor");

            entity.ToTable("Actor");

            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaNac).HasColumnName("fechaNac");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Poster)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("poster");
        });

        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.IdDomicilio).HasName("pk_id_domicilio");

            entity.ToTable("Domicilio");

            entity.HasIndex(e => e.IdUsuario, "Index_Domicilio_1").IsUnique();

            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.Calle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Colonia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("colonia");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.NumeroExterior)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroExterior");
            entity.Property(e => e.NumeroInterior)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("numeroInterior");
            entity.Property(e => e.Referencias)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("referencias");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Domicilio)
                .HasForeignKey<Domicilio>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idUsuario");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("pk_id_genero");

            entity.ToTable("Genero");

            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPelicula).HasName("pk_id_pelicula");

            entity.ToTable("Pelicula");

            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.AnioEstreno).HasColumnName("anioEstreno");
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("director");
            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Poster)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("poster");
            entity.Property(e => e.PrecioRecaudacion).HasColumnName("precioRecaudacion");
            entity.Property(e => e.Resumen)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("resumen");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasMany(d => d.IdActors).WithMany(p => p.IdPeliculas)
                .UsingEntity<Dictionary<string, object>>(
                    "PeliculaActor",
                    r => r.HasOne<Actor>().WithMany()
                        .HasForeignKey("IdActor")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idActor_TablaActor"),
                    l => l.HasOne<Pelicula>().WithMany()
                        .HasForeignKey("IdPelicula")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idPelicula_TablaPelicula"),
                    j =>
                    {
                        j.HasKey("IdPelicula", "IdActor").HasName("PK__Pelicula__BDD7DD83B5CF0AAA");
                        j.ToTable("Pelicula_Actor");
                        j.IndexerProperty<short>("IdPelicula").HasColumnName("idPelicula");
                        j.IndexerProperty<short>("IdActor").HasColumnName("idActor");
                    });

            entity.HasMany(d => d.IdGeneros).WithMany(p => p.IdPeliculas)
                .UsingEntity<Dictionary<string, object>>(
                    "PeliculaGenero",
                    r => r.HasOne<Genero>().WithMany()
                        .HasForeignKey("IdGenero")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idGenero_Genero"),
                    l => l.HasOne<Pelicula>().WithMany()
                        .HasForeignKey("IdPelicula")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idPelicula_Pelicula"),
                    j =>
                    {
                        j.HasKey("IdPelicula", "IdGenero").HasName("PK__Pelicula__B70944502A4072AA");
                        j.ToTable("Pelicula_Genero");
                        j.IndexerProperty<short>("IdPelicula").HasColumnName("idPelicula");
                        j.IndexerProperty<short>("IdGenero").HasColumnName("idGenero");
                    });
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("pk_id_rol");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.HasKey(e => e.IdSerie).HasName("pk_id_serie");

            entity.ToTable("Serie");

            entity.Property(e => e.IdSerie).HasColumnName("id_serie");
            entity.Property(e => e.AnioEstreno).HasColumnName("anioEstreno");
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("director");
            entity.Property(e => e.Duracion).HasColumnName("duracion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Plataforma)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("plataforma");
            entity.Property(e => e.Poster)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("poster");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasMany(d => d.IdActors).WithMany(p => p.IdSeries)
                .UsingEntity<Dictionary<string, object>>(
                    "SerieActor",
                    r => r.HasOne<Actor>().WithMany()
                        .HasForeignKey("IdActor")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idActorActor"),
                    l => l.HasOne<Serie>().WithMany()
                        .HasForeignKey("IdSerie")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idSerieSerie"),
                    j =>
                    {
                        j.HasKey("IdSerie", "IdActor").HasName("PK__Serie_Ac__BC9C7D348231C42D");
                        j.ToTable("Serie_Actor");
                        j.IndexerProperty<short>("IdSerie").HasColumnName("idSerie");
                        j.IndexerProperty<short>("IdActor").HasColumnName("idActor");
                    });

            entity.HasMany(d => d.IdGeneros).WithMany(p => p.IdSeries)
                .UsingEntity<Dictionary<string, object>>(
                    "SerieGenero",
                    r => r.HasOne<Genero>().WithMany()
                        .HasForeignKey("IdGenero")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idGenero"),
                    l => l.HasOne<Serie>().WithMany()
                        .HasForeignKey("IdSerie")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idSerie"),
                    j =>
                    {
                        j.HasKey("IdSerie", "IdGenero").HasName("PK__Serie_Ge__B642E4E775FD2652");
                        j.ToTable("Serie_Genero");
                        j.IndexerProperty<short>("IdSerie").HasColumnName("idSerie");
                        j.IndexerProperty<short>("IdGenero").HasColumnName("idGenero");
                    });

            entity.HasMany(d => d.IdTemporada).WithMany(p => p.IdSeries)
                .UsingEntity<Dictionary<string, object>>(
                    "SerieTemporadum",
                    r => r.HasOne<Temporadum>().WithMany()
                        .HasForeignKey("IdTemporada")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idTemporada_Serie_Temp"),
                    l => l.HasOne<Serie>().WithMany()
                        .HasForeignKey("IdSerie")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_idSerie_Serie_Temp"),
                    j =>
                    {
                        j.HasKey("IdSerie", "IdTemporada").HasName("PK__Serie_Te__CF305ADAFF75F110");
                        j.ToTable("Serie_Temporada");
                        j.IndexerProperty<short>("IdSerie").HasColumnName("idSerie");
                        j.IndexerProperty<short>("IdTemporada").HasColumnName("idTemporada");
                    });
        });

        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("pk_id_sesion");

            entity.ToTable("Sesion");

            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nickname");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Sesions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idUsuarioSesion");
        });

        modelBuilder.Entity<Temporadum>(entity =>
        {
            entity.HasKey(e => e.IdTemporada).HasName("pk_id_temporada");

            entity.Property(e => e.IdTemporada).HasColumnName("id_temporada");
            entity.Property(e => e.Episodios).HasColumnName("episodios");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.NumeroTemporada).HasColumnName("numeroTemporada");
            entity.Property(e => e.Sinopsis)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sinopsis");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("pk_id_usuario");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Correo, "UQ__Usuario__2A586E0BD1B6E206").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idRol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
