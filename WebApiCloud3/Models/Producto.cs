using System.ComponentModel.DataAnnotations;

namespace WebApiCloud3.Models
{
    public class Producto
    {
        [Key]
        public int id_Producto { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public bool Estado { get; set; }

        // Relación con la categoría
        public Categoria Categoria { get; set; }
    }
}
