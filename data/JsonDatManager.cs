using Newtonsoft.Json;

namespace SistemaDeInventario.Data
{
    public static class JsonDataManager
    {
        private static readonly string carpetaData = "data";

        static JsonDataManager()
        {
            if (!Directory.Exists(carpetaData))
            {
                Directory.CreateDirectory(carpetaData);
            }
        }
        public static void Guardar<T>(string archivo, T datos)
        {
            string rutaCompleta = Path.Combine(carpetaData, archivo);

            string json =
                JsonConvert.SerializeObject(
                    datos,
                    Formatting.Indented
                );

            File.WriteAllText(rutaCompleta, json);
        }

        public static T Cargar<T>(string archivo)
        {
            string rutaCompleta = Path.Combine(carpetaData, archivo);

            if (!File.Exists(rutaCompleta))
            {
                return default;
            }

            string json = File.ReadAllText(rutaCompleta);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}