using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace W26Week8DisconnectedModel
{
    public class Data
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
        }

        public DataTable GetAllProducts()
        {
            SqlConnection conn = new SqlConnection(GetConnectionString());

            string query = "select ProductID, ProductName, UnitPrice, UnitsInStock from Products";
            SqlDataAdapter adp = new SqlDataAdapter(query, conn);

            DataSet ds = new DataSet();

            adp.Fill(ds);

            DataTable tbl = ds.Tables[0];
            return tbl;
        }
    }
}
