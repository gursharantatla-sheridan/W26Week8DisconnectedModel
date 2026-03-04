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

            InitProductsTable();
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

        public void Insert(string name, decimal price, short quantity)
        {
            var row = _tblProds.NewRow();
            row["ProductName"] = name;
            row["UnitPrice"] = price;
            row["UnitsInStock"] = quantity;
            _tblProds.Rows.Add(row);    // required

            _adp.InsertCommand = _cmdBuilder.GetInsertCommand();
            _adp.Update(_tblProds);
        }

        public void Update(int id, string name, decimal price, short quantity)
        {
            var row = _tblProds.Rows.Find(id);

            row["ProductName"] = name;
            row["UnitPrice"] = price;
            row["UnitsInStock"] = quantity;

            _adp.UpdateCommand = _cmdBuilder.GetUpdateCommand();
            _adp.Update(_tblProds);
        }

        public void Delete(int id)
        {
            var row = _tblProds.Rows.Find(id);

            row!.Delete();

            _adp.DeleteCommand = _cmdBuilder.GetDeleteCommand();
            _adp.Update(_tblProds);
        }

        public DataTable SearchProductsByName(string name)
        {
            string query = "select ProductID, ProductName, UnitPrice, UnitsInStock from Products where ProductName LIKE @pName";

            SqlCommand cmd = new SqlCommand(query, _conn);
            cmd.Parameters.AddWithValue("pName", "%" + name + "%");

            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable GetCategories()
        {
            string query = "select CategoryID, CategoryName from Categories";

            SqlDataAdapter adp = new SqlDataAdapter(query, _conn);

            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds.Tables[0];
        }

        public DataTable GetProductsByCategory(int catId)
        {
            string query = "select p.ProductID, p.ProductName, c.CategoryName from Categories c" +
                " inner join Products p" +
                " on c.CategoryID = p.CategoryID" +
                " where p.CategoryID=@catId";

            SqlCommand cmd = new SqlCommand(query, _conn);
            cmd.Parameters.AddWithValue("catId", catId);

            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);

            return ds.Tables[0];
        }
    }
}
