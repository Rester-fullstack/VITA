using Microsoft.EntityFrameworkCore;
using VitaApi.Models;

namespace VitaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Medico> Medicos => Set<Medico>();

    public DbSet<Paciente> Pacientes => Set<Paciente>();

    public DbSet<Consulta> Consultas => Set<Consulta>();

    public DbSet<Especialidade> Especialidades => Set<Especialidade>();

    public DbSet<Exame> Exames => Set<Exame>();

    public DbSet<HistoricoClinico> HistoricosClinicos => Set<HistoricoClinico>();

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<AgendaMedica> AgendaMedica { get; set; }

    public DbSet<Receita> Receitas { get; set; }

    public DbSet<Atestado> Atestados { get; set; }

    public DbSet<Odontograma> Odontogramas { get; set; }

    public DbSet<PsicologiaRegistro> PsicologiaRegistros { get; set; }

    public DbSet<NutricaoRegistro> NutricaoRegistros { get; set; }

    public DbSet<SolicitacaoExame> SolicitacoesExames { get; set; }

    public DbSet<DeclaracaoComparecimento> DeclaracoesComparecimento { get; set; }

    public DbSet<Auditoria> Auditorias { get; set; }

    public DbSet<ConfiguracaoClinica> ConfiguracoesClinica { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        

        modelBuilder.Entity<Paciente>()
            .HasIndex(x => x.CPF)
            .IsUnique();

        

        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Paciente)
            .WithMany(p => p.Consultas)
            .HasForeignKey(c => c.PacienteId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Consulta>()
            .HasOne(c => c.Medico)
            .WithMany(m => m.Consultas)
            .HasForeignKey(c => c.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);

        

        modelBuilder.Entity<Medico>()
            .HasOne(m => m.Especialidade)
            .WithMany(e => e.Medicos)
            .HasForeignKey(m => m.EspecialidadeId)
            .OnDelete(DeleteBehavior.Restrict);

        

        modelBuilder.Entity<Exame>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.Exames)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Exame>()
            .HasOne(e => e.Paciente)
            .WithMany()
            .HasForeignKey(e => e.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);

        

        modelBuilder.Entity<HistoricoClinico>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.HistoricosClinicos)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HistoricoClinico>()
            .HasOne(x => x.Paciente)
            .WithMany()
            .HasForeignKey(x => x.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        

        modelBuilder.Entity<Receita>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.Receitas)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Receita>()
            .HasOne(r => r.Paciente)
            .WithMany()
            .HasForeignKey(r => r.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);

        

        modelBuilder.Entity<Atestado>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.Atestados)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Atestado>()
            .HasOne(a => a.Paciente)
            .WithMany()
            .HasForeignKey(a => a.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);

        

        modelBuilder.Entity<Odontograma>()
            .HasOne(o => o.Consulta)
            .WithMany()
            .HasForeignKey(o => o.ConsultaId)
            .OnDelete(DeleteBehavior.NoAction);



        modelBuilder.Entity<Odontograma>()
            .HasOne(o => o.Paciente)
            .WithMany()
            .HasForeignKey(o => o.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);

        

        modelBuilder.Entity<PsicologiaRegistro>()
            .HasOne(p => p.Consulta)
            .WithMany()
            .HasForeignKey(p => p.ConsultaId)
            .OnDelete(DeleteBehavior.NoAction);



        modelBuilder.Entity<PsicologiaRegistro>()
            .HasOne(p => p.Paciente)
            .WithMany()
            .HasForeignKey(p => p.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<NutricaoRegistro>()
            .HasOne(n => n.Consulta)
            .WithMany()
            .HasForeignKey(n => n.ConsultaId)
            .OnDelete(DeleteBehavior.NoAction);



        modelBuilder.Entity<NutricaoRegistro>()
            .HasOne(n => n.Paciente)
            .WithMany()
            .HasForeignKey(n => n.PacienteId)
            .OnDelete(DeleteBehavior.NoAction);



        modelBuilder.Entity<NutricaoRegistro>()
            .Property(n => n.Peso)
            .HasPrecision(6, 2);



        modelBuilder.Entity<NutricaoRegistro>()
            .Property(n => n.Altura)
            .HasPrecision(4, 2);



        modelBuilder.Entity<NutricaoRegistro>()
            .Property(n => n.Imc)
            .HasPrecision(5, 2);

        

        modelBuilder.Entity<SolicitacaoExame>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.SolicitacoesExames)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);

        

        modelBuilder.Entity<DeclaracaoComparecimento>()
            .HasOne(x => x.Consulta)
            .WithMany(x => x.DeclaracoesComparecimento)
            .HasForeignKey(x => x.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);

        

        modelBuilder.Entity<Auditoria>(entity =>
        {
            entity.ToTable("Auditorias");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Entidade)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Acao)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Icone)
                 .HasMaxLength(20);

            entity.Property(x => x.Cor)
                .HasMaxLength(20);

            entity.Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.UsuarioNome)
                .HasMaxLength(200);

            entity.Property(x => x.UsuarioRole)
                .HasMaxLength(50);

            entity.Property(x => x.DataHora)
                .HasDefaultValueSql("GETDATE()");
        });

        

        modelBuilder.Entity<ConfiguracaoClinica>(entity =>
        {
            entity.ToTable("ConfiguracoesClinica");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NomePlataforma)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.EmailSuporte)
                .HasMaxLength(200);

            entity.Property(x => x.TelefoneSuporte)
                .HasMaxLength(30);

            entity.Property(x => x.WhatsappSuporte)
                .HasMaxLength(30);

            entity.Property(x => x.RodapePdf)
                .HasMaxLength(1000);

            entity.Property(x => x.MensagemPadrao)
                .HasMaxLength(1000);

            entity.Property(x => x.AtualizadoEm)
                .HasDefaultValueSql("GETUTCDATE()");
        });

    }
}