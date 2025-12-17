using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed,
        [EnumMember(Value = "Shipped")]
        Shipped,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
    }
}
