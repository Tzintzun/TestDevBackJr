using System.ComponentModel.DataAnnotations;

namespace TestDevBackJr.Models.Dto
{
    public class LoginDto
    {
        
        public int? Id { get; set; }

        [Required(ErrorMessage ="El ID del usuario es obligatorio")]
        public int? UserId { get; set; }

        [Required (ErrorMessage = "La Extencion es obligatoria")]
        public int? Extension { get; set; }

        [Required (ErrorMessage = "Se requiere especificar el tipo de movimiento 0 = login, 1 = logout")]
        [Range(0,1, ErrorMessage ="El valor del tipo de movimiento tiene que estar entre 0 = login y 1 = logout")]
        public int? TipoMov { get; set; }

        [Required(ErrorMessage = "Se requiere especificar la fecha y hora del movimiento.")]
        public DateTime? Fecha { get; set; }
    }
}
