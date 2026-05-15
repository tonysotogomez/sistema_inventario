namespace SistemaDeInventario.Models
{
    public class Producto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        public decimal Total => Cantidad * Precio;
    }
}