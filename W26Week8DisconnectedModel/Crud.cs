using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;

namespace W26Week8DisconnectedModel
{
    public class Crud
    {
        private SqlConnection _conn;
        private SqlDataAdapter _adp;
        private SqlCommandBuilder _cmdBuilder;
        private DataSet _ds;
        private DataTable _tblProds;

        public Crud()
        {
            _conn = new SqlConnection(Data.GetConnectionString());

            string query = "select ProductID, ProductName, UnitPrice, UnitsInStock from Products";
            _adp = new SqlDataAdapter(query, _conn);

            _cmdBuilder = new SqlCommandBuilder(_adp);
        }

        private void InitProductsTable()
        {
            _ds = new DataSet();
            _adp.Fill(_ds, "Products");

            _tblProds = _ds.Tables["Products"]!;

            // define the primary key
            DataColumn[] pk = new DataColumn[1];
            pk[0] = _tblProds.Columns["ProductID"]!;
            pk[0].AutoIncrement = true;
            _tblProds.PrimaryKey = pk;
        }

        public DataTable GetAllProducts()
        {
            InitProductsTable();
            return _tblProds;
        }

        public DataRow? GetProductById(int id)
        {
            var row = _tblProds.Rows.Find(id);
            return row;
        }
    }
}
