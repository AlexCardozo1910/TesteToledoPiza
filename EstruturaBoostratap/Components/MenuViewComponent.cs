using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Components
{
    public class MenuViewComponent : ViewComponent
    {
        public const string SessionNomeUsuario = "_NomeUsuario";
        public const string SessionUsuarioID = "_UsuarioID";

        public IViewComponentResult Invoke()
        {
            //var id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUsuarioID));

            return View();
        }
    }
}
