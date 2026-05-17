using SistemaDeInventario.Models;
using SistemaDeInventario.Data;

namespace SistemaDeInventario.Services
{
    public class InventarioService
    {
        private readonly List<Producto> productos;
        private const string ARCHIVO = "productos.json";

        public InventarioService()
        {
            productos = JsonDataManager.Cargar<List<Producto>>(ARCHIVO) ?? new List<Producto>();
        }

        public void RegistrarProducto(Producto producto)
        {
            bool existe = productos.Any(p => p.Codigo == producto.Codigo);

            if (existe)
            {
                throw new Exception("Ese código ya existe.");
            }

            productos.Add(producto);
            Guardar();
        }

        public Producto BuscarProducto(string codigo)
        {
            return productos.FirstOrDefault(
                p => p.Codigo.ToLower() == codigo.ToLower()
            );
        }

        public List<Producto> ObtenerProductos()
        {
            return productos;
        }

        public void AgregarStock(string codigo, int cantidad)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            producto.Cantidad += cantidad;
            Guardar();
        }

        public void ModificarNombre(string codigo, string nuevoNombre)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            producto.Nombre = nuevoNombre;
            Guardar();
        }

        public void ModificarCantidad(string codigo, int nuevaCantidad)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            producto.Cantidad = nuevaCantidad;
            Guardar();
        }

        public void ModificarPrecio(string codigo, decimal nuevoPrecio)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            producto.Precio = nuevoPrecio;
            Guardar();
        }

        public void EliminarProducto(string codigo)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto == null)
            {
                throw new Exception("Producto no encontrado.");
            }

            productos.Remove(producto);
            Guardar();
        }

        public void OrdenarProductos()
        {
            productos.Sort((a, b) => a.Nombre.CompareTo(b.Nombre));
            Guardar();
        }

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, productos);
        }
    }
}