using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using RestCostumerService.Controllers;

namespace RestCostumerService.Model
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public int Year { get; set; }

        public Customer(int id, string firstName, string lastName, int year)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Year = year;
        }

        public Customer()
        {
            
        }
    }
}
