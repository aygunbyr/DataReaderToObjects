using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace DataReaderToObjects
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            List<Product> products = null;
            string connStr = @"Data Source=DESKTOP-5R2AJR3\SQLEXPRESS;Database=Northwind;Integrated Security=true;TrustServerCertificate=True;";
            using(SqlConnection cn = new SqlConnection(connStr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Select ProductName as Name, UnitPrice as Price, UnitsInStock as Units from Products", cn);
                var dataReader = cmd.ExecuteReader();
                products = GetList<Product>(dataReader);


            }

            if(products!= null)
            {
                dgvProducts.DataSource = products;
            }

        }

        private List<T> GetList<T>(IDataReader reader)
        {
            List<T> list = new List<T>();
            while(reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach(var prop in type.GetProperties())
                {
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(reader[prop.Name].ToString(), propType));
                }
                list.Add(obj);

            }
            return list;
        }
    }
}
