using UnitSystem;
using Zenject;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UnitSelector>().AsSingle();
        }
    }
}
