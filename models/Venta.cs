namespace SistemaDeInventario.Models
{
    public class Venta
    {
        public DateTime Fecha { get; set; }

        public string CodigoProducto { get; set; }

        public string NombreProducto { get; set; }

        public int CantidadVendida { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal TotalVenta => CantidadVendida * PrecioUnitario;
    }
}