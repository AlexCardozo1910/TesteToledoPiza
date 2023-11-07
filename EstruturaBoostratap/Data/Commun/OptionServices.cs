using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Data.Commun
{
	public class OptionServices
	{
		public int QTDRegistros { get; set; }
		public int PageAtual { get; set; }
		public int RPP { get; set; } = 20;
		public int Tipo { get; set; }
		public string Mensagem { get; set; }
		public string MensagemInfo { get; set; }
		public string MensagemReport { get; set; }
		public string MensagemOk { get; set; }
		public string MensagemErro { get; set; }
		public bool DadosAlterados { get; set; }

		public class RetornosJson
        {
			public string DataAtual { get; set; }
			public string Descricao { get; set; }
        }

		public class NomeMeses
		{
			public int CodigoMeses { get; set; }
			public string MesesNome { get; set; }
		}

		public List<NomeMeses> GetListaMeses()
		{

			var MesesReferentes = new List<NomeMeses>()
			{
				new NomeMeses(){ CodigoMeses = 1, MesesNome = "Janeiro" },
				new NomeMeses(){ CodigoMeses = 2, MesesNome = "Fevereiro" },
				new NomeMeses(){ CodigoMeses = 3, MesesNome = "Março" },
				new NomeMeses(){ CodigoMeses = 4, MesesNome = "Abril" },
				new NomeMeses(){ CodigoMeses = 5, MesesNome = "Maio" },
				new NomeMeses(){ CodigoMeses = 6, MesesNome = "Junho" },
				new NomeMeses(){ CodigoMeses = 7, MesesNome = "Julho" },
				new NomeMeses(){ CodigoMeses = 8, MesesNome = "Agosto" },
				new NomeMeses(){ CodigoMeses = 9, MesesNome = "Setembro" },
				new NomeMeses(){ CodigoMeses = 10, MesesNome = "Outubro" },
				new NomeMeses(){ CodigoMeses = 11, MesesNome = "Novembro" },
				new NomeMeses(){ CodigoMeses = 12, MesesNome = "Dezembro" },
			};

			return MesesReferentes;
		}

		public class RegistroPagina
		{
			public int NumeroPagina { get; set; }
			public string NomePagina { get; set; }
		}

		public List<RegistroPagina> GetRegistroPaginas()
		{
			var PaginaReferente = new List<RegistroPagina>()
			{
				new RegistroPagina(){ NumeroPagina = 7, NomePagina = "7 / página" },
				new RegistroPagina(){ NumeroPagina = 14, NomePagina = "14 / página" },
				new RegistroPagina(){ NumeroPagina = 20, NomePagina = "20 / página" },
				new RegistroPagina(){ NumeroPagina = 30, NomePagina = "30 / página" },
				new RegistroPagina(){ NumeroPagina = 50, NomePagina = "50 / página" },
			};

			return PaginaReferente;
		}

		public class Generos
		{
			public string CodigoGenero { get; set; }
			public string NomeGenero { get; set; }
		}

		public List<Generos> GetListaGenero()
		{
			var dados = new List<Generos>()
			{
				new Generos(){ CodigoGenero = "M", NomeGenero = "Masculino" },
				new Generos(){ CodigoGenero = "F", NomeGenero = "Feminino" },
				new Generos(){ CodigoGenero = "O", NomeGenero = "Outros" },
			};

			return dados;
		}

		public class Bloqueio
		{
			public bool Valor { get; set; }
			public string Texto { get; set; }
		}

		public List<Bloqueio> GetBloqueios()
		{
			var dados = new List<Bloqueio>()
			{
				new Bloqueio(){ Valor = false, Texto = "Sim" },
				new Bloqueio(){ Valor = true, Texto = "Não" },
			};

			return dados;
		}

		public string LimpaReais(string valor)
		{
			if (String.IsNullOrEmpty(valor))
				return "0.00";

			var limpa_real = valor.Replace("R$", "");
			var final = limpa_real.Replace(".", "");

			return final;
		}

		public string LimpaReaisDecimal(string valor)
		{

			if (String.IsNullOrEmpty(valor))
				return "0.00";

			var limpa_real = valor.Replace("R$", "");
			var limpa_ponto = limpa_real.Replace(".", "");
			var final = limpa_ponto.Replace(",", ".");

			return final;
		}

		/**
		* Função para formatar a data de várias formas tanto para usuário quanto para o banco de dados
		* @Author Alex Cardozo
		* @param String tipo user['dd/mm/aaaa'], user2['dd/mm/aaaa hh:mm:ss'], bd['aaaa-mm-dd'], bd2['aaaa-mm-dd hh:mm:ss']
		* @param data
		* @return String
		*/
		public string FormatarData(string tipo, string data)
		{

			if (data == "" || data == null)
				return data;

			string data_formatada = "";

			if (tipo == "user")
			{
				string[] data_hora = data.Split(' ');
				string[] array_data = data_hora[0].Split('-');
				data_formatada = array_data[2] + "/" + array_data[1] + "/" + array_data[0];
			}
			else if (tipo == "user2")
			{
				string[] data_hora = data.Split(' ');
				string[] array_data = data_hora[0].Split('-');
				data_formatada = array_data[2] + "/" + array_data[1] + "/" + array_data[0] + " " + data_hora[1];
			}
			else if (tipo == "bd")
			{
				string[] data_hora = data.Split(' ');
				string[] array_data = data_hora[0].Split('/');
				data_formatada = array_data[2] + "-" + array_data[1] + "-" + array_data[0];
			}
			else if (tipo == "bd2")
			{
				string[] data_hora = data.Split(' ');
				string[] array_data = data_hora[0].Split('/');
				data_formatada = array_data[2] + "-" + array_data[1] + "-" + array_data[0] + " " + data_hora[1];
			}
			else if (tipo == "user3")
            {
				string[] data_hora = data.Split(' ');
				string[] array_data = data_hora[0].Split('/');
				data_formatada = array_data[0] + "/" + array_data[1] + "/" + array_data[2];
			}

			return data_formatada;
		}

	}
}
