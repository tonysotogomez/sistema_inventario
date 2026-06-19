using SistemaDeInventario.Models;
using SistemaDeInventario.Services;

namespace SistemaDeInventario.UI
{
    public class ProductoMenu
    {
        private readonly InventarioService inventarioService;
        private readonly HistorialService historialService;
        private readonly MenuHelper menuHelper;

        public ProductoMenu(
            InventarioService inventarioService,
            HistorialService historialService)
        {
            this.inventarioService = inventarioService;
            this.historialService = historialService;
            menuHelper = new MenuHelper();
        }


        public void Mostrar()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("\n----- PRODUCTOS -----");
                Console.WriteLine("1. Registrar producto");
                Console.WriteLine("2. Agregar stock");
                Console.WriteLine("3. Modificar producto");
                Console.WriteLine("4. Eliminar producto");
                Console.WriteLine("5. Buscar producto");
                Console.WriteLine("6. Inventario");
                Console.WriteLine("7. Volver");
                Console.Write("Seleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        do
                        {
                            RegistrarProducto();
                        } while (menuHelper.Continuar("¿Desea registrar otro producto?"));
                        break;

                    case 2:
                        do
                        {
                            AgregarStock();
                        } while (menuHelper.Continuar("¿Desea agregar stock a otro producto?"));
                        break;

                    case 3:
                        do
                        {
                            ModificarProducto();
                        } while (menuHelper.Continuar("¿Desea modificar otro producto?"));
                        break;

                    case 4:
                        EliminarProducto();
                        break;

                    case 5:
                        do
                        {
                            BuscarProducto();
                        } while (menuHelper.Continuar("¿Desea buscar otro producto?"));
                        break;

                    case 6:
                        MenuInventario();
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (opcion != 7)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 7);
        }
        //funciones producto

