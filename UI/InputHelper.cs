namespace SistemaDeInventario.UI
{
    public static class InputHelper
    {
        public static string LeerTexto(string mensaje)
        {
            string valor;

            do
            {
                Console.Write(mensaje);

                valor = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(valor))
                {
                    Console.WriteLine(
                        "El campo no puede estar vacío."
                    );
                }

            } while (string.IsNullOrWhiteSpace(valor));

            return valor;
        }

        public static int LeerEntero(string mensaje)
        {
            int valor;

            while (true)
            {
                Console.Write(mensaje);

                string entrada =
                    (Console.ReadLine() ?? "").Trim();

                if (!int.TryParse(
                        entrada,
                        out valor))
                {
                    Console.WriteLine(
                        "Ingrese un número válido."
                    );

                    continue;
                }

                if (valor < 0)
                {
                    Console.WriteLine(
                        "El valor no puede ser negativo."
                    );

                    continue;
                }

                return valor;
            }
        }

        public static decimal LeerDecimal(string mensaje)
        {
            decimal valor;

            while (true)
            {
                Console.Write(mensaje);

                string entrada =
                    (Console.ReadLine() ?? "").Trim();

                if (!decimal.TryParse(
                        entrada,
                        out valor))
                {
                    Console.WriteLine(
                        "Ingrese un número válido."
                    );

                    continue;
                }

                if (valor < 0)
                {
                    Console.WriteLine(
                        "El valor no puede ser negativo."
                    );

                    continue;
                }

                return valor;
            }
        }
    }
}