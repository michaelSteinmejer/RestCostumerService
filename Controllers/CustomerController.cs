using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Remotion.Linq.Clauses;
using RestCostumerService.Model;

namespace RestCostumerService.Controllers
{
    [Produces("application/json")]
    [Route("api/customer")]

    
    public class CustomerController : Controller
    {
       

        public static string ConnectionString =
            "Server=tcp:michaelsteinmejer.database.windows.net,1433;Initial Catalog = SteinmejerDB; Persist Security Info=False;User ID = michaelsteinmejer; Password=Password123; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";
        
    //private string ConnectionString =
    // "Server = tcp:steinserver.database.windows.net,1433;Initial Catalog = SteinDB; Persist Security Info=False; User ID = Stein; Password = Password123; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;"

    public static int nextId = 0;
        private static List<Customer> CList = new List<Customer>() {
            new Customer(nextId++, "Michael", "steinmejer", 1990),
            new Customer(nextId++, "Michael", "steinmejer", 1990),
            new Customer(nextId++, "Michael", "steinmejer", 1990) };


        #region CRUDmedDB

        // GET: api/Customer
        [HttpGet]

        public List<Customer> Get()
        {
            List<Customer> customerList = new List<Customer>();
            //string sql = "SELECT * FROM Customer";

            using (SqlConnection DBConnection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand SelectCommand = new SqlCommand("SQLQuery2", DBConnection))
                {
                    SelectCommand.CommandType = CommandType.StoredProcedure;
                    DBConnection.Open();
                    using (SqlDataReader reader = SelectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                var cust = new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                              customerList.Add(cust);
                            }

                        }
                    }

                }
            }
            return customerList;
        }


        // GET: api/Customer/5
        [HttpPost]
        public int AddCustomer([FromBody] Customer cust)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string SqlQuery = "INSERT INTO Customer(FirstName, LastName, Year) VALUES (@FirstName, @LastName, @Year)";
                using (SqlCommand cmd = new SqlCommand(SqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@FirstName", cust.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cust.LastName);
                    cmd.Parameters.AddWithValue("@Year", cust.Year);

                    int RowsAffected = cmd.ExecuteNonQuery();



                    return RowsAffected;

                }
            }
        }

        [HttpPut("{id}")]
        public int UpdateCustomer(int id, [FromBody]Customer cust)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string SqlQuery = "UPDATE Customer SET FirstName=@FirstName, LastName=@LastName, Year=@Year WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(SqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@FirstName", cust.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", cust.LastName);
                    cmd.Parameters.AddWithValue("@Year", cust.Year);

                    int RowsAffected = cmd.ExecuteNonQuery();



                    return RowsAffected;


                }
            }

        }
        [HttpGet("{id}")]
        public Customer GetCustomerData(int id)
        {
            Customer cust = new Customer();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {

                string sqlQuery = "SELECT * FROM Customer WHERE Id= " + id;
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cust = new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                    //employee.ID = Convert.ToInt32(rdr["EmployeeID"]);
                    //employee.Name = rdr["Name"].ToString();
                    //employee.Gender = rdr["Gender"].ToString();
                    //employee.Department = rdr["Department"].ToString();
                    //employee.City = rdr["City"].ToString();
                }
            }
            return cust;
        }

        [HttpDelete("{id}")]

        public int DeleteCustomer(int id)

        {

            const string deleteStatement = "DELETE FROM Customer WHERE Id=@id";

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
        #endregion

        #region CRUDudenDB
        //// GET: api/Customer
        //[HttpGet]
        //public IEnumerable<Customer> GetAllCustomer()
        //{
        //    return CList;
        //}
        //// GET: api/Customer/1
        //[HttpGet("{id}")]
        //public IActionResult GetCustomer(int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    // return CList.Find(c => c.Id == id);
        //    var Cust = CList.SingleOrDefault(c => c.Id == id);
        //    if (Cust == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Cust);
        //}

        //// POST: api/Customer
        //[HttpPost]
        //public IActionResult Post([FromBody]Customer value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    value.Id = nextId++;
        //    CList.Add(value);
        //    return Ok(value);
        //}




        //DELETE: api/Customer/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    //CList.Remove(CList.Find(c => c.Id == id));
        //    var Cust = CList.SingleOrDefault(c => c.Id == id);
        //    CList.Remove(Cust);
        //    if (Cust == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Cust);
        //}

        ////PUT: api/Customer/5
        //[HttpPut("{id}")]

        //public IActionResult Put(int id, [FromBody]Customer customer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var Cust = CList.SingleOrDefault(c => c.Id == id);

        //    Cust.FirstName = customer.FirstName;
        //    Cust.LastName = customer.LastName;
        //    Cust.Year = customer.Year;
        //    if (Cust.Id != id)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Cust);
        //}
        #endregion
    }
}
