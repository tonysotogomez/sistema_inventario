using SistemaDeInventario.Models;
using SistemaDeInventario.Data;

namespace SistemaDeInventario.Services
{
    public class VentaService
    {
        private readonly List<Venta> ventas;
        private const string ARCHIVO = "ventas.json";
        public VentaService()
        {
            ventas = JsonDataManager.Cargar<List<Venta>>(ARCHIVO) ?? new List<Venta>();
        }

        public void RegistrarVenta(Producto producto, int cantidadVendida)
        {
            if (cantidadVendida > producto.Cantidad)
            {
                throw new Exception("Stock insuficiente.");
            }

            producto.Cantidad -= cantidadVendida;

            Venta venta = new Venta
            {
                Fecha = DateTime.Now,
                CodigoProducto = producto.Codigo,
                NombreProducto = producto.Nombre,
                CantidadVendida = cantidadVendida,
                PrecioUnitario = producto.Precio
            };

            ventas.Add(venta);
            Guardar();
        }

        public List<Venta> ObtenerVentas()
        {
            return ventas;
        }

        public List<Venta> ObtenerVentasDelDia()
        {
            return ventas
                .Where(v => v.Fecha.Date == DateTime.Today)
                .ToList();
        }

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, ventas);
        }
        
    }
}