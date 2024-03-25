using Microsoft.EntityFrameworkCore;
using server.Constants;
using server.Models;
namespace server.Data
{
    public class DBContext : DbContext
    {
        private readonly IHttpContextAccessor contextAccessor;
        public DBContext(DbContextOptions<DBContext> options, IHttpContextAccessor _contextAccessor) : base(options)
        {
            contextAccessor = _contextAccessor;
        }

        /*         public DbSet<Usuario> Usuario { get; set; } */
        public DbSet<RecGrupo> RecGrupo => Set<RecGrupo>();

        public DbSet<Proyecto> Proyectos => Set<Proyecto>();
        public DbSet<Usuario> Usuario => Set<Usuario>();
        /*  */
        /*         public DbSet<RecUsuarioGrupo> RecUsuarioGrupo => Set<RecUsuarioGrupo>(); */
        public DbSet<RiAccesoModelo> RiAccesoModelo => Set<RiAccesoModelo>();
        public DbSet<RiMenu> RiMenu => Set<RiMenu>();
        public DbSet<RiModelo> RiModelo => Set<RiModelo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Proyecto>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasIndex(u => u.Nombre).IsUnique();
                e.HasQueryFilter(p => p.Estado == States.ACTIVE);
            });

            modelBuilder.Entity<Usuario>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasQueryFilter(p => p.Estado == States.ACTIVE);
                e.HasOne(e => e.Proyecto).WithMany(e => e.Usuarios).HasForeignKey(e => e.IdProyecto);
            });

            //? RecGrupo
            modelBuilder.Entity<RecGrupo>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasQueryFilter(p => p.Estado == States.ACTIVE && p.IdProyecto == GetProjectId());
                e.HasMany(e => e.Usuarios).WithMany(e => e.Grupos);
                e.HasOne(e => e.Proyecto).WithMany(e => e.RecGrupos).HasForeignKey(e => e.IdProyecto);
            });

            //? RiAccesoModelo
            modelBuilder.Entity<RiAccesoModelo>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasQueryFilter(p => p.Estado == States.ACTIVE);
                e.HasOne(e => e.Grupo).WithMany(e => e.Accesos).HasForeignKey(e => e.IdGrupo);
                e.HasOne(e => e.Modelo).WithMany(e => e.Accesos).HasForeignKey(e => e.IdModelo);
            });

            //? RiMenu
            modelBuilder.Entity<RiMenu>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasQueryFilter(p => p.Estado == States.ACTIVE && p.ProyectoId == GetProjectId());
                e.HasOne(e => e.Padre).WithMany().HasForeignKey(e => e.IdPadre);
                e.HasMany(e => e.Grupos).WithMany(e => e.Menus);
                e.HasOne(e => e.Proyecto).WithMany(e => e.RiMenus).HasForeignKey(e => e.ProyectoId);
            });

            //? RiModelo
            modelBuilder.Entity<RiModelo>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
                e.HasQueryFilter(p => p.Estado == States.ACTIVE);
                e.HasOne(e => e.Menu).WithMany(e => e.Modelos).HasForeignKey(e => e.IdMenu);
            });


            //* ===================== END: DATA =====================
        }


        //* ===================== START: MIddelware =====================   
        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(c => c.State == EntityState.Deleted || c.State == EntityState.Added || c.State == EntityState.Modified)
            )
            {
                var type = entry.GetType();
                var isBaseEntity = type?.GetProperty("Estado") != null;
                if (isBaseEntity)
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["Estado"] = States.DELETED;
                    }
                    if (entry.State == EntityState.Added)
                    {
                        var token = GetTokenId();
                        if (token != null)
                        {
                            entry.CurrentValues["IdUsrCreacion"] = token;
                        }
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        var token = GetTokenId();
                        if (token != null)
                        {
                            entry.CurrentValues["IdUsrModificacion"] = token;
                        }
                    }
                    var date = DateTime.UtcNow;
                    if (entry.State == EntityState.Added)
                    {
                        entry.CurrentValues["FechaCreacion"] = date;
                        entry.CurrentValues["FechaModificacion"] = date;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.CurrentValues["FechaModificacion"] = date;
                    }
                }
            }
        }

        public Guid? GetTokenId()
        {
            var tokenId = contextAccessor?.HttpContext?.User.FindFirst("Id")?.Value;
            if (tokenId == null) return null;
            Guid id = Guid.Parse(tokenId);
            return id;
        }

        public Guid? GetProjectId()
        {
            var projectId = contextAccessor?.HttpContext?.User.FindFirst("ProjectId")?.Value;
            if (projectId == null) return null;
            Guid id = Guid.Parse(projectId);
            return id;
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
