using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderSystem
{
    public class OrderContainer
    {
        private readonly Dictionary<Orders,IOrder> _orders;
        
        public OrderContainer(IEnumerable<IOrder> orders)
        {
            _orders = orders.ToDictionary(i => i.OrderType,i => i);
        }
        
        public IEnumerable<IOrder> ActiveOrders
        {
            get
            {
                foreach (var order in _orders.Values)
                {
                    if(order.Activated)
                        yield return order;
                }
            }
        }

        public IOrder this[Orders type]
        {
            get => _orders[type];
        }
    }
}