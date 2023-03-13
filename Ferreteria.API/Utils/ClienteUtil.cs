using Ferreteria.BL;
using System.Data;

namespace Ferreteria.API.Utils
{
    public static class ClienteUtil
    {


        public static void generateResponse(DataSet sResultado, List<Clientes> listData)
        {
            if (sResultado.Tables.Count > 0)
            {
                try
                {
                    foreach (DataRow row in sResultado.Tables[0].Rows)
                    {
                        Clientes objResponse = new Clientes
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Cedula = row["Cedula"].ToString(),
                            Nombres = row["Nombres"].ToString(),
                            Apellidos = row["Apellidos"].ToString(),
                            Direccion = row["Direccion"].ToString(),
                            Edad = Convert.ToInt32(row["Edad"]),
                            Ciudad = new Ciudad
                            {
                                Id = Convert.ToInt32(row["Id"]),
                                Descripcion = row["Descripcion"].ToString()
                            }
                        };
                        listData.Add(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}
