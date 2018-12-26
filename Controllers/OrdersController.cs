using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses;
using RestCostumerService.Model;

namespace RestCostumerService.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly string ConnectionString = CustomerController.ConnectionString;
        
        [HttpGet]
       public List<Orders> Get(/*string customerId*/)
        {
            List<Orders> OrderList = new List<Orders>();
            
            string sql = "SELECT * FROM Orders";
            
            using (SqlConnection DBConnection = new SqlConnection(ConnectionString))
            {
                DBConnection.Open();
                using (SqlCommand SelectCommand = new SqlCommand(sql, DBConnection))
                {
                    using (SqlDataReader reader = SelectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                               var Order = new Orders(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                               OrderList.Add(Order);
                            }

                        }
                    }

                }
            }
            return OrderList;
        }

        [HttpGet("{id}")]
        public List<Orders> GetOrdersForCustomer(int id)
        {
            List<Orders> OrderList = new List<Orders>();
            string sql = "SELECT Orders.OrderId, Customer.LastName, Orders.KundeId FROM Orders, Customer WHERE Orders.KundeId=@Id AND Orders.KundeId = Customer.Id";
            //string sql ="SELECT KundeId, FirstName, LastName, OrderId FROM Orders RIGHT JOIN Customer C ON Orders.KundeId = C.Id WHERE C.Id=" + id;
            using (SqlConnection DBConnection = new SqlConnection(ConnectionString))
            {
                DBConnection.Open();
                using (SqlCommand SelectCommand = new SqlCommand(sql, DBConnection))
                {
                    SelectCommand.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = SelectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                var Order = new Orders(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                                OrderList.Add(Order);
                            }

                        }
                    }

                }
            }
            return OrderList;
        }




        // GET: api/Orders/5
        [HttpPost]
        public int Post([FromBody] Orders Order)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string SqlQuery = "INSERT INTO Customer(OrderId, OrderBeskrivelse) VALUES (@OrderId, @OrderBeskrivelse)";
                using (SqlCommand cmd = new SqlCommand(SqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", Order.OrderId);
                    cmd.Parameters.AddWithValue("@OrderBeskrivelse", Order.OrderDescription);
                    int RowsAffected = cmd.ExecuteNonQuery();

                    return RowsAffected;

                }
            }
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Orders Order)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string SqlQuery = "UPDATE Orders SET OrderBeskrivelse=@OrderBeskrivelse WHERE OrderId=@Id;";
                using (SqlCommand cmd = new SqlCommand(SqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", id);
                    cmd.Parameters.AddWithValue("@OrderBeskrivelse", Order.OrderDescription);
                    int RowsAffected = cmd.ExecuteNonQuery();

                    return RowsAffected;

                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            const string deleteStatement = "DELETE FROM Orders WHERE OrderId=@id";

            using (SqlConnection databaseConnection = new SqlConnection(ConnectionString))
            {

                databaseConnection.Open();

                using (SqlCommand insertCommand = new SqlCommand(deleteStatement, databaseConnection))

                {

                    insertCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    return rowsAffected;

                }

            }
        }
        [HttpGet("/api/Customer/Orders/{id}")]
        public Orders GetCustomerData([FromRoute]int id)
        {
            Orders ord = new Orders();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                string sqlQuery = "SELECT * FROM Orders WHERE OrderId= " + id;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ord = new Orders(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));
                    //employee.ID = Convert.ToInt32(rdr["EmployeeID"]);
                    //employee.Name = rdr["Name"].ToString();
                    //employee.Gender = rdr["Gender"].ToString();
                    //employee.Department = rdr["Department"].ToString();
                    //employee.City = rdr["City"].ToString();
                }
            }
            return ord;
        }
    }
}
