using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EstruturaBoostratap.Controllers
{
    public class LoginController : BasicController
    {

        // GET: LoginController
        public ActionResult Index()
        {
            LoginModelView objeto = new LoginModelView();

            return View(objeto);
        }

        // POST: Logar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
        {
            try
            {
                var Login = collection["email-login"];
                var Senha = HashValue(collection["login-senha"]);

                LoginModelView objeto = new LoginModelView();

                objeto.RealizaLogin(Login, Senha);

                if (objeto.UsuarioID > 0)
                {
                    HttpContext.Session.SetString(SessionNomeUsuario, objeto.NomeUsuario);
                    HttpContext.Session.SetInt32(SessionUsuarioID, objeto.UsuarioID);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    objeto.EmailUsuario = collection["email-login"];
                    objeto.SenhaUsuario = collection["login-senha"];
                    objeto.MensagemErro = "Dados Informados São Inválidos!";
                    return View(objeto);
                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return RedirectToAction("Index", "Login");
            }
        }

        // GET: Usuario/Create
        public ActionResult Sair()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Login");
        }


        // GET: LoginController/RedefinicaoSenha
        public ActionResult RedefinicaoSenha()
        {
            LoginModelView objeto = new LoginModelView();

            return View(objeto);
        }

        // POST: LoginController/RedefinicaoSenha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RedefinicaoSenha(IFormCollection collection)
        {
            LoginModelView objeto = new LoginModelView();

            try
            {
                var Login = collection["email-login"];

                objeto.RecuperaSenha(Login);

                if (objeto.UsuarioID > 0)
                {
                    objeto.EmailUsuario = objeto.EmailUsuario;

                    objeto.EnviarEmail(objeto.UsuarioID, objeto.NomeUsuario, objeto.EmailUsuario, objeto.SenhaUsuario);
                    //objeto.SendMail("");

                    objeto.MensagemOk = "Um e-mail foi enviado para redefinição de senha!";

                    return View(objeto);
                }
                else
                {
                    objeto.EmailUsuario = collection["email-login"];
                    objeto.MensagemErro = "E-mail não cadastrado!";
                    return View(objeto);
                }

            }
            catch (Exception ex)
            {
                objeto.MensagemErro = "Falha no envio de e-mail!";
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return View(objeto);
            }
        }

        // GET: LoginController/RedefinicaoSenha
        public ActionResult NovaSenha(string Token, string ID)
        {
            LoginModelView objeto = new LoginModelView();
            objeto.ValidaToken(Token, ID);

            if (objeto.UsuarioID > 0)
            {
                objeto.Token = true;
                return View(objeto);
            }
            else
            {
                objeto.Token = false;
                objeto.MensagemErro = "Token informado é inválido!";
                return View(objeto);
            }
        }

        // POST: LoginController/NovaSenha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NovaSenha(IFormCollection collection)
        {
            try
            {
                LoginModelView objeto = new LoginModelView
                {
                    Token = true
                };

                if (collection["login-senha"] != collection["login-senha-confirmar"])
                {
                    objeto.UsuarioID = Convert.ToInt32(collection["usuarioID"]);
                    objeto.MensagemErro = "As senhas informadas não conferem!";
                    return View(objeto);
                }

                objeto.AlterarSenha(HashValue(collection["login-senha"]), collection["UsuarioID"]);

                if (objeto.SenhaAlterada == true)
                {
                    objeto.MensagemOk = "Senha alterada com sucesso!";
                }
                else
                {
                    objeto.MensagemErro = "Não foi possível realizar a ação!";
                }

                return View(objeto);
            }
            catch (Exception ex)
            {
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return RedirectToAction("NovaSenha", "Login");
            }
        }
    }
}
