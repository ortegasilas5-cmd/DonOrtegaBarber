
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoBeleza.Models
{
    [Table("Profissional")]
    public class Profissional
    {
        [Key]
        public int id_profissional { get; set; }

        [Required(ErrorMessage = "Informe o nome do profissional.")]
        [StringLength(100)]
        public string nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a especialidade.")]
        [StringLength(80)]
        public string especialidade { get; set; } = string.Empty;

        [StringLength(11)]
        public string? cpf { get; set; }
    }
}
