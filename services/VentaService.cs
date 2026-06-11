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

        public List<Venta> ObtenerVentasPorFecha(DateTime fecha)
        {
            return ventas
                .Where(v => v.Fecha.Date == fecha.Date)
                .ToList();
        }

        public List<Venta> ObtenerVentasPorProducto(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Venta>();
            }

            textoBusqueda = textoBusqueda.Trim();

            return ventas
                .Where(v =>
                    v.CodigoProducto.Equals(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                    ||
                    v.NombreProducto.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                )
                .OrderByDescending(v => v.Fecha)
                .ToList();
        }

        public List<Venta> ObtenerUltimas5VentasPorProducto(string codigoProducto)
        {
            return ventas
                .Where(v => v.CodigoProducto.Equals(codigoProducto, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(v => v.Fecha)
                .Take(5)
                .ToList();
        }

        public List<Venta> ObtenerVentasPorProductoYFecha(string textoBusqueda, DateTime fecha)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Venta>();
            }

            textoBusqueda = textoBusqueda.Trim();

            return ventas
                .Where(v =>
                    (
                        v.CodigoProducto.Equals(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                        ||
                        v.NombreProducto.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                    )
                    && v.Fecha.Date == fecha.Date
                )
                .OrderByDescending(v => v.Fecha)
                .ToList();
        }

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, ventas);
        }
        
    }
}