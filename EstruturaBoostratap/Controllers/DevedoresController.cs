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
        public ActionResult Index(int page, string Mensagem, int Tipo)
        {

            DevedoresModelView dados = new DevedoresModelView
            {
                PageAtual = page == 0 ? 1 : page,
                Mensagem = Mensagem,
                Tipo = Tipo
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
                RGDevedor = collection["RGDevedor"]
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


        public ActionResult TelefonesDevedor(int id)
        {
            DevedoresModelView dados = new DevedoresModelView();

            dados.ListaTelefoneDevedores = dados.GetListaTelefones(id);

            return View(dados);
        }

        public ActionResult Delete(int id)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {

                dados.DeleteDevedor(id);

                return RedirectToAction("index", new { Mensagem = "Ação Excecutada!", Tipo = 4 });
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }
    }
}
