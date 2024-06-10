using UnityEngine;

namespace OrderSystem
{
    [CreateAssetMenu(fileName = "OrderData", menuName = "SO/OrderData")]
    public class OrderDataSO : ScriptableObject
    {
        [field: SerializeField] public Orders OrderType { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}