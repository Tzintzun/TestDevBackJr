using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJr.Models
{
    [Table("ccUsers")]
    public class User
    {
        [Key]
        [Column("User_id")]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombres { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApellidoPaterno { get; set; }


        [MaxLength(50)]
        public string ApellidoMaterno { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Column("TipoUser_id")]
        public int TipoUserId { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [Column("fCreate")]
        public DateTime FCreate { get; set; }

        [Required]
        [Column("IDArea")]
        public int AreaId { get; set; }

        [Required]
        public DateTime LastLoginAttempt { get; set; }

        [ForeignKey("AreaId")]
        public Area Area { get; set; }

        
    }
}
