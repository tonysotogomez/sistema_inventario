using SistemaDeInventario.Models;
using SistemaDeInventario.Services;

namespace SistemaDeInventario.UI
{
    public class Menu
    {
        private readonly InventarioService inventarioService;
        private readonly VentaService ventaService;
        private readonly HistorialService historialService;

        public Menu()
        {
            inventarioService = new InventarioService();
            ventaService = new VentaService();
            historialService = new HistorialService();
        }

        public void Mostrar()
        {
            int opcion;

            do
            {
                Console.Clear();

                Console.WriteLine("===== SISTEMA DE INVENTARIO =====");
                Console.WriteLine("1. Registrar producto");
                Console.WriteLine("2. Agregar stock");
                Console.WriteLine("3. Registrar venta");
                Console.WriteLine("4. Modificar producto");
                Console.WriteLine("5. Eliminar producto");
                Console.WriteLine("6. Buscar producto");
                Console.WriteLine("7. Mostrar inventario");
                Console.WriteLine("8. Mostrar ventas del día");
                Console.WriteLine("9. Mostrar historial");
                Console.WriteLine("10. Ordenar productos");
                Console.WriteLine("11. Salir");
                //Ya no se muestra la opcion de Guardar Datos porque ahora se guardan automáticamente después de cada operación.

                Console.Write("\nSeleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        RegistrarProducto();
                        break;

                    case 2:
                        AgregarStock();
                        break;

                    case 3:
                        RegistrarVenta();
                        break;

                    case 4:
                        ModificarProducto();
                        break;

                    case 5:
                        EliminarProducto();
                        break;

                    case 6:
                        BuscarProducto();
                        break;

                    case 7:
                        MostrarInventario();
                        break;

                    case 8:
                        MostrarVentasDelDia();
                        break;

                    case 9:
                        MostrarHistorial();
                        break;

                    case 10:
                        OrdenarProductos();
                        break;

                    default: Console.WriteLine("Seleccione una opción válida");break;
                }

                if (opcion != 11)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 11);
        }

