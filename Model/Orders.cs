using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RestCostumerService.Model
{
    [DataContract]
    public class Orders
    {
        [DataMember]
        public int OrderId { get; set; }
        [DataMember]
        public string OrderDescription { get; set; }
        [DataMember]
        public int KundeId { get; set; }

        public Orders(int orderId, string orderDescription, int kundeId)
        {
            OrderId = orderId;
            OrderDescription = orderDescription;
            KundeId = kundeId;
        }

        public Orders()
        {
            
        }
    }
}
