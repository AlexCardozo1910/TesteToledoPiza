using System;



namespace EstruturaBoostratap.Data.Models
{
    public class DBModel
    {
        private static readonly string Servidor = "mssql.alexcardozo.com.br";
        private static readonly string BancoDados = "alexcardozo";
        private static readonly string Usuario = "alexcardozo";
        private static readonly string Senha = "C@rd0z01910";

        static public string strConn = $"server={Servidor}; User id={Usuario};database={BancoDados};password={Senha}";


        public static string ViewBagMessage(string msg)
        {
            try
            {
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = msg.Replace("'", " ").Replace(Environment.NewLine, " ");
                }
            }
            catch (Exception)
            {
            }

            return msg;
        }
    }
}
