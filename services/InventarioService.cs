using SistemaDeInventario.Models;
using SistemaDeInventario.Data;

namespace SistemaDeInventario.Services
{
    public class InventarioService
    {
        private readonly List<Producto> productos;
        private const string ARCHIVO = "productos.json";
        private readonly VentaService ventaService;

        public InventarioService()
        {
            productos = JsonDataManager.Cargar<List<Producto>>(ARCHIVO) ?? new List<Producto>();
            ventaService = new VentaService();
        }

        public void RegistrarProducto(Producto producto)
        {
            if (productos.Any(p =>
                p.Codigo.Equals(producto.Codigo, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Ese código ya existe.");
            }

            if (productos.Any(p =>
                p.Nombre.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Ese nombre de producto ya existe.");
            }

            productos.Add(producto);
            Guardar();
        }

        public bool ExisteCodigo(string codigo)
        {
            return productos.Any(p =>
                p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        }

        public bool ExisteNombre(string nombre)
        {
            return productos.Any(p =>
                p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        public Producto? BuscarProducto(string codigo)
        {
            return productos.FirstOrDefault(
                p => p.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)
            );
        }

        public List<Producto> BuscarProductos(string textoBusqueda)
        {
            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                return new List<Producto>();
            }

            textoBusqueda = textoBusqueda.Trim();

            Producto? productoPorCodigo = productos.FirstOrDefault(
                p => p.Codigo.Equals(textoBusqueda, StringComparison.OrdinalIgnoreCase)
            );

            if (productoPorCodigo != null)
            {
                return new List<Producto> { productoPorCodigo };
            }

            return productos
                .Where(p => p.Nombre.Contains(textoBusqueda, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Nombre)
                .ToList();
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

        public List<ProductoDemanda> ObtenerProductosConMayorDemanda()
        {
            List<Venta> ventas = ventaService.ObtenerVentas();
            return ventas
                .GroupBy(v => v.CodigoProducto)
                .Select(grupo =>
                {
                    Producto producto = productos.FirstOrDefault(
                        p => p.Codigo.Equals(
                            grupo.Key,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );

                    return new ProductoDemanda
                    {
                        Codigo = grupo.Key,
                        Nombre = producto != null ? producto.Nombre : "Producto no encontrado",
                        CantidadTotalVendida = grupo.Sum(v => v.CantidadVendida)
                    };
                })
                .OrderByDescending(p => p.CantidadTotalVendida)
                .ToList();
        }

        public List<Producto> ObtenerProductosConBajoStock()
        {
            return productos
                .Where(p => p.Cantidad >= 1 && p.Cantidad <= 5)
                .OrderBy(p => p.Cantidad)
                .ThenBy(p => p.Nombre)
                .ToList();
        }

        public List<Producto> ObtenerProductosSinStock()
        {
            return productos
                .Where(p => p.Cantidad == 0)
                .OrderBy(p => p.Nombre)
                .ToList();
        }

        private void Guardar()
        {
            JsonDataManager.Guardar(ARCHIVO, productos);
        }
    }
}