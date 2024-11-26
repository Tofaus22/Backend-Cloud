using System.ComponentModel.DataAnnotations;

namespace WebApiCloud3.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Token { get; set; }
    }
}
