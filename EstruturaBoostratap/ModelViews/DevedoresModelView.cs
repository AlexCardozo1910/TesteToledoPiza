using Dapper;
using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstruturaBoostratap.ModelViews
{
    public class DevedoresModelView : OptionServices
    {
        #region *** COMPONENTES ***
        public int DevedorID { get; set; }
        public int ContratoID { get; set; }
        public string DevedoresID { get; set; }
        public string NomeDevedor { get; set; }
        public string EmailDevedor { get; set; }
        public string CPFDevedor { get; set; }
        public string RGDevedor { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string IBGE { get; set; }
        public string Complemento { get; set; }
        public string ValorTotalParcelas { get; set; }

        public List<DevedoresModelView> ListaDevedores { get; set; }
        public List<DevedoresEndereco> ListaEnderecoDevedores { get; set; }
        public List<TelefonesDevedores> ListaTelefoneDevedores { get; set; }
        public List<Contratos> ListaContratos { get; set; }
        public List<AcordosContratos> ListaParcelas { get; set; }
        public List<Parcelas> ListaParcelasVencidas { get; set; }
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
                if (!String.IsNullOrEmpty(RGDevedor))
                    sql.AppendFormat(" AND UPPER(RGDevedor) LIKE UPPER('%{0}%')", RGDevedor);
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
                if (!String.IsNullOrEmpty(RGDevedor))
                    sql.AppendFormat(" AND UPPER(RGDevedor) LIKE UPPER('%{0}%')", RGDevedor);
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

        public int GravaContrato(int id, int NumeroContrato)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("Contratos");
                sql.AppendLine("( ");
                sql.AppendLine("IDDevedor, ");
                sql.AppendLine("NumeroContrato, ");
                sql.AppendLine("DataInclusao ");
                sql.AppendLine(") ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}', ", id);
                sql.AppendFormat("'{0}', ", NumeroContrato);
                sql.AppendLine("CAST(GETDATE() AS DATE) ");
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

        public void GravarParcelas(int ContratoID, int NumeroParcela, decimal ValorParcela, DateTime DataVencimento)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("Parcelas");
                sql.AppendLine("( ");
                sql.AppendLine("IDContrato, ");
                sql.AppendLine("NumeroParcela, ");
                sql.AppendLine("ValorParcela, ");
                sql.AppendLine("DataVencimento, ");
                sql.AppendLine("Status ");
                sql.AppendLine(") ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}', ", ContratoID);
                sql.AppendFormat("'{0}', ", NumeroParcela);
                sql.AppendFormat("'{0}', ", LimpaReaisDecimal(ValorParcela.ToString()));
                sql.AppendFormat("'{0}', ", DataVencimento.ToString("yyyy-MM-dd"));
                sql.AppendFormat("'{0}' ", 'A');
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

        public List<Contratos> GetListaContrato(int id)
        {

            List<Contratos> ListasContratos = new List<Contratos>();

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("IDContrato, ");
                sql.AppendLine("IDDevedor, ");
                sql.AppendLine("NumeroContrato, ");
                sql.AppendLine("DataInclusao ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Contratos");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("IDDevedor = {0} ", id);

                var Dados = conexao.Query<Contratos>(sql.ToString()).ToList();

                foreach (var item in Dados)
                {
                    Contratos dados = new Contratos
                    {
                        IDContrato = item.IDContrato,
                        IDDevedor = item.IDDevedor,
                        NumeroContrato = item.NumeroContrato,
                        DataInclusao = item.DataInclusao,
                        ListaParcelas = GetListaParcelas(item.IDContrato),
                        ParcelasVencidas = GetPossuiParcelas(item.IDContrato)
                    };

                    ListasContratos.Add(dados);
                }

                return ListasContratos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Parcelas> GetListaParcelasVencidas(int id)
        {
            List<Parcelas> ListasParcelas = new List<Parcelas>();

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("T2.NumeroContrato, ");
                sql.AppendLine("T1.IDParcela, ");
                sql.AppendLine("T1.NumeroParcela, ");
                sql.AppendLine("FORMAT(T1.ValorParcela, 'C', 'pt-BR') AS ValorParcela, ");
                sql.AppendLine("T1.DataVencimento, ");
                sql.AppendLine("DATEDIFF(DAY, T1.DataVencimento, GETDATE()) as DiasAbertos  ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Parcelas T1");
                sql.AppendLine("INNER JOIN ");
                sql.AppendLine("Contratos T2 ON T1.IDContrato = T2.IDContrato");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("T1.IDContrato = {0} ", id);
                sql.AppendLine("AND ");
                sql.AppendLine("T1.DataVencimento < GETDATE() ");
                sql.AppendLine("AND ");
                sql.AppendLine("T1.Status = 'A' ");

                var Dados = conexao.Query<Parcelas>(sql.ToString()).ToList();

                GetTotalParcelasVencidas(id);

                foreach (var item in Dados)
                {
                    Parcelas dados = new Parcelas
                    {
                        NumeroContrato = item.NumeroContrato,
                        IDParcela = item.IDParcela,
                        NumeroParcela = item.NumeroParcela,
                        ValorParcela = item.ValorParcela,
                        DataVencimento = item.DataVencimento.Date,
                        DiasAbertos = item.DiasAbertos
                    };

                    ListasParcelas.Add(dados);
                }

                return ListasParcelas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void GetTotalParcelasVencidas(int id)
        {

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("FORMAT(SUM(ValorParcela), 'C', 'pt-BR') AS ValorTotalParcelas ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Parcelas");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("IDContrato = {0} ", id);
                sql.AppendLine("AND ");
                sql.AppendLine("DataVencimento < GETDATE() ");
                sql.AppendLine("AND ");
                sql.AppendLine("Status = 'A' ");

                var Dados = conexao.Query<string>(sql.ToString());

                ValorTotalParcelas = Dados.First();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Parcelas> GetListaParcelas(int ContratoID)
        {

            List<Parcelas> ListasParcelas = new List<Parcelas>();

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("IDParcela, ");
                sql.AppendLine("NumeroParcela, ");
                sql.AppendLine("FORMAT(ValorParcela, 'C', 'pt-BR') AS ValorParcela, ");
                sql.AppendLine("CAST(DataVencimento AS DATE) AS DataVencimento, ");
                sql.AppendLine("CAST(DataPagamento AS DATE) AS DataPagamento, ");
                sql.AppendLine("CASE WHEN Status = 'A' THEN 'Parcela Aberta' ELSE 'Parcela Paga' END AS Status ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Parcelas");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("IDContrato = {0} ", ContratoID);

                var Dados = conexao.Query<Parcelas>(sql.ToString()).ToList();

                foreach (var item in Dados)
                {
                    Parcelas dados = new Parcelas
                    {
                        IDParcela = item.IDParcela,
                        NumeroParcela = item.NumeroParcela,
                        ValorParcela = item.ValorParcela,
                        DataVencimento = item.DataVencimento.Date,
                        DataPagamento = item.DataPagamento.Date,
                        Status = item.Status
                    };

                    ListasParcelas.Add(dados);
                }

                return ListasParcelas;
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

        public bool VerificaCPF(string CPF, int id)
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
                if(id > 0)
                {
                    sql.AppendLine("AND ");
                    sql.AppendFormat("DevedoresID != '{0}' ", id);
                }

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
                DevedorID = Convert.ToInt32(Dados.DevedoresID);
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
                Bairro = Dados.Bairro;
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

        public void RealizaPagamento(int id)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("Parcelas ");
                sql.AppendLine("SET ");
                sql.AppendLine("DataPagamento = CAST(GETDATE() AS DATE), ");
                sql.AppendLine("Status = 'P' ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("IDParcela = '{0}' ", id);

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
                sql.AppendLine("Devedores ");
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

        public bool GetPossuiParcelas(int id)
        {

            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(IDParcela) AS QTDParcelas");
                sql.AppendLine("FROM ");
                sql.AppendLine("Parcelas");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("IDContrato = {0} ", id);
                sql.AppendLine("AND ");
                sql.AppendLine("DataVencimento < GETDATE(); ");

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

        public List<AcordosContratos> GetAcordos(int id, int ContratoID, DateTime DataPagamento)
        {
            SqlConnection conexao = new SqlConnection(DBModel.strConn);

            ListaParcelas = new List<AcordosContratos>();

            try
            {
                var parametros = new DynamicParameters();
                parametros.Add("@DevedorID", id);
                parametros.Add("@ContratoID", ContratoID);
                parametros.Add("@DataPagamento", DataPagamento.ToString("yyyy-MM-dd"));

                var Dados = conexao.Query<AcordosContratos>("CalcularValoresAtualizados", parametros, commandType: CommandType.StoredProcedure).ToList();

                foreach (var item in Dados)
                {
                    AcordosContratos dados = new AcordosContratos
                    {
                        CPF = item.CPF,
                        Contrato = item.Contrato,
                        DataPagamento = item.DataPagamento,
                        ValorPrincipal = item.ValorPrincipal,
                        ValorAtualizado = item.ValorAtualizado,
                        PG1Parcela = item.PG1Parcela,
                        PG2Parcela = item.PG2Parcela,
                        Quitacao = item.Quitacao
                    };

                    ListaParcelas.Add(dados);
                }

                return ListaParcelas;
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

    public class Contratos
    {
        #region *** COMPONENTES ***
        public int IDContrato { get; set; }
        public int IDDevedor { get; set; }
        public string NumeroContrato { get; set; }
        public DateTime DataInclusao { get; set; }
        public bool ParcelasVencidas { get; set; }

        public List<Parcelas> ListaParcelas { get; set; }
        #endregion
    }

    public class Parcelas
    {
        #region *** COMPONENTES ***
        public string NumeroContrato { get; set; }
        public int IDParcela { get; set; }
        public int IDContrato { get; set; }
        public int NumeroParcela { get; set; }
        public string ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataPagamento { get; set; }
        public string Status { get; set; }
        public int DiasAbertos { get; set; }
        #endregion
    }

    public class AcordosContratos
    {
        #region *** COMPONENTES ***
        public string CPF { get; set; }
        public string Contrato { get; set; }
        public DateTime DataPagamento { get; set; }
        public string ValorPrincipal { get; set; }
        public string ValorAtualizado { get; set; }
        public string PG1Parcela { get; set; }
        public string PG2Parcela { get; set; }
        public string Quitacao { get; set; }
        #endregion
    }

}