        private void RegistrarProducto()
        {
            try
            {
                string codigo;

                do
                {
                    codigo =
                        InputHelper.LeerTexto("Código: ");

                    if (inventarioService.ExisteCodigo(codigo))
                    {
                        Console.WriteLine("Ese código ya existe.");
                        codigo = "";
                    }

                } while (string.IsNullOrWhiteSpace(codigo));

                string nombre;

                do
                {
                    nombre =
                        InputHelper.LeerTexto("Nombre: ");

                    if (inventarioService.ExisteNombre(nombre))
                    {
                        Console.WriteLine("Ese nombre de producto ya existe.");
                        nombre = "";
                    }

                } while (string.IsNullOrWhiteSpace(nombre));

                int cantidad;

                do
                {
                    cantidad =
                        InputHelper.LeerEntero("Cantidad: ");

                    if (cantidad <= 0)
                    {
                        Console.WriteLine(
                            "La cantidad inicial del producto debe ser mayor que cero."
                        );
                    }

                } while (cantidad <= 0);

                decimal precio;

                do
                {
                    precio =
                        InputHelper.LeerDecimal("Precio: ");

                    if (precio <= 0)
                    {
                        Console.WriteLine(
                            "El precio del producto debe ser mayor que cero."
                        );
                    }

                } while (precio <= 0);

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
                Producto? producto =
                    BuscarProducto();

                if (producto == null)
                {
                    return;
                }

                int cantidad =
                    InputHelper.LeerEntero(
                        "Cantidad a agregar: ");

                if (cantidad <= 0)
                {
                    Console.WriteLine(
                        "La cantidad debe ser mayor que cero."
                    );

                    return;
                }

                int cantidadAnterior =
                    producto.Cantidad;

                inventarioService.AgregarStock(
                    producto.Codigo,
                    cantidad
                );

                historialService
                    .RegistrarMovimiento(
                        "ENTRADA",
                        producto,
                        cantidadAnterior,
                        producto.Cantidad,
                        "Stock agregado."
                    );

                Console.WriteLine(
                    "Stock actualizado."
                );
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
                Producto? producto = BuscarProducto();

                if (producto == null)
                {
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("1. Modificar nombre");
                Console.WriteLine("2. Modificar cantidad");
                Console.WriteLine("3. Modificar precio");

                int opcion =
                    InputHelper.LeerEntero(
                        "Seleccione: ");

                switch (opcion)
                {
                    case 1:

                        string nombre =
                            InputHelper.LeerTexto(
                                "Nuevo nombre: ");

                        inventarioService
                            .ModificarNombre(
                                producto.Codigo,
                                nombre
                            );

                        break;

                    case 2:

                        int cantidad =
                            InputHelper.LeerEntero(
                                "Nueva cantidad: ");

                        inventarioService
                            .ModificarCantidad(
                                producto.Codigo,
                                cantidad
                            );

                        break;

                    case 3:

                        decimal precio =
                            InputHelper.LeerDecimal(
                                "Nuevo precio: ");

                        inventarioService
                            .ModificarPrecio(
                                producto.Codigo,
                                precio
                            );

                        break;

                    default:

                        Console.WriteLine(
                            "Opción inválida."
                        );

                        return;
                }

                historialService
                    .RegistrarMovimiento(
                        "MODIFICACION",
                        producto,
                        producto.Cantidad,
                        producto.Cantidad,
                        "Producto modificado."
                    );

                Console.WriteLine(
                    "Producto actualizado."
                );
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
                Producto? producto = BuscarProducto();

                if (producto == null)
                {
                    return;
                }

                string respuesta =
                    InputHelper.LeerTexto(
                        "¿Desea eliminar el producto? (S/N): "
                    );

                if (respuesta.ToUpper() != "S")
                {
                    Console.WriteLine(
                        "Operación cancelada."
                    );

                    return;
                }

                historialService
                    .RegistrarMovimiento(
                        "ELIMINACION",
                        producto,
                        producto.Cantidad,
                        0,
                        "Producto eliminado."
                    );

                inventarioService
                    .EliminarProducto(
                        producto.Codigo
                    );

                Console.WriteLine(
                    "Producto eliminado."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void MostrarProducto(Producto producto)
        {
            Console.WriteLine("--------------------------------");

            Console.WriteLine($"Código: {producto.Codigo}");

            Console.WriteLine($"Nombre: {producto.Nombre}");

            Console.WriteLine($"Cantidad: {producto.Cantidad}");

            Console.WriteLine($"Precio: {producto.Precio}");

            Console.WriteLine($"Total: {producto.Total}");
        }

        private Producto? BuscarProducto()
        {
            string textoBusqueda =
                InputHelper.LeerTexto("Ingrese código o nombre del producto: ");

            List<Producto> resultados =
                inventarioService.BuscarProductos(textoBusqueda);

            if (resultados.Count == 0)
            {
                Console.WriteLine("Producto no encontrado.");
                return null;
            }

            if (resultados.Count == 1)
            {
                MostrarProducto(resultados[0]);
                return resultados[0];
            }

            Console.WriteLine("\nSe encontraron coincidencias en los productos:");
            Console.WriteLine("-------------------------------------------------");

            for (int i = 0; i < resultados.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {resultados[i].Nombre}");
            }

            int opcion =
                InputHelper.LeerEntero("\nSeleccione el número del producto: ");

            if (opcion < 1 || opcion > resultados.Count)
            {
                Console.WriteLine("Opción fuera de rango.");
                return null;
            }

            Producto productoSeleccionado = resultados[opcion - 1];

            MostrarProducto(productoSeleccionado);

            return productoSeleccionado;
        }






        private void MenuInventario()
        {
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("----- INVENTARIO -----");
                Console.WriteLine();
                Console.WriteLine("1. Mostrar inventario");
                Console.WriteLine("2. Mostrar productos con bajo stock");
                Console.WriteLine("3. Mostrar productos sin stock");
                Console.WriteLine("4. Mostrar productos con mayor demanda");
                Console.WriteLine("5. Volver");
                Console.Write("Seleccione una opción: ");

                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        MostrarInventario();
                        break;

                    case 2:
                        MostrarProductosConBajoStock();
                        break;

                    case 3:
                        MostrarProductosSinStock();
                        break;

                    case 4:
                        MostrarProductosConMayorDemanda();
                        break;

                    case 5:
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcion != 5);
        }

        private void MostrarProductosConMayorDemanda()
        {
            List<ProductoDemanda> productos =
                inventarioService.ObtenerProductosConMayorDemanda();

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas para calcular la demanda.");
                return;
            }

            Console.WriteLine("----- PRODUCTOS CON MAYOR DEMANDA -----");

            for (int i = 0; i < productos.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {productos[i].Nombre} - {productos[i].CantidadTotalVendida} unidades vendidas"
                );
            }
        }

        private void MostrarProductosSinStock()
        {
            List<Producto> productos =
                inventarioService.ObtenerProductosSinStock();

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos sin stock.");
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
                Console.WriteLine("\n--------------------------------");
            }
        }


        private void MostrarProductosConBajoStock()
        {
            List<Producto> productos =
                inventarioService.ObtenerProductosConBajoStock();

            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos con bajo stock.");
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
                Console.WriteLine("--------------------------------");
            }

            MostrarResumenInventario(productos);
        }

        private void MostrarResumenInventario(List<Producto> productos)

        {

            decimal valorTotalInventario =

                productos.Sum(p => p.Total);



            Console.WriteLine("\nRESUMEN DEL INVENTARIO");

            Console.WriteLine($"Productos registrados : {productos.Count}");

            Console.WriteLine($"Valor total del inventario : S/. {valorTotalInventario:N2}");

        }
    }
}