using Ferreteria.API.SharedCode;
using Ferreteria.API.Utils;
using Ferreteria.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Ferreteria.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {

        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Clientes>> GetValidarAcceso([FromBody] Usuarios user)
        {
            var cadenaConexion = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["conexion_bd_user"];
            XDocument xmlParam = DBXmlMethods.GetXml(user);
            DataSet sResultado = await DBXmlMethods.EjecutaBase("ValidateUser", cadenaConexion, "VALIDA_USUARIO", xmlParam.ToString());

            if (sResultado.Tables.Count > 0)
            {
                try
                {
                    if (sResultado.Tables[0].Rows.Count > 0)
                    {
                        Usuarios userTmp = new Usuarios();
                        userTmp.Id = Convert.ToInt32(sResultado.Tables[0].Rows[0]["Id"]);
                        userTmp.Usuario = sResultado.Tables[0].Rows[0]["Usuario"].ToString();
                        return Ok(JsonConvert.SerializeObject(CrearToken(userTmp)));
                    }
                    else
                    {
                        RespuestaSP objResponse = new RespuestaSP();
                        objResponse.Respuesta = "Error";
                        objResponse.Leyenda = "Error en las credenciales de acceso";
                        return BadRequest(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Ok();
        }

        private string CrearToken(Usuarios user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Usuario),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
                GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }


    }
}
