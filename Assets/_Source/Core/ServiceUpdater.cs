using System.Collections.Generic;
using Zenject;

namespace Core
{
    public class ServiceUpdater: ITickable
    {
        private List<IUpdatable> _updatables;
        private Queue<IUpdatable> _subscribeQueue;
        private Queue<IUpdatable> _unsubscribeQueue;

        public ServiceUpdater()
        {
            _updatables = new List<IUpdatable>();
            _subscribeQueue = new Queue<IUpdatable>();
            _unsubscribeQueue = new Queue<IUpdatable>();
        }

        public void Subscribe(IUpdatable updatable)
        {
            _subscribeQueue.Enqueue(updatable);
        }

        public void Unsubscribe(IUpdatable updatable)
        {
            _unsubscribeQueue.Enqueue(updatable);
        }

        public void Tick()
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update();
            }
            while (_unsubscribeQueue.Count>0)
            {
                _updatables.Remove(_unsubscribeQueue.Dequeue());
            }
            while (_subscribeQueue.Count>0)
            {
                _updatables.Add(_subscribeQueue.Dequeue());
            }
        }
    }
}