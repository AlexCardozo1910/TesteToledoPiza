using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Data.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public DateTime DataCadastro { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string RgIe { get; set; }
        public bool Isento { get; set; }
        public string Genero { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public bool Status { get; set; }
    }
}
