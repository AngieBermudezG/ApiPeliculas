using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class PeliculasModels
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string NameMovie { get; set; }
        
        [Required]
        public int CategoriaId { get; set; }
        
        [ForeignKey("CategoriaId")]
        public CategoriaModel CategoriaModel { get; set; }
    }
}