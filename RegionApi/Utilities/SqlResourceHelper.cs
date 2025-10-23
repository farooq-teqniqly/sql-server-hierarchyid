using System.Reflection;

namespace RegionApi.Utilities
{
    public static class SqlResourceHelper
    {
        public static string ReadEmbeddedSql(string resourceName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(resourceName);

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
