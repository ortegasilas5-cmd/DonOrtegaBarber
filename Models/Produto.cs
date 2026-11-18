
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoBeleza.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Key]
        public int id_produto { get; set; }

        [Required(ErrorMessage = "Informe o nome do produto.")]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [StringLength(60)]
        public string? marca { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal preco { get; set; }

        [Required]
        public int estoque { get; set; }
    }
}
