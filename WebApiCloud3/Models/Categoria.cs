using System.ComponentModel.DataAnnotations;

namespace WebApiCloud3.Models
{
    public class Categoria
    {
        [Key]
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
