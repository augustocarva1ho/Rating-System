using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAvaliacao.Models
{
    public class Unidades
    {
        [Key]
        public string IdUni { get; set; } = string.Empty;

       
        public string NomeUni { get; set; } = string.Empty;

        public ICollection<Avaliacao>? Avaliacao { get; set; }

    }
    
    public class Tipo_avaliacao{
        [Key]
        public int IdTipo { get; set; }

        public string NomeTipo { get; set; } = string.Empty;

    }

    public class Perguntas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configura a propriedade como auto-incremento
        public int IdPergunta { get; set; }

        public string TextoPergunta { get; set; } = string.Empty;
        
        public bool StatusPergunta { get; set; } 
    }

    public class Avaliacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configura a propriedade como auto-incremento
        public int IdAval { get; set; }
        
        public int idAtendimento { get; set; }

        [Required]
        public string SenhaAval { get; set; } = string.Empty;

        [Required]
        public int NotaAval { get; set; }

        [Required]
        public string UnidadeAval { get; set; } = string.Empty;

        public string DataAval { get; set; } = string.Empty;

        public int TipoAval { get; set; } 

        [ForeignKey("TipoAval")]
        public Tipo_avaliacao? Tipo { get; set; }

        [ForeignKey("UnidadeAval")]
        public Unidades? Unidades { get; set; } 

        [ForeignKey("Perguntas")]
        public int? IdPergunta { get; set; }
        public Perguntas? Perguntas { get; set; }
        
    }
       
    public class Respostas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configura a propriedade como auto-incremento
        public int IdResp { get; set; }

        [Required]
        public int NotaResp { get; set; } 

        [Required]
        public string SenhaResp { get; set; } = string.Empty;

        [Required]
        public string UnidadeResp { get; set; } = string.Empty;

        public string DataResp { get; set; } = string.Empty;

        public int idAvalComp { get; set; }

        [ForeignKey("UnidadeResp")]
        public Unidades? Unidades { get; set; } 

        [ForeignKey("Perguntas")]
        public int IdPergunta { get; set; }
        public Perguntas? Perguntas { get; set; }
    }

    public class RespostaDTO
    {
        public int IdPergunta { get; set; }
        public int NotaAval { get; set; }
        public string SenhaAval { get; set; } = string.Empty;
        public string UnidadeAval { get; set; } = string.Empty;
        public int TipoAval{ get; set; }
    }
    public class AvaliacaoCompletaDTO
    {
        public List<RespostaDTO> Avaliacao { get; set; }
    }
    public class Senhas
    {
        [Key]
        public int IdSenha { get; set; }
        public string Senha { get; set; } = string.Empty;
        public string DataSenha { get; set; } = string.Empty;
        public int StatusSenha { get; set; }
    }
}
