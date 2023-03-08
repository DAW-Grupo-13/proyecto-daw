using Microsoft.Data.SqlClient;
using Ferreteria.BL;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Ferreteria.API.SharedCode
{
    public static class DBXmlMethods
    {
        public static XDocument GetXml<T>(T criterio)
        {
            XDocument resultado = new XDocument(new XDeclaration("1.0", "utf-8", "true"));
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using XmlWriter xw = resultado.CreateWriter(); xs.Serialize(xw, criterio);
                return resultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<DataSet> EjecutaBase(string nombreProcedimiento, string cadenaConexion, string transaccion, string dataXML)
        {
            DataSet dsResultado = new DataSet();
            SqlConnection cnn = new SqlConnection(cadenaConexion);
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter adt = new SqlDataAdapter();
                cmd.CommandText = nombreProcedimiento;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = cnn;
                cmd.CommandTimeout = 120;
                cmd.Parameters.Add("@iTransaccion", SqlDbType.VarChar).Value = transaccion;
                cmd.Parameters.Add("@iXML", SqlDbType.Xml).Value = dataXML;
                await cnn.OpenAsync().ConfigureAwait(false);
                adt = new SqlDataAdapter(cmd);
                adt.Fill(dsResultado);
                cmd.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.Write("Logs", "EjecutaBase", ex.ToString());
                cnn.Close();
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

            return dsResultado;

        }


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
