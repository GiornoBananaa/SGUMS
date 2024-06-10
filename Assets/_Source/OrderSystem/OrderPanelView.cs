using System.Collections.Generic;
using System.Linq;
using SelectionSystem;
using UnityEngine;
using Zenject;

namespace OrderSystem
{
    public class OrderPanelView : MonoBehaviour
    {
        private readonly Dictionary<Orders, OrderView> _orderViews = new();
        [SerializeField] private RectTransform _ordersLayout;
        private UnitSelection _unitSelection;
        private OrderContainer _orderContainer;
        private OrderPanelDataSO _orderPanelData;
        private OrderView _orderPrefab;
        
        [Inject]
        public void Construct(UnitSelection unitSelection, 
            OrderContainer orderContainer, OrderPanelDataSO orderPanelData)
        {
            _unitSelection = unitSelection;
            _orderContainer = orderContainer;
            _orderPanelData = orderPanelData;
            _orderPrefab = orderPanelData.OrderPrefab;
        }
        
        private void Awake()
        {
            _unitSelection.OnSelectionChanged += UpdateOrders;
            foreach (var orderData in _orderPanelData.OrdersData)
            {
                OrderView orderView = Instantiate(_orderPrefab, _ordersLayout);
                orderView.Construct(this,_orderPanelData.GetOrdersByOrderType()[orderData.OrderType]);
                _orderViews.Add(orderData.OrderType,orderView);
                orderView.gameObject.SetActive(false);
            }
        }

        private void UpdateOrders()
        {
            ClearOrders();
            foreach (var order in _orderContainer.ActiveOrders)
            {
                AddOrder(order.OrderType);
            }
        }

        public void ExecuteOrder(Orders orderType)
        {
            _orderContainer[orderType].Execute();
        }
        
        private void AddOrder(Orders orderType)
        {
            _orderViews[orderType].gameObject.SetActive(true);
        }
        
        private void ClearOrders()
        {
            foreach (var orderType in _orderViews.Keys)
            {
                _orderViews[orderType].gameObject.SetActive(false);
            }
        }
    }
}
