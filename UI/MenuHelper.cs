namespace SistemaDeInventario.UI
{
    public class MenuHelper
    {
        public bool Continuar(string mensaje)
        {
            Console.WriteLine();
            Console.WriteLine("1. " + mensaje);
            Console.WriteLine("2. Volver");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine() ?? "";

            return opcion == "1";
        }
    }
}