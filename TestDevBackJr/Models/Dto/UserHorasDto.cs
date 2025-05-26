using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJr.Models.Dto
{
    public class UserHorasDto
    {
        [Column("User_id")]
        public int Id { get; set; }

        [Column("horasTotales")]
        public int Horas {  get; set; }
    }
}
