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
        public string SP1_Agent_With_Higest_Sum(int year)
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
                        //run stored procedure
                        SqlDataReader reader = command.ExecuteReader();
                        string strJSON = "";

                        if (reader.Read())  //get results
                        {
                            Dictionary<String, Object> record = new Dictionary<String, Object>();
                            record["AGENT_CODE"] = reader["AGENT_CODE"]; 
                            //put results to json
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            strJSON = js.Serialize(record);  
                        }
                        return strJSON;
                    }
                    catch
                    {
                        return "No result.";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        [WebMethod]
        public string SP2_Nth_Order_per_Agent(int numOforders,string agentList)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"];
            string strConnectionString = connectionString.ConnectionString;

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand command = new SqlCommand("sp_nth_order_per_agent", connection))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@agentList";
                    param1.SqlDbType = SqlDbType.NVarChar;
                    param1.Value = agentList;
                    command.Parameters.Add(param1);
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@num";
                    param2.SqlDbType = SqlDbType.Int;
                    param2.Value = numOforders;
                    command.Parameters.Add(param2);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        //run stored procedure
                        SqlDataReader reader = command.ExecuteReader();
                        string strJSON = "";

                        while (reader.Read()) //get result
                        {
                            Dictionary<String, Object> record = new Dictionary<String, Object>();
                            record["ORD_NUM"] = reader["ORD_NUM"];
                            record["ORD_AMOUNT"] = reader["ORD_AMOUNT"];
                            record["ADVANCE_AMOUNT"] = reader["ADVANCE_AMOUNT"];
                            record["ORD_DATE"] = reader["ORD_DATE"];
                            record["CUST_CODE"] = reader["CUST_CODE"];
                            record["AGENT_CODE"] = reader["AGENT_CODE"];
                            record["ORD_DESCRIPTION"] = reader["ORD_DESCRIPTION"];

                            //put result to json
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            strJSON+= js.Serialize(record);  
                        }
                        return strJSON;
                    }
                    catch
                    {
                        return "No Result.";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        [WebMethod]
        public string SP3_Agents_Orders_per_Year(int year, int num)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"];
            string strConnectionString = connectionString.ConnectionString;

            // Create the connection.
            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                // Create a SqlCommand, and identify it as a stored procedure.
                using (SqlCommand command = new SqlCommand("sp_agents_with_num_orders_per_year", connection))
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@year";
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Value = year;
                    command.Parameters.Add(param1);
                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@num";
                    param2.SqlDbType = SqlDbType.Int;
                    param2.Value = num;
                    command.Parameters.Add(param2);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    try
                    {
                        connection.Open();
                        //run stored procedure
                        SqlDataReader reader = command.ExecuteReader();
                        string strJSON = "";

                        while (reader.Read()) //get result
                        {
                            Dictionary<String, Object> record = new Dictionary<String, Object>();
                            record["AGENT_CODE"] = reader["AGENT_CODE"];
                            record["AGENT_NAME"] = reader["AGENT_NAME"];
                            record["PHONE_NO"] = reader["PHONE_NO"];

                            //put result to json
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            strJSON+= js.Serialize(record);
                           
                        } 
                        return strJSON;
                    }
                    catch
                    {
                        return "No Result.";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
