using Newtonsoft.Json;

namespace SistemaDeInventario.Data
{
    public static class JsonDataManager
    {
        public static void Guardar<T>(string archivo, T datos)
        {
            string json =
                JsonConvert.SerializeObject(
                    datos,
                    Formatting.Indented
                );

            File.WriteAllText(archivo, json);
        }

        public static T Cargar<T>(string archivo)
        {
            if (!File.Exists(archivo))
            {
                return default;
            }

            string json = File.ReadAllText(archivo);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}