using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Controllers
{
    public class DevedoresController : BasicController
    {
        public ActionResult Index(int page)
        {

            DevedoresModelView dados = new DevedoresModelView
            {
                PageAtual = page == 0 ? 1 : page
            };

            dados.ListaDevedores = dados.ListDevedores();

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int page, IFormCollection collection)
        {

            DevedoresModelView dados = new DevedoresModelView
            {
                PageAtual = page == 0 ? 1 : page,
                DevedorID = Convert.ToInt32(collection["DevedorID"]),
                NomeDevedor = collection["NomeDevedor"],
                EmailDevedor = collection["EmailDevedor"],
                CPFDevedor =  collection["CPFDevedor"],
                Contrato = collection["Contrato"],
                DataPagamento = Convert.ToDateTime(collection["DataPagamento"]),
                ValorPrincipal = collection["ValorPrincipal"],
                ValorAtualizado = collection["ValorAtualizado"]
            };

            try
            {
                dados.ListaDevedores = dados.ListDevedores();

                return View(dados);
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }
    }
}
