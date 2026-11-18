
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoBeleza.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int id_cliente { get; set; }

        [Required(ErrorMessage = "Informe o nome do cliente.")]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [StringLength(15)]
        public string? telefone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data_nascimento { get; set; }
    }
}
