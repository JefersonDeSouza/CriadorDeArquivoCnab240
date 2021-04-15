namespace GeradorDeArquivoCnab240.Console.Extensions
{
    public static class StringExtensions
    {
        public static string DocumentoLimpo(this string texto)
        {
            return texto?.Replace(@".", "").Replace("-", "").Replace(@"/", "").Trim();
        }
    }
}