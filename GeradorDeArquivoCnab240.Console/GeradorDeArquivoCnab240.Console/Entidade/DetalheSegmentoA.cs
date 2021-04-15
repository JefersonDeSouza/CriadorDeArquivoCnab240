using System;
using System.Globalization;
using GeradorDeArquivoCnab240.Console.Validadores;

namespace GeradorDeArquivoCnab240.Console.Entidade
{
    public class DetalheSegmentoA
    {
        public string NomeFornecedor { get; set; }
        public string NumeroNota { get; set; }
        public string ValorPgto { get; set; }
        public string ValorTotalPgto { get; set; }
        public string CnpjFornecedor { get; set; }
        public string DataVencimento { get; set; }

        public string ValorTaxa { get; set; }

        private const int LengthNomeFornecedor = 30;
        private const int LengthNumeroNota = 20;
        private const int LengthDocumentoFornecedor = 14;
        private const int LengthValorPgto = 15;
        private const int LengthTotalValorPgto = 18;
        private const int LengthTotalValorTaxa = 15;

        private const int TotalHeaderArquivo = 2;
        private const int TotalTrailerArquivo = 2;
        private const decimal TotalSegmentoArquivo = 2;

        public bool IsValid
        {
            get => true;
            private set { }
        }

        public DetalheSegmentoA(string nomeFornecedor, string numeroNota, decimal valorPgto, string cnpjFornecedor,
            DateTime dataVencimento, int totalLinhaArquivo, decimal valorTaxa)
        {
            NomeFornecedor = nomeFornecedor;
            NumeroNota = numeroNota;
            ValorPgto = valorPgto.ToString();
            CnpjFornecedor = ObterDocumentoFornecedor(cnpjFornecedor);
            DataVencimento = dataVencimento.Date.ToString("ddMMyyyy");
            ValorTotalPgto = ObterValorTotalArquivo(totalLinhaArquivo, valorPgto);
            ValorTaxa = ObterValorTaxa(valorTaxa);
            ComplementarNomeFornecedor();
            ComplementarNumNota();
            ComplementarValorPgto();
            ComplementarValorTotalPgto();

            Validar();
        }

        private string ObterDocumentoFornecedor(string documento)
        {
            var padding = System.Math.Abs(documento.Length - LengthDocumentoFornecedor);
            return string.Empty.PadRight(padding, '0') + documento;
        }

        private string ObterValorTaxa(decimal valorTaxa)
        {
            var vlTotal = valorTaxa.ToString(CultureInfo.InvariantCulture);
            var padding = System.Math.Abs(vlTotal.Length - LengthTotalValorTaxa);
            var taxaComPadding = string.Empty.PadRight(padding, '0') + valorTaxa;
            return taxaComPadding.Replace(",", "0");
        }

        private string ObterValorTotalArquivo(int totalLinhaArquivo, decimal valorPgto)
        {
            var linhasDescartes = TotalHeaderArquivo + TotalTrailerArquivo;
            var totalLinhas = totalLinhaArquivo - linhasDescartes;
            return ((totalLinhas / TotalSegmentoArquivo) * valorPgto).ToString(CultureInfo.InvariantCulture);
        }

        private void ComplementarNomeFornecedor()
        {
            if (string.IsNullOrEmpty(NomeFornecedor)) return;

            var padding = System.Math.Abs(NomeFornecedor.Length - LengthNomeFornecedor);
            //            NomeFornecedor = NomeFornecedor.PadRight(padding, ' ');
            NomeFornecedor = string.Empty.PadLeft(padding, ' ') + NomeFornecedor;
        }


        private void ComplementarNumNota()
        {
            if (string.IsNullOrEmpty(NumeroNota)) return;

            var padding = System.Math.Abs(NomeFornecedor.Length - LengthNumeroNota);
            NumeroNota = string.Empty.PadRight(padding, '0') + NumeroNota;
        }

        private void ComplementarValorPgto()
        {
            var vlTotal = ValorPgto.ToString(CultureInfo.InvariantCulture);
            var padding = System.Math.Abs(vlTotal.Length - LengthValorPgto);
            ValorPgto = string.Empty.PadRight(padding, '0') + ValorPgto;
        }

        private void ComplementarValorTotalPgto()
        {
            var vlTotal = ValorTotalPgto.ToString(CultureInfo.InvariantCulture);
            var padding = System.Math.Abs(vlTotal.Length - LengthTotalValorPgto);
            ValorTotalPgto = string.Empty.PadRight(padding, '0') + ValorTotalPgto;
        }

        private void Validar()
        {
            if (NomeFornecedor.Length != LengthNomeFornecedor)
            {
                IsValid = false;
                return;
            }

            if (NumeroNota.Length != LengthNumeroNota)
            {
                IsValid = false;
                return;
            }

            if (ValorPgto.ToString(CultureInfo.CurrentCulture).Length != LengthValorPgto)
            {
                IsValid = false;
                return;
            }

            if (CnpjValidador.Validar(CnpjFornecedor))
            {
                IsValid = false;
            }
        }
    }
}