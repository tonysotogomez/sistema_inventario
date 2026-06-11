using SistemaDeInventario.Services;

namespace SistemaDeInventario.UI
{
    public class Menu
    {
        private readonly ProductoMenu productoMenu;
        private readonly VentaMenu ventaMenu;
        private readonly HistorialMenu historialMenu;

        public Menu()
        {
            InventarioService inventarioService = new InventarioService();
            VentaService ventaService = new VentaService();
            HistorialService historialService = new HistorialService();

            productoMenu = new ProductoMenu(inventarioService, historialService);
            ventaMenu = new VentaMenu(inventarioService, ventaService, historialService);
            historialMenu = new HistorialMenu(inventarioService, ventaService, historialService);
        }

        public void Mostrar()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("----- SISTEMA DE INVENTARIO -----");
                Console.WriteLine("1. Productos");
                Console.WriteLine("2. Ventas");
                Console.WriteLine("3. Historial");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        productoMenu.Mostrar();
                        break;

                    case 2:
                        ventaMenu.Mostrar();
                        break;

                    case 3:
                        historialMenu.Mostrar();
                        break;

                    case 4:
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != 4);
        }
    }
}