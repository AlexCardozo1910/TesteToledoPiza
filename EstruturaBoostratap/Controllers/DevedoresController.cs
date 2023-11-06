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
        
        public ActionResult Create()
        {
            DevedoresModelView dados = new DevedoresModelView();

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, List<string> Telefone, List<string> Descricao)
        {
            DevedoresModelView dados = new DevedoresModelView
            {
                CPFDevedor = collection["CPFDevedor"],
                RGDevedor = collection["RGDevedor"],
                NomeDevedor = collection["NomeDevedor"],
                EmailDevedor = collection["EmailDevedor"],
                Cep = collection["Cep"],
                IBGE = collection["IBGE"],
                Endereco = collection["Endereco"],
                Numero = collection["Numero"],
                Bairro = collection["Bairro"],
                Cidade = collection["Cidade"],
                Estado = collection["Estado"],
                Complemento = collection["Complemento"]
            };

            try
            {
                if (dados.VerificaCPF(dados.CPFDevedor))
                {
                    dados.MensagemInfo = "CPF já cadastrado!";
                    return View(dados);
                }

                int Codigo = dados.Inserir();

                if (Telefone[0] != null && Descricao[0] != null)
                {
                    for(int i = 0; i < Telefone.Count; i++)
                    {
                        dados.InserirTelefone(Codigo, Telefone[i], Descricao[i]);
                    }
                }

                dados.InserirEndereco(Codigo);

                return RedirectToAction("Edit", new { id = Codigo, Mensagem = "Cadastro Realizado!", Tipo = 4 });
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }

        public ActionResult Edit(int id, string Mensagem, int Tipo)
        {
            DevedoresModelView dados = new DevedoresModelView
            {
                Mensagem = Mensagem,
                Tipo = Tipo
            };

            dados.GetInfo(id);
            dados.ListaTelefoneDevedores = dados.GetListaTelefones(id);
            dados.GetEndereco(id);

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, List<string> Telefone, List<string> Descricao)
        {
            DevedoresModelView dados = new DevedoresModelView
            {
                CPFDevedor = collection["CPFDevedor"],
                RGDevedor = collection["RGDevedor"],
                NomeDevedor = collection["NomeDevedor"],
                EmailDevedor = collection["EmailDevedor"],
                Cep = collection["Cep"],
                IBGE = collection["IBGE"],
                Endereco = collection["Endereco"],
                Numero = collection["Numero"],
                Bairro = collection["Bairro"],
                Cidade = collection["Cidade"],
                Estado = collection["Estado"],
                Complemento = collection["Complemento"]
            };

            try
            {
                dados.AlterarInfo(id);

                foreach(var item in dados.GetListaTelefones(id))
                {
                    dados.AlterarTelefones(item.TelefoneDevedorID, collection["Tele_" + item.TelefoneDevedorID], collection["Desc_" + item.TelefoneDevedorID]);
                }
                
                dados.AlterarEnderecos(id);

                if (Telefone[0] != null && Descricao[0] != null)
                {
                    for (int i = 0; i < Telefone.Count; i++)
                    {
                        dados.InserirTelefone(id, Telefone[i], Descricao[i]);
                    }
                }

                dados.MensagemOk = "Ação executada!";

                dados.GetInfo(id);
                dados.ListaTelefoneDevedores = dados.GetListaTelefones(id);
                dados.GetEndereco(id);

                return View(dados);
            }
            catch (Exception ex){
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

        public ActionResult AlterarTelefone(int id, string telefone, string descricao)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {

                dados.AlterarTelefones(id, telefone, descricao);

                return Json(true);
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
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

        public ActionResult DeleteTelefone(int id)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {

                dados.DeleteTelefone(id);

                return Json(true);
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }
    }
}
