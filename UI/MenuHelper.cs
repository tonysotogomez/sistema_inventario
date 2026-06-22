namespace SistemaDeInventario.UI
{
    public class MenuHelper
    {
        public bool Continuar(string mensaje)
        {
            Console.WriteLine();
            Console.WriteLine("1. " + mensaje);
            Console.WriteLine("2. Volver");

            int opcion;

            do
            {
                opcion = InputHelper.LeerEntero("Seleccione una opción: ");

                if (opcion != 1 && opcion != 2)
                {
                    Console.WriteLine("Seleccione una opción válida.");
                }

            } while (opcion != 1 && opcion != 2);

            return opcion == 1;
        }
    }
}