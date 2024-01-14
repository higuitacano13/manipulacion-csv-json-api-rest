using ManipulacionCSVJson.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ManipulacionCSVJson.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<string[]> LeerArchivoCSV(string rutaArchivo)
        {
            List<string[]> datosCSV = new List<string[]>();

            // ----- Instrucciión using para liberar los recursos al finalizar de leer el archivo ---- //
            using (TextFieldParser lectoArchivoCSV = new TextFieldParser(rutaArchivo))
            {
                // ----- Especificar archivo separado por coma ---- //
                lectoArchivoCSV.TextFieldType = FieldType.Delimited;
                lectoArchivoCSV.SetDelimiters(",");

                while (!lectoArchivoCSV.EndOfData)
                {
                    string[] fila = lectoArchivoCSV.ReadFields(); // -> Leer Fila
                    datosCSV.Add(fila);
                }
            }

            return datosCSV;
        }

        public string ConvertirDatosAJSON(List<string[]> datosCSV)
        {
            List<Dictionary<string, string>> listaDiccionarios = new List<Dictionary<string, string>>();

            // ----- Creamos el encabezado con los titulos de el archivo CSV ---- //
            string[] titulosTabla = datosCSV[0];

            // ----- Rrecoremos a partir de la segunda fila ---- //
            for (int i = 1; i < datosCSV.Count; i++)
            {
                string[] fila = datosCSV[i];
                Dictionary<string, string> diccionario = new Dictionary<string, string>();

                // -----  Guardamos en el diccionario ---- //
                for (int j = 0; j < titulosTabla.Length && j < fila.Length; j++)
                {
                    diccionario[titulosTabla[j]] = fila[j];
                }

                listaDiccionarios.Add(diccionario);
            }

            // ----- Convertimos en formato Json ---- //
            string jsonDatos = JsonConvert.SerializeObject(listaDiccionarios, Formatting.Indented);

            return jsonDatos;
        }
    }
}