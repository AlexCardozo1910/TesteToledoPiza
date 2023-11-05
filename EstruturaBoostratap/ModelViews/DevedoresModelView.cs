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
        public string NomeDevedor { get; set; }
        public string EmailDevedor { get; set; }
        public string CPFDevedor { get; set; }
        public string Contrato { get; set; }
        public DateTime DataPagamento { get; set; }
        public string ValorPrincipal { get; set; }
        public string ValorAtualizado { get; set; }

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
                sql.AppendLine("DevedorID, ");
                sql.AppendLine("NomeDevedor, ");
                sql.AppendLine("EmailDevedor, ");
                sql.AppendLine("CPFDevedor, ");
                sql.AppendLine("Contrato, ");
                sql.AppendLine("DataPagamento, ");
                sql.AppendLine("FORMAT(ValorPrincipal, 'C', 'pt-br' ) AS ValorPrincipal, ");
                sql.AppendLine("FORMAT(ValorPrincipal, 'C', 'pt-br' ) AS ValorAtualizado, ");
                sql.AppendLine("ROW_NUMBER() OVER (ORDER BY DevedorID) AS Linha ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendLine(" 1 = 1 ");
                
                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(DevedorID.ToString()))
                    sql.AppendFormat(" AND DevedorID = '{0}'", DevedorID);
                if (!String.IsNullOrEmpty(NomeDevedor))
                    sql.AppendFormat(" AND UPPER(NomeDevedor) LIKE UPPER('%{0}%')", NomeDevedor);
                if (!String.IsNullOrEmpty(EmailDevedor))
                    sql.AppendFormat(" AND UPPER(EmailDevedor) LIKE UPPER('%{0}%')", EmailDevedor);
                if (!String.IsNullOrEmpty(CPFDevedor))
                    sql.AppendFormat(" AND UPPER(CPFDevedor) LIKE UPPER('%{0}%')", CPFDevedor);
                if (!String.IsNullOrEmpty(Contrato))
                    sql.AppendFormat(" AND Contrato = '{0}'", Contrato);
                if (!String.IsNullOrEmpty(DataPagamento.ToString()))
                    sql.AppendFormat(" AND DataPagamento = '{0}'", FormatarData("db", DataPagamento.ToString()));
                if (!String.IsNullOrEmpty(ValorPrincipal))
                    sql.AppendFormat(" AND ValorPrincipal = '{0}'", LimpaReaisDecimal(ValorPrincipal));
                if (!String.IsNullOrEmpty(ValorAtualizado))
                    sql.AppendFormat(" AND ValorAtualizado = '{0}'", LimpaReaisDecimal(ValorAtualizado));
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
                        DevedorID = item.DevedorID,
                        NomeDevedor = item.NomeDevedor,
                        EmailDevedor = item.EmailDevedor,
                        CPFDevedor = item.CPFDevedor,
                        Contrato = item.Contrato,
                        DataPagamento = item.DataPagamento,
                        ValorPrincipal = item.ValorPrincipal,
                        ValorAtualizado = item.ValorAtualizado
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
                sql.AppendLine("COUNT(DevedorID) AS QTDRegistros");
                sql.AppendLine("FROM ");
                sql.AppendLine("Devedores");
                sql.AppendLine("WHERE ");
                sql.AppendLine(" 1 = 1 ");

                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(DevedorID.ToString()))
                    sql.AppendFormat(" AND DevedorID = '{0}'", DevedorID);
                if (!String.IsNullOrEmpty(NomeDevedor))
                    sql.AppendFormat(" AND UPPER(NomeDevedor) LIKE UPPER('%{0}%')", NomeDevedor);
                if (!String.IsNullOrEmpty(EmailDevedor))
                    sql.AppendFormat(" AND UPPER(EmailDevedor) LIKE UPPER('%{0}%')", EmailDevedor);
                if (!String.IsNullOrEmpty(CPFDevedor))
                    sql.AppendFormat(" AND UPPER(CPFDevedor) LIKE UPPER('%{0}%')", CPFDevedor);
                if (!String.IsNullOrEmpty(Contrato))
                    sql.AppendFormat(" AND Contrato = '{0}'", Contrato);
                if (!String.IsNullOrEmpty(DataPagamento.ToString()))
                    sql.AppendFormat(" AND DataPagamento = '{0}'", FormatarData("db", DataPagamento.ToString()));
                if (!String.IsNullOrEmpty(ValorPrincipal))
                    sql.AppendFormat(" AND ValorPrincipal = '{0}'", LimpaReaisDecimal(ValorPrincipal));
                if (!String.IsNullOrEmpty(ValorAtualizado))
                    sql.AppendFormat(" AND ValorAtualizado = '{0}'", LimpaReaisDecimal(ValorAtualizado));
                /*Fim dos filtros*/

                var Dados = conexao.Query<int>(sql.ToString());

                QTDRegistros = Dados.First();
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
        #endregion
    }

    public class TelefonesDevedores
    {
        #region *** COMPONENTES ***
        public int TelefoneDevedorID { get; set; }
        public int DevedorID { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        #endregion
    }
}
