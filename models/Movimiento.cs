namespace SistemaDeInventario.Models
{
    public class Movimiento
    {
        public DateTime Fecha { get; set; }

        public string Tipo { get; set; }

        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public int CantidadAnterior { get; set; }

        public int CantidadNueva { get; set; }

        public string Descripcion { get; set; }
    }
}