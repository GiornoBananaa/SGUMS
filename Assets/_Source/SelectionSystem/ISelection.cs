using System.Collections.Generic;

namespace SelectionSystem
{
    public interface ISelection<T>
    {
        IEnumerable<T> Selected { get; }
        int SelectedCount { get; }
        
        void Select(T selectable);
        void DeselectAll();
    }
}