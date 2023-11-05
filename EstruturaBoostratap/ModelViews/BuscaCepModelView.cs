using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.ModelViews
{
    public class BuscaCepModelView
    {
        #region *** COMPONENTES ***
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
        public string Ibge { get; set; }
        public string Gia { get; set; }
        public string DDD { get; set; }
        public string Siafi { get; set; }
        public bool Erro { get; set; }
        #endregion
    }
}
