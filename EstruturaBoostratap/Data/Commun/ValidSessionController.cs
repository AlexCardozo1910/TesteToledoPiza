using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace EstruturaBoostratap.Data.Commun
{
    internal class ValidSessionController : ActionFilterAttribute
    {
        public const string SessionNomeUsuario = "_NomeUsuario";
        public const string SessionUsuarioID = "_UsuarioID";
        public const string SessionProjetoID = "_ProjetoID";

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32(SessionUsuarioID) == null)
            {
                var routes = new RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" }
                    };
                context.Result = new RedirectToRouteResult(routes);
            }

            base.OnResultExecuting(context);
        }

    }
}