using System;



namespace EstruturaBoostratap.Data.Models
{
    public class DBModel
    {
        private static readonly string Servidor = "mysql31-farm15.uni5.net";
        private static readonly string BancoDados = "alexcardozo01";
        private static readonly string Usuario = "alexcardozo01";
        private static readonly string Senha = "Abacate123";

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
