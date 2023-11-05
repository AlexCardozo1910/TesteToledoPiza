using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Components
{
    public class PaginacaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            //var id = Convert.ToInt32(HttpContext.Session.GetInt32(SessionUsuarioID));

            return View();
        }
    }
}
