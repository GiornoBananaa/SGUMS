using UnityEngine;

namespace UnitSystem
{
    public interface IAreaSelector
    {
        public void StartSelection(Vector2 startPoint);
        public void SetDragPoint(Vector2 dragPoint);
        public void EndSelection();
    }
}