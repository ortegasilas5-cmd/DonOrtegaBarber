
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaoBeleza.Models
{
    [Table("Agendamento")]
    public class Agendamento
    {
        [Key]
        public int id_agendamento { get; set; }

        [Required]
        public DateTime data_hora { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; } = "agendado";

        [Required]
        public int id_cliente { get; set; }

        [Required]
        public int id_profissional { get; set; }

        [Required]
        public int id_servico { get; set; }

        [ForeignKey("id_cliente")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("id_profissional")]
        public Profissional? Profissional { get; set; }

        [ForeignKey("id_servico")]
        public Servico? Servico { get; set; }
    }
}
