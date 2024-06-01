using UnityEngine;

namespace UnitSystem
{
    public interface IAreaSelectionView
    {
        public void EnableView(Vector2 startPoint);

        public void SetDragPoint(Vector2 dragPoint);

        public void DisableView();
    }
}