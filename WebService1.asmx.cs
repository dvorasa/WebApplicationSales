using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Data;

namespace WebApplicationSales
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public string SP1(int year)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"];
            string strConnectionString = connectionString.ConnectionString;

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand command = new SqlCommand("sp_agent_with_highest_sum", connection))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@year";
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Value = year;
                    command.Parameters.Add(param1);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            Dictionary<String, Object> record = new Dictionary<String, Object>();
                            record["AGENT_CODE"] = reader["AGENT_CODE"]; // "name" is column name in Users table

                            // בתוך reader
                            // יש לנו את השורה עם כל המידע של המשתמש
                            // נהפוך שורה זו למחרוזת ונחזיר אותה
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            string strJSON = js.Serialize(record);
                            return strJSON;
                        }
                    }
                    catch
                    {
                        return "error in sql command";
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return "Year not found in table.";
                }
            }
        }
    }
}
