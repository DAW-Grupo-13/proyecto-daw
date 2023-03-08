using Microsoft.AspNetCore.Mvc;
using Ferreteria.API.SharedCode;
using Ferreteria.BL;
using System.Data;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ferreteria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {

        // POST api/<ClientesController>
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Clientes>> GetClienteEdad([FromBody] Clientes clientes)
        {
            var cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_bd_user"];
            XDocument xmlParam = DBXmlMethods.GetXml(clientes);
            DataSet sResultado = await DBXmlMethods.EjecutaBase("GetClientes", cadenaConexion, "CONSULTA_CLIENTES_EDAD", xmlParam.ToString());
            List<Clientes> listData = new List<Clientes>();

            DBXmlMethods.generateResponse(sResultado, listData);

            return Ok(listData);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Clientes>> GetClienteApellido([FromBody] Clientes clientes)
        {
            var cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_bd_user"];
            XDocument xmlParam = DBXmlMethods.GetXml(clientes);
            DataSet sResultado = await DBXmlMethods.EjecutaBase("GetClientes", cadenaConexion, "CONSULTA_CLIENTES_APELLIDO", xmlParam.ToString());
            List<Clientes> listData = new List<Clientes>();

            DBXmlMethods.generateResponse(sResultado, listData);

            return Ok(listData);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Clientes>> GetClienteCiudad([FromBody] Clientes clientes)
        {
            var cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_bd_user"];
            XDocument xmlParam = DBXmlMethods.GetXml(clientes);
            DataSet sResultado = await DBXmlMethods.EjecutaBase("GetClientes", cadenaConexion, "CONSULTA_CLIENTES_CIUDAD", xmlParam.ToString());
            List<Clientes> listData = new List<Clientes>();

            DBXmlMethods.generateResponse(sResultado, listData);

            return Ok(listData);
        }

        

    }
}
