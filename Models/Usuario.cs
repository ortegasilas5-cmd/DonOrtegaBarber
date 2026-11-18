
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SalaoBeleza.Models
{
    public class Usuario : IdentityUser<int>
    {
        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Ativo";
    }
}
