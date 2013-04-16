using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Week4_AddtoCart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["ProductID"] != null && Request.Params["ProductID"].ToString().Length > 0)
        {
            //insert into the order lines. 
            //first I need to check to see if they have an order....
            //insert the order, and set the session variable.
            System.Data.SqlClient.SqlConnection conn =
                            new System.Data.SqlClient.SqlConnection();
            var conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"];
            conn.ConnectionString = conString.ConnectionString;

            conn.Open();
            System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
            comm.Connection = conn;

            string sql = "";
            //Response.Write(conn.State.ToString());

            //now go and get that latest orderid
            sql = "select max(OrderID) + 1 from LineItems";

            comm.Parameters.Clear();

            comm.CommandText = sql;
            object result;
            result = comm.ExecuteScalar();

            sql = "insert into LineItems (OrderId, Productid, Quantity)" +
                " values (@OrderId, @Productid, @Quantity)";

            comm.CommandText = sql;

 
            comm.Parameters.AddWithValue("@OrderId", (int)result);
            comm.Parameters.AddWithValue("@ProductID", Request.Params["ProductID"].ToString());
            comm.Parameters.AddWithValue("@Quantity", "1");
            
            comm.ExecuteNonQuery();

            
            



            lblDisplay.Text = "Your item was added to your cart!";

        }
    }

    private int GetCustomerOrder(int CustomerID)
    {
        if (Session["OrderID"] == null || Session["OrderID"].ToString().Length <= 0)
        {
             System.Data.SqlClient.SqlConnection conn =
                            new System.Data.SqlClient.SqlConnection();
             var conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"];
            conn.ConnectionString = conString.ConnectionString;

            conn.Open();
            System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
            comm.Connection = conn;

            string sql = "";
             //now go and get that latest orderid
            sql = "select max(OrderID) + 1 from Orders";
            
            comm.Parameters.Clear();

            comm.CommandText = sql;
            object result;
            result = comm.ExecuteScalar();

            if (result != null && result.ToString().Length > 0)
            {
                Session["OrderID"] = (int)result;
            }
            else
            {
                Session["OrderID"] = -1;
            }
           
            //insert the order, and set the session variable.
           
            //Response.Write(conn.State.ToString());

            sql = "insert into Orders (OrderID, OrderDate, CustomerID)" +
                " values (@OrderID, @OrderDate, @CustomerID)";


            comm.CommandText = sql;
            //comm.CommandText = mysb.ToString();
            comm.Parameters.AddWithValue("@OrderID", Session["OrderID"].ToString());
            comm.Parameters.AddWithValue("@OrderDate", DateTime.Now.ToString());
            comm.Parameters.AddWithValue("@CustomerID", CustomerID);

            comm.ExecuteNonQuery();
            

        }
       
            return int.Parse(Session["OrderID"].ToString());
       

    }
}