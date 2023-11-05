using Dapper;
using EstruturaBoostratap.Data.Models;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Components
{
    public class UserViewComponent : ViewComponent
    {
        public const string SessionNomeUsuario = "_NomeUsuario";
        public const string SessionUsuarioID = "_UsuarioID";

        public IViewComponentResult Invoke()
        {
            var id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUsuarioID));
            var items = GetItemsAsync(id);
            return View(items);
        }

        private object GetItemsAsync(int ID)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_usuario AS UsuarioID, ");
                sql.AppendLine("nome_completo AS NomeUsuario ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Usuarios ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("id_usuario = {0}", ID);

                var DadosUsuarios = conexao.Query<LoginModelView>(sql.ToString()).SingleOrDefault();

                LoginModelView dados = new LoginModelView();

                if (DadosUsuarios != null)
                {
                    dados.UsuarioID = DadosUsuarios.UsuarioID;
                    dados.NomeUsuario = DadosUsuarios.NomeUsuario;
                }

                return dados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
