using Assets.Sources.Gameplay.Handlers;
using Assets.Sources.Gameplay.Player.Aiming;

namespace Assets.Sources.Gameplay.Root
{
    public class TankLevelInstaller : GameplayInstaller
    {
        protected override void BindAiming() =>
            Container.BindInterfacesAndSelfTo<TankAiming>().AsSingle();

        protected override void BindGameplayBootstrapper() =>
            Container.BindInterfacesTo<GameplayBootstrapper>().AsSingle().NonLazy();
    }
}
