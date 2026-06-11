using SistemaDeInventario.Models;
using SistemaDeInventario.Services;

namespace SistemaDeInventario.UI
{
    public class HistorialMenu
    {
        private readonly InventarioService inventarioService;
        private readonly VentaService ventaService;
        private readonly HistorialService historialService;
        private readonly MenuHelper menuHelper;


        public HistorialMenu(
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
                Console.WriteLine("----- HISTORIAL -----");
                Console.WriteLine();
                Console.WriteLine("1. Mostrar historial");
                Console.WriteLine("2. Buscar historial por fecha");
                Console.WriteLine("3. Buscar historial por producto");
                Console.WriteLine("4. Volver");
                Console.Write("Seleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        MostrarHistorial();
                        break;

                    case 2:
                        MostrarHistorialPorFecha();
                        break;

                    case 3:
                        MostrarHistorialPorProducto();
                        break;

                    case 4:
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (opcion != 4)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 4);
        }

        //funcion historial
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

        private void MostrarHistorialPorFecha()
        {
            Console.Write("Ingrese la fecha (dd/MM/yyyy): ");
            string fechaTexto = Console.ReadLine();

            if (!DateTime.TryParse(fechaTexto, out DateTime fecha))
            {
                Console.WriteLine("Fecha inválida.");
                return;
            }

            List<Movimiento> movimientos =
                historialService.ObtenerHistorialPorFecha(fecha);

            if (movimientos.Count == 0)
            {
                Console.WriteLine("No hay movimientos registrados en esa fecha.");
                return;
            }

            MostrarMovimientos(movimientos);
        }

        private void MostrarHistorialPorProducto()
        {
            Producto producto = SeleccionarProductoParaHistorial();

            if (producto == null)
            {
                return;
            }

            List<Movimiento> movimientos =
                historialService.ObtenerHistorialPorProducto(producto.Codigo);

            if (movimientos.Count == 0)
            {
                Console.WriteLine("Este producto no tiene historial registrado.");
                return;
            }

            MostrarMovimientos(movimientos);
        }

        private Producto SeleccionarProductoParaHistorial()
        {
            Console.Write("Ingrese código o nombre del producto: ");
            string textoBusqueda = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                Console.WriteLine("Debe ingresar un código o nombre.");
                return null;
            }

            List<Producto> resultados =
                inventarioService.BuscarProductos(textoBusqueda);

            if (resultados.Count == 0)
            {
                Console.WriteLine("Producto no encontrado.");
                return null;
            }

            if (resultados.Count == 1)
            {
                return resultados[0];
            }

            Console.WriteLine("\nSe encontraron coincidencias:");
            Console.WriteLine("--------------------------------");

            for (int i = 0; i < resultados.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {resultados[i].Nombre}");
            }

            Console.Write("\nSeleccione el número del producto: ");
            string opcionTexto = Console.ReadLine();

            if (!int.TryParse(opcionTexto, out int opcion))
            {
                Console.WriteLine("Opción inválida. Debe ingresar un número.");
                return null;
            }

            if (opcion < 1 || opcion > resultados.Count)
            {
                Console.WriteLine("Opción fuera de rango.");
                return null;
            }

            return resultados[opcion - 1];
        }

        private void MostrarMovimientos(List<Movimiento> movimientos)
        {
            foreach (Movimiento movimiento in movimientos)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Fecha: {movimiento.Fecha:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Tipo: {movimiento.Tipo}");
                Console.WriteLine($"Código: {movimiento.CodigoProducto}");
                Console.WriteLine($"Producto: {movimiento.NombreProducto}");
                Console.WriteLine($"Cantidad anterior: {movimiento.CantidadAnterior}");
                Console.WriteLine($"Cantidad nueva: {movimiento.CantidadNueva}");
                Console.WriteLine($"Descripción: {movimiento.Descripcion}");
            }

            Console.WriteLine("--------------------------------");
        }

    }
}
