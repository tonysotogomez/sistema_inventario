using SistemaDeInventario.Models;
using SistemaDeInventario.Services;

namespace SistemaDeInventario.UI
{
    public class VentaMenu
    {
        private readonly InventarioService inventarioService;
        private readonly VentaService ventaService;
        private readonly HistorialService historialService;
        private readonly MenuHelper menuHelper;

        public VentaMenu(
        InventarioService inventarioService,
        VentaService ventaService,
        HistorialService historialService)
        {
            this.inventarioService = inventarioService;
            this.ventaService = ventaService;
            this.historialService = historialService;
            menuHelper = new MenuHelper();
        }

        public void Mostrar()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("\n----- VENTAS -----");
                Console.WriteLine("1. Registrar venta");
                Console.WriteLine("2. Ver ventas de hoy");
                Console.WriteLine("3. Buscar ventas por fecha");
                Console.WriteLine("4. Buscar ventas por producto");
                Console.WriteLine("5. Buscar ventas por producto y fecha");
                Console.WriteLine("6. Volver");
                Console.Write("\nSeleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        do
                        {
                            RegistrarVenta();
                        } while (menuHelper.Continuar("¿Desea registrar otra venta?"));
                        break;

                    case 2:
                        MostrarVentasDelDia();
                        break;

                    case 3:
                        do
                        {
                            MostrarVentasPorFecha();
                        } while (menuHelper.Continuar("¿Desea buscar otra fecha?"));
                        break;

                    case 4:
                        do
                        {
                            MostrarVentasPorProducto();
                        } while (menuHelper.Continuar("¿Desea buscar otro producto?"));
                        break;

                    case 5:
                        MostrarVentasPorProductoYFecha();
                        break;

                    case 6:
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (opcion != 6)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 6);
        }

        //funciones ventas
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

        private void MostrarVentasPorFecha()
        {
            Console.Write("Ingrese la fecha a consultar (dd/mm/yyyy): ");
            string fechaTexto = Console.ReadLine();

            if (!DateTime.TryParse(fechaTexto, out DateTime fecha))
            {
                Console.WriteLine("Fecha inválida. Ingrese una fecha válida.");
                return;
            }

            List<Venta> ventasEncontradas =
                ventaService.ObtenerVentasPorFecha(fecha);

            if (ventasEncontradas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas en esa fecha.");
                return;
            }

            Console.WriteLine($"\nVENTAS DEL DÍA {fecha:dd/MM/yyyy}:");

            foreach (Venta venta in ventasEncontradas)
            {
                Console.WriteLine("--------------------------------");

                Console.WriteLine($"Fecha: {venta.Fecha}");

                Console.WriteLine($"Producto: {venta.NombreProducto}");

                Console.WriteLine($"Cantidad: {venta.CantidadVendida}");

                Console.WriteLine($"Precio: {venta.PrecioUnitario}");

                Console.WriteLine($"Total: {venta.TotalVenta}");
            }
        }

        private void MostrarVentas(List<Venta> ventas)
        {
            foreach (Venta venta in ventas)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine($"Código: {venta.CodigoProducto}");
                Console.WriteLine($"Fecha: {venta.Fecha:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Total: S/ {venta.TotalVenta}");
            }
        }

        private void MostrarVentasPorProductoYFecha()
        {
            Console.Write("Ingrese el código del producto: ");
            string codigo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("Debe ingresar un código.");
                return;
            }

            Console.Write("Ingrese la fecha (dd/mm/yyyy): ");
            string fechaTexto = Console.ReadLine();

            if (!DateTime.TryParse(fechaTexto, out DateTime fecha))
            {
                Console.WriteLine("Fecha inválida.");
                return;
            }

            List<Venta> ventas =
                ventaService.ObtenerVentasPorProductoYFecha(
                    codigo,
                    fecha
                );

            if (ventas.Count == 0)
            {
                Console.WriteLine(
                    "No existen ventas para ese producto en esa fecha."
                );
                return;
            }

            MostrarVentas(ventas);
        }


        private void MostrarVentasPorProducto()
        {
            Console.Write("Ingrese código o nombre del producto: ");
            string textoBusqueda = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                Console.WriteLine("Debe ingresar un código o nombre del producto.");
                return;
            }

            textoBusqueda = textoBusqueda.Trim();

            int opcion;

            do
            {
                Console.WriteLine("\n--- CONSULTAR VENTAS DE UN PRODUCTO ---");
                Console.WriteLine("1. Ver últimas 5 ventas del producto");
                Console.WriteLine("2. Buscar ventas del producto por fecha");
                Console.WriteLine("3. Volver");
                Console.Write("Seleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        List<Venta> ultimasVentas =
                            ventaService.ObtenerUltimas5VentasPorProducto(textoBusqueda);

                        if (ultimasVentas.Count == 0)
                        {
                            Console.WriteLine("No hay ventas registradas para ese producto.");
                            break;
                        }

                        Console.WriteLine("\nÚLTIMAS 5 VENTAS DEL PRODUCTO:");
                        MostrarVentas(ultimasVentas);
                        break;

                    case 2:
                        Console.Write("Ingrese la fecha (dd/mm/yyyy): ");
                        string fechaTexto = Console.ReadLine();

                        if (!DateTime.TryParse(fechaTexto, out DateTime fecha))
                        {
                            Console.WriteLine("Fecha inválida.");
                            break;
                        }

                        List<Venta> ventasPorFecha =
                            ventaService.ObtenerVentasPorProductoYFecha(textoBusqueda, fecha);

                        if (ventasPorFecha.Count == 0)
                        {
                            Console.WriteLine("No existen ventas para ese producto en esa fecha.");
                            break;
                        }

                        Console.WriteLine($"\nVENTAS DEL PRODUCTO EN LA FECHA {fecha:dd/MM/yyyy}:");
                        MostrarVentas(ventasPorFecha);
                        break;

                    case 3:
                        Console.WriteLine("Volviendo al menú anterior...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida. Intente nuevamente.");
                        break;
                }

            } while (opcion != 3);
        }

    }

}