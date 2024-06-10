using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OrderSystem
{
    [CreateAssetMenu(fileName = "OrdersPanelData", menuName = "SO/OrdersPanelData")]
    public class OrderPanelDataSO : ScriptableObject
    {
        [field: SerializeField] public OrderDataSO[] OrdersData { get; private set; }
        [field: SerializeField] public OrderView OrderPrefab { get; private set; }

        private Dictionary<Orders, OrderDataSO> _ordersByOrderType;
        
        public Dictionary<Orders, OrderDataSO> GetOrdersByOrderType()
        {
            return _ordersByOrderType ??= OrdersData.ToDictionary(i => i.OrderType, i => i);
        }
    }
}