        private void RegistrarProducto()
        {
            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();

                Console.Write("Cantidad: ");
                int cantidad = int.Parse(Console.ReadLine());

                Console.Write("Precio: ");
                decimal precio = decimal.Parse(Console.ReadLine());

                Producto producto = new Producto
                {
                    Codigo = codigo,
                    Nombre = nombre,
                    Cantidad = cantidad,
                    Precio = precio
                };

                inventarioService.RegistrarProducto(producto);

                historialService.RegistrarMovimiento(
                    "REGISTRO",
                    producto,
                    0,
                    cantidad,
                    "Producto registrado."
                );

                Console.WriteLine("Producto registrado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AgregarStock()
        {
            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                Producto producto =
                    inventarioService.BuscarProducto(codigo);

                if (producto == null)
                {
                    Console.WriteLine("Producto no encontrado.");
                    return;
                }

                int cantidadAnterior = producto.Cantidad;

                Console.Write("Cantidad a agregar: ");
                int cantidad = int.Parse(Console.ReadLine());

                inventarioService.AgregarStock(codigo, cantidad);

                historialService.RegistrarMovimiento(
                    "ENTRADA",
                    producto,
                    cantidadAnterior,
                    producto.Cantidad,
                    "Stock agregado."
                );

                Console.WriteLine("Stock actualizado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RegistrarVenta()
        {
            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                Producto producto =
                    inventarioService.BuscarProducto(codigo);

                if (producto == null)
                {
                    Console.WriteLine("Producto no encontrado.");
                    return;
                }

                Console.WriteLine($"Producto: {producto.Nombre}");
                Console.WriteLine($"Stock: {producto.Cantidad}");

                int cantidadAnterior = producto.Cantidad;

                Console.Write("Cantidad vendida: ");
                int cantidad = int.Parse(Console.ReadLine());

                ventaService.RegistrarVenta(producto, cantidad);

                historialService.RegistrarMovimiento(
                    "SALIDA",
                    producto,
                    cantidadAnterior,
                    producto.Cantidad,
                    "Venta realizada."
                );

                Console.WriteLine("Venta registrada.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ModificarProducto()
        {
            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                Producto producto =
                    inventarioService.BuscarProducto(codigo);

                if (producto == null)
                {
                    Console.WriteLine("Producto no encontrado.");
                    return;
                }

                Console.WriteLine("1. Modificar nombre");
                Console.WriteLine("2. Modificar cantidad");
                Console.WriteLine("3. Modificar precio");

                Console.Write("Seleccione: ");

                int opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:

                        Console.Write("Nuevo nombre: ");

                        string nombre = Console.ReadLine();

                        inventarioService.ModificarNombre(
                            codigo,
                            nombre
                        );

                        break;

                    case 2:

                        Console.Write("Nueva cantidad: ");

                        int cantidad = int.Parse(Console.ReadLine());

                        inventarioService.ModificarCantidad(
                            codigo,
                            cantidad
                        );

                        break;

                    case 3:

                        Console.Write("Nuevo precio: ");

                        decimal precio = decimal.Parse(Console.ReadLine());

                        inventarioService.ModificarPrecio(
                            codigo,
                            precio
                        );

                        break;
                }

                historialService.RegistrarMovimiento(
                    "MODIFICACION",
                    producto,
                    producto.Cantidad,
                    producto.Cantidad,
                    "Producto modificado."
                );

                Console.WriteLine("Producto actualizado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void EliminarProducto()
        {
            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                Producto producto =
                    inventarioService.BuscarProducto(codigo);

                if (producto == null)
                {
                    Console.WriteLine("Producto no encontrado.");
                    return;
                }

                historialService.RegistrarMovimiento(
                    "ELIMINACION",
                    producto,
                    producto.Cantidad,
                    0,
                    "Producto eliminado."
                );

                inventarioService.EliminarProducto(codigo);

                Console.WriteLine("Producto eliminado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BuscarProducto()
        {
            Console.Write("Ingrese código: ");

            string codigo = Console.ReadLine();

            Producto producto =
                inventarioService.BuscarProducto(codigo);

            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            Console.WriteLine("--------------------------------");

            Console.WriteLine($"Código: {producto.Codigo}");

            Console.WriteLine($"Nombre: {producto.Nombre}");

            Console.WriteLine($"Cantidad: {producto.Cantidad}");

            Console.WriteLine($"Precio: {producto.Precio}");

            Console.WriteLine($"Total: {producto.Total}");
        }

        private void MostrarInventario()
        {
            List<Producto> productos =
                inventarioService.ObtenerProductos();

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos.");
                return;
            }

            foreach (Producto producto in productos)
            {
                Console.WriteLine("--------------------------------");

                Console.WriteLine($"Código: {producto.Codigo}");

                Console.WriteLine($"Nombre: {producto.Nombre}");

                Console.WriteLine($"Cantidad: {producto.Cantidad}");

                Console.WriteLine($"Precio: {producto.Precio}");

                Console.WriteLine($"Total: {producto.Total}");
            }
        }

        private void MostrarVentasDelDia()
        {
            List<Venta> ventas =
                ventaService.ObtenerVentasDelDia();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas hoy.");
                return;
            }

            foreach (Venta venta in ventas)
            {
                Console.WriteLine("--------------------------------");

                Console.WriteLine($"Fecha: {venta.Fecha}");

                Console.WriteLine($"Producto: {venta.NombreProducto}");

                Console.WriteLine($"Cantidad: {venta.CantidadVendida}");

                Console.WriteLine($"Precio: {venta.PrecioUnitario}");

                Console.WriteLine($"Total: {venta.TotalVenta}");
            }
        }

        private void MostrarHistorial()
        {
            List<Movimiento> historial =
                historialService.ObtenerHistorial();

            if (historial.Count == 0)
            {
                Console.WriteLine("No hay historial.");
                return;
            }

            foreach (Movimiento movimiento in historial)
            {
                Console.WriteLine("--------------------------------");

                Console.WriteLine($"Fecha: {movimiento.Fecha}");

                Console.WriteLine($"Tipo: {movimiento.Tipo}");

                Console.WriteLine($"Producto: {movimiento.NombreProducto}");

                Console.WriteLine($"Cantidad anterior: {movimiento.CantidadAnterior}");

                Console.WriteLine($"Cantidad nueva: {movimiento.CantidadNueva}");

                Console.WriteLine($"Descripción: {movimiento.Descripcion}");
            }
        }

        private void OrdenarProductos()
        {
            inventarioService.OrdenarProductos();

            Console.WriteLine("Productos ordenados.");
        }
    }
}