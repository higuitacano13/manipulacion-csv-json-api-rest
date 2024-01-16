using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArchivoCSVController : ControllerBase
    {
        [HttpGet("GetCSVFileContent")]
        public IActionResult GetCSVFileContentInJson(string csvPath)
        {
            try
            {
                if (System.IO.File.Exists(csvPath))
                {
                    List<string[]> csvContent = ArchivoHelper.LeerArchivoCSV(csvPath);

                    if (csvContent != null && csvContent.Count > 0)
                    {
                        string jsonData = ArchivoHelper.ConvertirDatosAJSON(csvContent);
                        return Content(jsonData, "application/json");
                    }
                    else
                    {
                        return NotFound("El archivo CSV está vacío o no contiene datos válidos.");
                    }
                }
                else
                {
                    return NotFound("El archivo CSV no fue encontrado en la ruta especificada.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
