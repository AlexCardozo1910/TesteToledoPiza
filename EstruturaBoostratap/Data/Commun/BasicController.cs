using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static EstruturaBoostratap.Data.Commun.UploadArquivo;
using static EstruturaBoostratap.Data.Commun.OptionServices;

namespace EstruturaBoostratap.Data.Commun
{
    public class BasicController : Controller
    {
		public const string SessionNomeUsuario = "_NomeUsuario";
		public const string SessionUsuarioID = "_UsuarioID";

        private readonly IWebHostEnvironment webHostEnvironment;
		
		public IHostEnvironment Enviroment { get; set; }

		/*Converte em criptografia*/
		public static string HashValue(string value)
		{
			UnicodeEncoding encoding = new UnicodeEncoding();
			byte[] hashBytes;
			using (HashAlgorithm hash = SHA1.Create())
				hashBytes = hash.ComputeHash(encoding.GetBytes(value));

			StringBuilder hashValue = new StringBuilder(hashBytes.Length * 2);
			foreach (byte b in hashBytes)
			{
				hashValue.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
			}

			return hashValue.ToString();
		}

		/*Limpa o R$*/
		public string LimpaReais(string valor)
		{
			if (!String.IsNullOrEmpty(valor))
				return "0.00";

			var limpa_real = valor.Replace("R$", "");
			var final = limpa_real.Replace(".", "");

			return final;
		}

		public string LimpaReaisDecimal(string valor)
		{

			if (!String.IsNullOrEmpty(valor))
				return "0.00";

			var limpa_real = valor.Replace("R$", "");
			var limpa_ponto = limpa_real.Replace(".", "");
			var final = limpa_ponto.Replace(",", ".");

			return final;
		}

		public string ConverteReais(decimal valor)
		{
			var convert = valor.ToString("N2");
			var final = "R$ " + convert;

			return final;
		}

		public string TodosMeses(int mes)
		{
			return mes switch
			{
				1 => "Janeiro",
				2 => "Fevereiro",
				3 => "Março",
				4 => "Abril",
				5 => "Maio",
				6 => "Junho",
				7 => "Julho",
				8 => "Agosto",
				9 => "Setembro",
				10 => "Outubro",
				11 => "Novembro",
				12 => "Dezembro",
				_ => throw new System.NotImplementedException(),
			};
		}

		public decimal LimpaPorcentagem(string valor)
		{
			decimal limpa_porcentagem = Convert.ToDecimal(valor.Replace("%", ""));

			return limpa_porcentagem;
		}

		public static string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
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

		public string GeraBarras(int id)
		{
			string barCode = id.ToString();

			using (Bitmap bitMap = new Bitmap(barCode.Length * 40, 80))
			{
				using (Graphics graphics = Graphics.FromImage(bitMap))
				{
					Font oFont = new Font("IDAutomationHC39M Free Version", 16);
					PointF point = new PointF(2f, 2f);
					SolidBrush blackBrush = new SolidBrush(Color.Black);
					SolidBrush whiteBrush = new SolidBrush(Color.White);
					graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
					graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
				}
				using (MemoryStream ms = new MemoryStream())
				{
					bitMap.Save(ms, ImageFormat.Png);
					return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public string UploadedFile(UploadsArquivos arquivos, string LocalArquivo)
		{
			string nomeUnicoArquivo = null;

			if (arquivos.Arquivo != null)
			{
				string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, LocalArquivo);
				nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + arquivos.Arquivo.FileName;
				string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
				using var fileStream = new FileStream(caminhoArquivo, FileMode.Create);
				arquivos.Arquivo.CopyTo(fileStream);
			}
			return nomeUnicoArquivo;
		}
	}
}
