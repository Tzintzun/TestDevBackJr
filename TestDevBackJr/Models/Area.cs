using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestDevBackJr.Models
{
    [Table("ccRIACat_Areas")]
    public class Area
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IDArea")]
        public int Id { get; set; }

        [Required]
        [Column("AreaName")]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        [Column("StatusArea")]
        public int Status { get; set; }

        [Required]
        [Column("CreateDate")]
        public DateTime CreateDate { get; set; }


    }
}
