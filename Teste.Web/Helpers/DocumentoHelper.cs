using System.Text.RegularExpressions;

namespace Teste.Web.Helpers
{
    public static class DocumentoHelper
    {
        public static bool DocumentoEhValido(string documento)
        {
            documento = SomenteNumeros(documento);

            if (documento.Length == 11)
                return ValidarCPF(documento);

            if (documento.Length == 14)
                return ValidarCNPJ(documento);

            return false;
        }

        public static string FormatarDocumento(string documento)
        {
            documento = SomenteNumeros(documento);

            if (documento.Length == 11)
                return Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00");

            if (documento.Length == 14)
                return Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");

            return documento; // Retorna como veio, se não for nem CPF nem CNPJ
        }

        public static string SomenteNumeros(string texto)
        {
            return Regex.Replace(texto, "[^0-9]", "");
        }

        private static bool ValidarCPF(string cpf)
        {
            // Regras básicas de validação de CPF (pode melhorar se quiser)
            if (cpf.Length != 11 || new string(cpf[0], cpf.Length) == cpf)
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        private static bool ValidarCNPJ(string cnpj)
        {
            if (cnpj.Length != 14 || new string(cnpj[0], cnpj.Length) == cnpj)
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            tempCnpj += digito;

            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}