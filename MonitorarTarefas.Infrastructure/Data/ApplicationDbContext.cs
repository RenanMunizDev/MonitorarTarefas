using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonitorarTarefas.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MonitorarTarefas.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<HistoricoTarefa> HistoricoTarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>().Property(u => u.Id).HasColumnName("Id");
            modelBuilder.Entity<Usuario>().Property(u => u.Funcao).HasColumnName("Funcao");

            modelBuilder.Entity<Tarefa>().ToTable("Tarefas");
            modelBuilder.Entity<Tarefa>().Property(t => t.Id).HasColumnName("Id");
            modelBuilder.Entity<Tarefa>().Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            modelBuilder.Entity<Tarefa>().Property(t => t.DataConclusao).HasColumnName("DataConclusao");
        }

    }
}
