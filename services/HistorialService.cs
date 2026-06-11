using SistemaDeInventario.Models;
using SistemaDeInventario.Data;

namespace SistemaDeInventario.Services
{
    public class HistorialService
    {
        private readonly List<Movimiento> historial;
        private const string ARCHIVO = "historial.json";

        public HistorialService()
        {
            historial = JsonDataManager.Cargar<List<Movimiento>>(ARCHIVO) ?? new List<Movimiento>();
        }

        public void RegistrarMovimiento(
            string tipo,
            Producto producto,
            int cantidadAnterior,
            int cantidadNueva,
            string descripcion)
        {
            Movimiento movimiento = new Movimiento
            {
                Fecha = DateTime.Now,
                Tipo = tipo,
                CodigoProducto = producto.Codigo,
                NombreProducto = producto.Nombre,
                CantidadAnterior = cantidadAnterior,
                CantidadNueva = cantidadNueva,
                Descripcion = descripcion
            };

            historial.Add(movimiento);
            Guardar();
        }

        public List<Movimiento> ObtenerHistorial()
        {
            return historial;
        }

        public List<Movimiento> ObtenerHistorialPorFecha(DateTime fecha)
        {
            return historial
                .Where(m => m.Fecha.Date == fecha.Date)
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }

        public List<Movimiento> ObtenerHistorialPorProducto(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Movimiento>();
            }

            textoBusqueda = textoBusqueda.Trim();

            return historial
                .Where(m =>
                    m.CodigoProducto.Equals(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                    ||
                    m.NombreProducto.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase)
                )
                .OrderByDescending(m => m.Fecha)
                .ToList();
        }

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, historial);
        }
    }
}