using System;

namespace GeradorDeArquivoCnab240.Console.Helpers
{
    public static class GerardorDeCnpjHelper
    {
        public static string GeraCnpj()
        {
            var resto = 0;
            var digito1 = 0;
            var digito2 = 0;
            string nDigResult;
            string numerosContatenados;
            string numeroGerado;
            Random numeroAleatorio = new Random();
            // numeros gerados
            var n1 = numeroAleatorio.Next(10);
            var n2 = numeroAleatorio.Next(10);
            var n3 = numeroAleatorio.Next(10);
            var n4 = numeroAleatorio.Next(10);
            var n5 = numeroAleatorio.Next(10);
            var n6 = numeroAleatorio.Next(10);
            var n7 = numeroAleatorio.Next(10);
            var n8 = numeroAleatorio.Next(10);
            var n9 = numeroAleatorio.Next(10);
            var n10 = numeroAleatorio.Next(10);
            var n11 = numeroAleatorio.Next(10);
            var n12 = numeroAleatorio.Next(10);
            var soma = ((n12 * 2)
                        + ((n11 * 3)
                           + ((n10 * 4)
                              + ((n9 * 5)
                                 + ((n8 * 6)
                                    + ((n7 * 7)
                                       + ((n6 * 8)
                                          + ((n5 * 9)
                                             + ((n4 * 2)
                                                + ((n3 * 3)
                                                   + ((n2 * 4)
                                                      + (n1 * 5))))))))))));
            var valor = ((soma / 11)
                         * 11);
            digito1 = (soma - valor);
            // Primeiro resto da divis�o por 11.
            resto = (digito1 % 11);
            if ((digito1 < 2))
            {
                digito1 = 0;
            }
            else
            {
                digito1 = (11 - resto);
            }

            var soma2 = ((digito1 * 2)
                         + ((n12 * 3)
                            + ((n11 * 4)
                               + ((n10 * 5)
                                  + ((n9 * 6)
                                     + ((n8 * 7)
                                        + ((n7 * 8)
                                           + ((n6 * 9)
                                              + ((n5 * 2)
                                                 + ((n4 * 3)
                                                    + ((n3 * 4)
                                                       + ((n2 * 5)
                                                          + (n1 * 6)))))))))))));
            var valor2 = ((soma2 / 11)
                          * 11);
            digito2 = (soma2 - valor2);
            // Primeiro resto da divis�o por 11.
            resto = (digito2 % 11);
            if ((digito2 < 2))
            {
                digito2 = 0;
            }
            else
            {
                digito2 = (11 - resto);
            }

            // Conctenando os numeros
            numerosContatenados = (string.Concat(n1)
                                   + (string.Concat(n2) + ("."
                                                           + (string.Concat(n3)
                                                              + (string.Concat(n4)
                                                                 + (string.Concat(n5) + ("."
                                                                                         + (string.Concat(n6)
                                                                                            + (string.Concat(n7)
                                                                                               + (string.Concat(n8) +
                                                                                                  ("/"
                                                                                                   + (string.Concat(n9)
                                                                                                      + (string.Concat(
                                                                                                             n10)
                                                                                                         + (string
                                                                                                                .Concat(
                                                                                                                    n11)
                                                                                                            + (string
                                                                                                                   .Concat(
                                                                                                                       n12) +
                                                                                                               "-"))))))
                                                                                            )))))))));
            // Concatenando o primeiro resto com o segundo.
            nDigResult = (string.Concat(digito1) + string.Concat(digito2));
            numeroGerado = (numerosContatenados + nDigResult);
            System.Console.WriteLine(("Digito 2 ->" + digito2));
            System.Console.WriteLine(("CNPJ Gerado " + numeroGerado));
            return numeroGerado;
        }
    }
}