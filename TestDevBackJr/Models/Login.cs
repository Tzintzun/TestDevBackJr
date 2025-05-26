using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJr.Models
{
    [Table("ccloglogin")]
    public class Login
    {

        public int Id { get; set; }

        [Column("User_id")]
        public int UserId { get; set; }
        public int Extension { get; set; }
        public int TipoMov { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Login()
        {
            Fecha = DateTime.Now;
        }
    }
}
