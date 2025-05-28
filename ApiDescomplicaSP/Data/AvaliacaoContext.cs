using Microsoft.EntityFrameworkCore;
using ApiAvaliacao.Models;

namespace ApiAvaliacao.Data
{
    public class AvaliacaoContext : DbContext
    {
        public AvaliacaoContext(DbContextOptions<AvaliacaoContext> options)
            : base(options)
        {
        }

        public DbSet<Avaliacao> Avaliacao { get; set; } // Tabela avaliacao
        public DbSet<Perguntas> Perguntas { get; set; }   // Tabela Perguntas
        public DbSet<Respostas> Respostas { get; set; }   // Tabela Respostas
        public DbSet<Senhas> Senhas { get; set; }
    }
}
