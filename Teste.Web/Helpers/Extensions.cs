namespace Teste.Web.Helpers
{
    public static class Extensions
    {
        public static string FormatarDocumento(this string documento)
        {
            if (string.IsNullOrEmpty(documento))
                return "";

            documento = new string(documento.Where(char.IsDigit).ToArray());

            if (documento.Length == 11)
                return Convert.ToUInt64(documento).ToString(@"000\.000\.000\-00");
            else if (documento.Length == 14)
                return Convert.ToUInt64(documento).ToString(@"00\.000\.000\/0000\-00");
            else
                return documento;
        }
    }
}