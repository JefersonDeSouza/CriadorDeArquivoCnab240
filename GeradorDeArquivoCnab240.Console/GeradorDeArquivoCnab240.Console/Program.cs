using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GeradorDeArquivoCnab240.Console.Entidade;
using GeradorDeArquivoCnab240.Console.Helpers;

namespace GeradorDeArquivoCnab240.Console
{
    class Program
    {
        private static bool ArquivoComFornecedorUnico => true;
        private static string CnpjCliente => "12075667000132";
        private static string IdentificadorClienteArquivo => "DAY12075667000132";
        private static string NomeFornecedor => "TESTE DE FORNECEDOR";
        private static DateTime DataVencimento => DateTime.Now.AddDays(7);
        private static decimal ValorTaxa => 112M;
        private static string CnpjFornecedor { get; set; }

        static void Main(string[] args)
        {
            GerarArquivoCnab240();
        }

        private static void GerarArquivoCnab240()
        {
            var arquivo = LerArquivo();
            System.Console.ReadLine();
        }

        private static StringBuilder LerArquivo()
        {
            StringBuilder arquivo = new StringBuilder();

            try
            {
                string line;
                int counter = 0;
                string path = Path.Combine(Environment.CurrentDirectory, @"Arquivos\ArquivoReferencia\PG000##CODIGOCLIENTE##_##DATAHORA##-##INDICE##.REM");
                var totalLinhas = File.ReadAllLines(path).Count();

                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        var nwLine = SubstituirValores(line, totalLinhas);
                        arquivo.AppendLine(nwLine);
                    }
                }

                var arquivoSemLinhasVazias = Regex.Replace(arquivo.ToString(), @"^\s+$[\r\n\r\n]*", string.Empty,
                    RegexOptions.Multiline);
                var pathDestino = Environment.CurrentDirectory.Split("bin").First() + "Arquivos";

                for (var i = 0; i < 5; i++)
                {
                    using (var sw = File.CreateText(Path.Combine(pathDestino,
                        GeradorHelper.ObterNomeArquivo(pathDestino, IdentificadorClienteArquivo))))
                    {
                        sw.Write(arquivoSemLinhasVazias);
                    }
                }

                System.Console.WriteLine("{0} linhas.", counter);
                return arquivo;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"não foi possível ler arquivo. Erro: {ex}");
                throw;
            }
        }

        private static string SubstituirValores(string linha, int totalLinhas)
        {
            if (string.IsNullOrEmpty(linha))
                return string.Empty;

            linha = SetarDadosHeader(linha);
            linha = SetarDadosSegmentoA(linha, totalLinhas);

            return linha;
        }

        private static string SetarDadosHeader(string linha)
        {
            var dadosHeaderArquivo = new DadosHeaderArquivo(CnpjCliente);
            var dadosHeaderLote = new DadosHeaderLote(CnpjCliente);

            if (!dadosHeaderArquivo.IsValid)
                throw new Exception("Header arquivo inválido.");
            if (!dadosHeaderLote.IsValid)
                throw new Exception("Header do lote inválido.");

            linha = linha.Replace("##CNPJ_CLIENTE##", dadosHeaderArquivo.CnpjCliente);
            linha = linha.Replace("##DATA_OPERACAO##", dadosHeaderArquivo.DataOperacao.ToString("ddMMyyyy"));

            return linha;
        }

        private static string SetarDadosSegmentoA(string linha, int totalLinhas)
        {
            if (ArquivoComFornecedorUnico && string.IsNullOrEmpty(CnpjFornecedor))
            {
                CnpjFornecedor = GerardorDeCnpjHelper.GeraCnpj().CnpjLimpo();
            }
            else if (!ArquivoComFornecedorUnico)
            {
                CnpjFornecedor = GerardorDeCnpjHelper.GeraCnpj().CnpjLimpo();
            }

            var dadosSegmentoA = new DetalheSegmentoA(NomeFornecedor,
                NotaFiscalHelper.GerarNumeroNotaFiscalAleatorio(), 1500, CnpjFornecedor.CnpjLimpo(),
                DataVencimento, totalLinhas, ValorTaxa);

            if (!dadosSegmentoA.IsValid)
                throw new Exception("Segmento A inválido.");

            linha = linha.Replace("##NOME_FORNECEDOR##", dadosSegmentoA.NomeFornecedor);
            linha = linha.Replace("##NUM_NOTA##", dadosSegmentoA.NumeroNota);
            linha = linha.Replace("##DATA_VENCIMENTO##",dadosSegmentoA.DataVencimento.ToString(CultureInfo.InvariantCulture));
            linha = linha.Replace("##VALOR_TAXA##", dadosSegmentoA.ValorTaxa.ToString(CultureInfo.InvariantCulture));
            linha = linha.Replace("##VALOR_PGTO##", dadosSegmentoA.ValorPgto.ToString(CultureInfo.InvariantCulture));
            linha = linha.Replace("##CNPJ_FORNECEDOR##", dadosSegmentoA.CnpjFornecedor);
            linha = linha.Replace("##TOTAL_PGTO##",dadosSegmentoA.ValorTotalPgto.ToString(CultureInfo.InvariantCulture));

            return linha;
        }
    }
}