using Dapper;
using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EstruturaBoostratap.ModelViews
{
    public class LoginModelView : OptionServices
    {
        #region *** COMPONENTES ***
        public int UsuarioID { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; }
        public string MensagemOk { get; set; }
        public string MensagemErro { get; set; }
        public bool Token { get; set; }
        public bool SenhaAlterada { get; set; }
        public int ProjetoID { get; set; }
        #endregion

        #region *** METODOS ***

        public void RealizaLogin(string Login, string Senha)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_usuario AS UsuarioID, ");
                sql.AppendLine("nome_completo AS NomeUsuario, ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Usuarios ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("email_usuario = '{0}'", Login);
                sql.AppendLine(" AND ");
                sql.AppendFormat("senha = '{0}'", Senha);

                var DadosUsuarios = conexao.Query<LoginModelView>(sql.ToString()).SingleOrDefault();

                if (DadosUsuarios != null)
                {
                    UsuarioID = DadosUsuarios.UsuarioID;
                    NomeUsuario = DadosUsuarios.NomeUsuario;
                    ProjetoID = DadosUsuarios.ProjetoID;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void RecuperaSenha(string Email)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_usuario AS UsuarioID, ");
                sql.AppendLine("nome_completo AS NomeUsuario, ");
                sql.AppendLine("email_usuario AS EmailUsuario, ");
                sql.AppendLine("senha AS SenhaUsuario, ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Usuarios ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("email_usuario = '{0}'", Email);

                var DadosUsuarios = conexao.Query<LoginModelView>(sql.ToString()).SingleOrDefault();

                LoginModelView dados = new LoginModelView();

                if (DadosUsuarios != null)
                {
                    UsuarioID = DadosUsuarios.UsuarioID;
                    EmailUsuario = DadosUsuarios.EmailUsuario;
                    NomeUsuario = DadosUsuarios.NomeUsuario;
                    SenhaUsuario = DadosUsuarios.SenhaUsuario;
                    ProjetoID = DadosUsuarios.ProjetoID;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ValidaToken(string Token, string ID)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_usuario AS UsuarioID ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Usuarios ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("senha = '{0}' ", Token);
                sql.AppendLine("AND ");
                sql.AppendFormat("id_usuario = {0}", ID);

                var DadosUsuarios = conexao.Query<LoginModelView>(sql.ToString()).SingleOrDefault();

                LoginModelView dados = new LoginModelView();

                if (DadosUsuarios != null)
                {
                    UsuarioID = DadosUsuarios.UsuarioID;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlterarSenha(string Senha, string UsuarioID)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("Usuarios ");
                sql.AppendLine("SET ");
                sql.AppendFormat("senha = '{0}' ", Senha);
                sql.AppendLine("WHERE ");
                sql.AppendFormat("id_usuario = {0}", UsuarioID);

                MySqlCommand comando = new MySqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();

                LoginModelView dados = new LoginModelView();
                SenhaAlterada = true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void EnviarEmail(int UsuarioID, string NomeUsuario, string EmailUsuario, string SenhaUsuario)
        {
            try
            {
                var smtpClient = new SmtpClient("email-smtp.us-east-1.amazonaws.com", 587)
                {
                    EnableSsl = true,
                    Timeout = 60 * 60,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("AKIA3HVOCJZSTZ6QS4KK", "BNGW1zVebITERMm/G3ISgIrA/Ym2G3Vb6+cd+sje0yv6")
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("naoresponda@naoresponda.com.br", "Não Responda (Não Responder)"),
                    Body = MensagemRecuperacao(UsuarioID, NomeUsuario, EmailUsuario, SenhaUsuario),
                    Subject = "Resetar Senha",
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };

                mail.To.Add(EmailUsuario);
                smtpClient.Send(mail);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string MensagemRecuperacao(int UsuarioID, string NomeUsuario, string EmailUsuario, string SenhaUsuario)
        {

            string Mensagem = "<table style='border-radius:0 0 4px 4px;' width='100%' border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#ffffff'><tbody><tr><td style='color:#4c4c4c;font-size:14px;line-height:22px;font-family:verdana, sans-serif;padding:40px;'>";
            Mensagem += "<strong> Olá " + NomeUsuario + ",</strong>";
            Mensagem += "<br><br>Você está recebendo este e-mail porque solicitou a recuperação de senha do seu acesso.<br><br>";
            Mensagem += "<strong> Usuário / Login:</strong> " + EmailUsuario;
            Mensagem += "<br><strong> Senha:</strong>" +
                "<a rel='nofollow noopener noreferrer' style='color:#47ac82;font-weight:bold;text-decoration:none;' target='_blank' href='https://toolbox.insv.com.br/Login/NovaSenha?Token=" + SenhaUsuario + "&ID=" + UsuarioID + "'> " +
                "Clique aqui e defina uma nova senha" +
                "</a>";
            Mensagem += "<br><br>Por motivos de segurança, o link é válido até as próximas 24 horas, tudo bem?<br><br> Se você não deseja redefinir sua senha ou não fez essa solicitação, sua conta está protegida e você pode ignorar esta mensagem com segurança.<br><br>Até mais!<hr style='border:0;min-height:1px;background:#e6e6e6;margin:28px 0;'> Essa é uma mensagem automática, não responda este email.";
            Mensagem += "</td></tr></tbody></table>";

            return Mensagem;
        }
        #endregion
    }
}
