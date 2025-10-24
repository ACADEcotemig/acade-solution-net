using Microsoft.EntityFrameworkCore;
using AcadeAppApi.Models;


namespace AcadeAppApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<EstacaoDeColeta> EstacoesDeColeta { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Trabalhador> Trabalhadores { get; set; }

    }
}
