
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoBeleza.Models
{
    [Table("Servico")]
    public class Servico
    {
        [Key]
        public int id_servico { get; set; }

        [Required(ErrorMessage = "Informe o nome do serviço.")]
        [StringLength(80)]
        public string nome { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal preco { get; set; }

        [Required]
        public int duracao_min { get; set; }
    }
}
