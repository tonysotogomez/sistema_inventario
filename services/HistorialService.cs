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

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, historial);
        }
    }
}