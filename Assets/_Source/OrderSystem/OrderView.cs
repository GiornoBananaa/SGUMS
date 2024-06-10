using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace OrderSystem
{
    public class OrderView: MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _icon;

        private Orders _orderType;
        private OrderPanelView _panelView;
        
        [Inject]
        public void Construct(OrderPanelView panelView, OrderDataSO orderData)
        {
            _icon.sprite = orderData.Icon;
            _orderType = orderData.OrderType;
            _panelView = panelView;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _panelView.ExecuteOrder(_orderType);
        }
    }
}