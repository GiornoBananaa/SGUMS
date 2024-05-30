using System.Collections.Generic;

namespace UnitSystem
{
    public class UnitSelector
    {
        public readonly HashSet<ISelectable> SelectedObjects = new();
        
        public void Select(IEnumerable<ISelectable> selectables)
        {
            foreach (var selectable in selectables)
            {
                Select(selectable);
            }
        }
        
        public void Select(ISelectable selectable)
        {
            SelectedObjects.Add(selectable);
            selectable.SelectionProjector.enabled = true;
        }
        
        public void Deselect(ISelectable selectable)
        {
            SelectedObjects.Remove(selectable);
            selectable.SelectionProjector.enabled = true;
        }

        public void DeselectAll()
        {
            foreach (var selectable in SelectedObjects)
            {
                selectable.SelectionProjector.enabled = false;
            }
            SelectedObjects.Clear();
        }
    }
}
