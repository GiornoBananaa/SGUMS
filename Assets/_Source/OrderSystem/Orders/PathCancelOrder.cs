using System.Linq;
using UnitSystem.MovementSystem;

namespace OrderSystem
{
    public class PathCancelOrder : IOrder
    {
        private readonly PathContainer _pathContainer;
        private readonly PathCreator _pathCreator;
        private readonly PathDrawer _pathDrawer;

        public Orders OrderType => Orders.PathCancel;
        public bool Activated
        {
            get
            {
                foreach (var path in _pathContainer.AllPaths)
                {
                    if (_pathDrawer.PathIsShowed(path))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public PathCancelOrder(PathCreator pathCreator, PathContainer pathContainer, PathDrawer pathDrawer)
        {
            _pathContainer = pathContainer;
            _pathCreator = pathCreator;
            _pathDrawer = pathDrawer;
        }
        
        public void Execute()
        {
            for (int i = 0; i < _pathContainer.AllPaths.Count;)
            {
                Path path = _pathContainer.AllPaths[i];
                if (_pathDrawer.PathIsShowed(path))
                {
                    _pathCreator.DestroyPath(path);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}