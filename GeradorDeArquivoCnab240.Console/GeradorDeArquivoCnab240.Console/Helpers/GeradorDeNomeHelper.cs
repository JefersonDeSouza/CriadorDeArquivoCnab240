using System;

namespace GeradorDeArquivoCnab240.Console.Helpers
{
    public static class GeradorDeNomeHelper
    {
        private static string Letras => "ABCDEFGHIJKLMNOPQRSTUVYWXZ";
        private static string Consoantes => "BCDFGHJKLMNPQRSTVYWXZ";
        private static string Vogais => "AEIOU";
        private static decimal TamanhoPalavra => 19;
        public static string GerarNomeAleatorio()
        {
            var random = new Random();
            var armazenaLetra = string.Empty;
            var metadeDaPalavra = Math.Round(TamanhoPalavra / 2);


            while (armazenaLetra.Length <= TamanhoPalavra)
            {
                var indexConsoante = random.Next(Consoantes.Length);
                var indexVogal = random.Next(Vogais.Length);

                var constante = Consoantes.Substring(indexConsoante == 0 ? 1 : indexConsoante, 1);
                var vogal = Vogais.Substring(indexVogal == 0 ? 1 : indexVogal, 1);

                //armazenaLetra += Letras.Substring(indexConsoante + i, indexVogal + i);

                if (metadeDaPalavra == armazenaLetra.Length)
                    armazenaLetra += " ";

                armazenaLetra += $"{constante}{vogal}";

                if (armazenaLetra.Length == TamanhoPalavra)
                    break;
            }
            return armazenaLetra;
        }
    }
}
