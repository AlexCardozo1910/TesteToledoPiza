using Dapper;
using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstruturaBoostratap.ModelViews
{
    public class DevedoresModelView : OptionServices
    {
        #region *** COMPONENTES ***
        public int DevedorID { get; set; }
        public string DevedoresID { get; set; }
        public string NomeDevedor { get; set; }
        public string EmailDevedor { get; set; }
        public string CPFDevedor { get; set; }
        public string RGDevedor { get; set; }
        public string Contrato { get; set; }
        public string DataPagamento { get; set; }
        public string ValorPrincipal { get; set; }
        public string ValorAtualizado { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string IBGE { get; set; }
        public string Complemento { get; set; }


        public List<DevedoresModelView> ListaDevedores { get; set; }
        public List<DevedoresEndereco> ListaEnderecoDevedores { get; set; }
        public List<TelefonesDevedores> ListaTelefoneDevedores { get; set; }
        #endregion

        #region *** METODOS ***
        public List<DevedoresModelView> ListDevedores()
        {

            List<DevedoresModelView> ListasDevedores = new List<DevedoresModelView>();

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("DECLARE @LIMIT INT = {0}, @OFFSET INT = {1}; ", RPP, (PageAtual - 1) * RPP);
                sql.AppendLine("WITH resultado AS ");
                sql.AppendLine("( ");
                sql.AppendLine("SELECT ");
                sql.AppendLine("DevedoresID, ");
                sql.AppendLine("NomeDevedor, ");
                sql.AppendLine("EmailDevedor, ");
                sql.AppendLine("CPFDevedor, ");
                sql.AppendLine("RGDevedor, ");
                sql.AppendLine("ROW_NUMBER() OVER (ORDER BY DevedoresID) AS Linha ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendLine(" 1 = 1 ");
                
                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(DevedoresID))
                    sql.AppendFormat(" AND DevedoresID = '{0}'", DevedoresID);
                if (!String.IsNullOrEmpty(NomeDevedor))
                    sql.AppendFormat(" AND UPPER(NomeDevedor) LIKE UPPER('%{0}%')", NomeDevedor);
                if (!String.IsNullOrEmpty(EmailDevedor))
                    sql.AppendFormat(" AND UPPER(EmailDevedor) LIKE UPPER('%{0}%')", EmailDevedor);
                if (!String.IsNullOrEmpty(CPFDevedor))
                    sql.AppendFormat(" AND UPPER(CPFDevedor) LIKE UPPER('%{0}%')", CPFDevedor);
                /*Fim dos filtros*/

                sql.AppendLine(")");
                sql.AppendLine("SELECT * FROM Resultado WHERE Linha >= @OFFSET AND Linha < @OFFSET + @LIMIT ");
                sql.AppendLine("ORDER BY NomeDevedor ASC");

                var Dados = conexao.Query<DevedoresModelView>(sql.ToString()).ToList();

                TotalRegistro();

                foreach (var item in Dados)
                {
                    DevedoresModelView dados = new DevedoresModelView
                    {
                        DevedorID = Convert.ToInt32(item.DevedoresID),
                        DevedoresID = item.DevedoresID,
                        NomeDevedor = item.NomeDevedor,
                        EmailDevedor = item.EmailDevedor,
                        CPFDevedor = item.CPFDevedor,
                        RGDevedor = item.RGDevedor
                    };

                    ListasDevedores.Add(dados);
                }

                return ListasDevedores;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void TotalRegistro()
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(DevedoresID) AS QTDRegistros");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendLine(" 1 = 1 ");

                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(DevedoresID))
                    sql.AppendFormat(" AND DevedoresID = '{0}'", DevedoresID);
                if (!String.IsNullOrEmpty(NomeDevedor))
                    sql.AppendFormat(" AND UPPER(NomeDevedor) LIKE UPPER('%{0}%')", NomeDevedor);
                if (!String.IsNullOrEmpty(EmailDevedor))
                    sql.AppendFormat(" AND UPPER(EmailDevedor) LIKE UPPER('%{0}%')", EmailDevedor);
                if (!String.IsNullOrEmpty(CPFDevedor))
                    sql.AppendFormat(" AND UPPER(CPFDevedor) LIKE UPPER('%{0}%')", CPFDevedor);
                /*Fim dos filtros*/

                var Dados = conexao.Query<int>(sql.ToString());

                QTDRegistros = Dados.First();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public int Inserir()
        {

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("Devedores");
                sql.AppendLine("( ");
                sql.AppendLine("NomeDevedor, ");
                sql.AppendLine("EmailDevedor, ");
                sql.AppendLine("CPFDevedor, ");
                sql.AppendLine("RGDevedor ");
                sql.AppendLine(") ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}', ", NomeDevedor);
                sql.AppendFormat("'{0}', ", EmailDevedor);
                sql.AppendFormat("'{0}', ", CPFDevedor);
                sql.AppendFormat("'{0}' ", RGDevedor);
                sql.AppendLine("); SELECT CAST(scope_identity() AS INT) ");

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                int Codigo = Convert.ToInt32(comando.ExecuteScalar());

                return Codigo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void InserirTelefone(int id, string Telefone, string Descricao)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("TelefonesDevedores");
                sql.AppendLine("( ");
                sql.AppendLine("DevedorID, ");
                sql.AppendLine("Telefone, ");
                sql.AppendLine("Descricao ");
                sql.AppendLine(") ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}', ", id);
                sql.AppendFormat("'{0}', ", Telefone);
                sql.AppendFormat("'{0}' ", Descricao);
                sql.AppendLine(");");

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void InserirEndereco(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("DevedoresEndereco");
                sql.AppendLine("( ");
                sql.AppendLine("DevedorID, ");
                sql.AppendLine("Cep, ");
                sql.AppendLine("Endereco, ");
                sql.AppendLine("Numero, ");
                sql.AppendLine("Bairro, ");
                sql.AppendLine("Cidade, ");
                sql.AppendLine("Estado, ");
                sql.AppendLine("IBGE, ");
                sql.AppendLine("Complemento ");
                sql.AppendLine(") ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}', ", id);
                sql.AppendFormat("'{0}', ", Cep);
                sql.AppendFormat("'{0}', ", Endereco);
                sql.AppendFormat("'{0}', ", Numero);
                sql.AppendFormat("'{0}', ", Bairro);
                sql.AppendFormat("'{0}', ", Cidade);
                sql.AppendFormat("'{0}', ", Estado);
                sql.AppendFormat("'{0}', ", IBGE);
                sql.AppendFormat("'{0}' ", Complemento);
                sql.AppendLine(");");

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool VerificaCPF(string CPF)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(DevedoresID) AS QTDRegistros");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("CPFDevedor = '{0}' ", CPF);

                var Dados = conexao.Query<int>(sql.ToString());

                if (Dados.First() > 0)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<TelefonesDevedores> GetListaTelefones(int id)
        {

            List<TelefonesDevedores> ListaTelefone = new List<TelefonesDevedores>();

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("TelefoneDevedorID, ");
                sql.AppendLine("Telefone, ");
                sql.AppendLine("Descricao ");
                sql.AppendLine("FROM ");
                sql.AppendLine("TelefonesDevedores");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedorID = {0} ", id);

                var Dados = conexao.Query<TelefonesDevedores>(sql.ToString()).ToList();

                foreach(var item in Dados)
                {
                    TelefonesDevedores dados = new TelefonesDevedores
                    {
                        TelefoneDevedorID = item.TelefoneDevedorID,
                        Telefone = item.Telefone,
                        Descricao = item.Descricao
                    };

                    ListaTelefone.Add(dados);
                }

                return ListaTelefone;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GetInfo(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("DevedoresID, ");
                sql.AppendLine("NomeDevedor, ");
                sql.AppendLine("EmailDevedor, ");
                sql.AppendLine("CPFDevedor, ");
                sql.AppendLine("RGDevedor ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedoresID = {0} ", id);

                var Dados = conexao.Query<DevedoresModelView>(sql.ToString()).SingleOrDefault();

                DevedoresID = Dados.DevedoresID;
                NomeDevedor = Dados.NomeDevedor;
                EmailDevedor = Dados.EmailDevedor;
                CPFDevedor = Dados.CPFDevedor;
                RGDevedor = Dados.RGDevedor;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GetEndereco(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("Cep, ");
                sql.AppendLine("Endereco, ");
                sql.AppendLine("Numero, ");
                sql.AppendLine("Bairro, ");
                sql.AppendLine("Cidade, ");
                sql.AppendLine("Estado, ");
                sql.AppendLine("IBGE, ");
                sql.AppendLine("Complemento ");
                sql.AppendLine("FROM ");
                sql.AppendLine("DevedoresEndereco");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedorID = {0} ", id);

                var Dados = conexao.Query<DevedoresEndereco>(sql.ToString()).SingleOrDefault();

                Cep = Dados.Cep;
                Endereco = Dados.Endereco;
                Numero = Dados.Numero;
                Cidade = Dados.Cidade;
                Estado = Dados.Estado;
                IBGE = Dados.IBGE;
                Complemento = Dados.Complemento;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlterarInfo(int id)
        {

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("Devedores ");
                sql.AppendLine("SET ");
                sql.AppendFormat("NomeDevedor = '{0}', ", NomeDevedor);
                sql.AppendFormat("EmailDevedor = '{0}', ", EmailDevedor);
                sql.AppendFormat("CPFDevedor = '{0}', ", CPFDevedor);
                sql.AppendFormat("RGDevedor = '{0}' ", RGDevedor);
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedoresID = '{0}' ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlterarEnderecos(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("DevedoresEndereco ");
                sql.AppendLine("SET ");
                sql.AppendFormat("Cep = '{0}', ", Cep);
                sql.AppendFormat("Endereco = '{0}', ", Endereco);
                sql.AppendFormat("Numero = '{0}', ", Numero);
                sql.AppendFormat("Bairro = '{0}', ", Bairro);
                sql.AppendFormat("Cidade = '{0}', ", Cidade);
                sql.AppendFormat("Estado = '{0}', ", Estado);
                sql.AppendFormat("IBGE = '{0}', ", IBGE);
                sql.AppendFormat("Complemento = '{0}' ", Complemento);
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedorID = '{0}' ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlterarTelefones(int id, string Telefone, string Descricao)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("TelefonesDevedores ");
                sql.AppendLine("SET ");
                sql.AppendFormat("Telefone = '{0}', ", Telefone);
                sql.AppendFormat("Descricao = '{0}' ", Descricao);
                sql.AppendLine("WHERE ");
                sql.AppendFormat("TelefoneDevedorID = '{0}' ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteDevedor(int id)
        {
            DeleteTelefones(id);
            DeleteEnderecos(id);

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedor ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedoresID = {0} ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteTelefone(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE ");
                sql.AppendLine("FROM ");
                sql.AppendLine("TelefonesDevedores ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("TelefoneDevedorID = {0} ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteTelefones(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE ");
                sql.AppendLine("FROM ");
                sql.AppendLine("TelefonesDevedores ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedorID = {0} ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void DeleteEnderecos(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE ");
                sql.AppendLine("FROM ");
                sql.AppendLine("DevedoresEndereco ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("DevedorID = {0} ", id);

                SqlCommand comando = new SqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion
    }

    public class DevedoresEndereco
    {
        #region *** COMPONENTES ***
        public int EnderecoDevedorID { get; set; }
        public int DevedorID { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string IBGE { get; set; }
        public string Complemento { get; set; }
        #endregion
    }

    public class TelefonesDevedores
    {
        #region *** COMPONENTES ***
        public int TelefoneDevedorID { get; set; }
        public int DevedorID { get; set; }
        public string Telefone { get; set; }
        public string  Descricao { get; set; }
        #endregion
    }
}
