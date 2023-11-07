using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EstruturaBoostratap.Data.Commun.OptionServices;

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
                DevedoresID = collection["DevedoresID"],
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
                if (dados.VerificaCPF(dados.CPFDevedor, 0))
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
            dados.GetEndereco(id);
            dados.ListaTelefoneDevedores = dados.GetListaTelefones(id);
            dados.ListaContratos = dados.GetListaContrato(id);

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

                if (dados.VerificaCPF(dados.CPFDevedor, id))
                {
                    dados.MensagemInfo = "CPF cadastrado em outro devedor!";
                    return View(dados);
                }

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
                dados.GetEndereco(id);
                dados.ListaTelefoneDevedores = dados.GetListaTelefones(id);
                dados.ListaContratos = dados.GetListaContrato(id);

                return View(dados);
            }
            catch (Exception ex){
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }

        public ActionResult PagamentoParcela(int id)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {
                dados.RealizaPagamento(id);

                RetornosJson json = new RetornosJson
                {
                    DataAtual = DateTime.Now.ToString("dd/MM/yyyy"),
                    Descricao = "Parcela Paga"
                };

                return Json(json);
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }
        }

        public ActionResult ContratosDevedores(int id)
        {
            DevedoresModelView dados = new DevedoresModelView();

            dados.ListaContratos = dados.GetListaContrato(id);

            return View(dados);
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

        public ActionResult CriarContrato(int id)
        {

            DevedoresModelView dados = new DevedoresModelView();

            try
            {

                Random random = new Random();

                int ContratoID = dados.GravaContrato(id, random.Next(785426, 987635));

                DateTime hoje = DateTime.Now;
                for (int i = 1; i <= 3; i++)
                {
                    decimal ValorParcela = 457 + (decimal)random.NextDouble() * 2;
                    DateTime DataParcela = hoje.AddMonths(i);
                    dados.GravarParcelas(ContratoID, i, ValorParcela, DataParcela);
                }

                return RedirectToAction("Edit", new { id = id, Mensagem = "Contrato gerado!", Tipo = 4 });
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


        public ActionResult RealizarAcordo(int contratoid)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {
                return View(dados);
            }
            catch (Exception ex)
            {
                dados.MensagemReport = ex.Message;
                return View(dados);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RealizarAcordo(int id, int contratoid, DateTime datapagamento)
        {
            DevedoresModelView dados = new DevedoresModelView();

            try
            {
                dados.GetAcordos(id, contratoid, datapagamento);

                return View(dados);
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
