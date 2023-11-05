using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Controllers
{
    public class BuscaCepController : BasicController
    {
        public ActionResult BuscaEnderecos(string cep)
        {
            var cepClear = cep.Replace("-", "");

            var url = @"https://viacep.com.br/ws/" + cepClear + "/json/";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<BuscaCepModelView>(response.Content);

            return Json(result);
        }
    }
}
