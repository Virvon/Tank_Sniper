using Assets.Sources.Gameplay.Player.Aiming;
using UnityEngine;

namespace Assets.Sources.Gameplay.Root
{
    public class DroneLevelInstaller : GameplayInstaller
    {
        [SerializeField] private uint _dronesCount;

        public override void InstallBindings()
        {
            base.InstallBindings();
            BindDronesCount();
        }

        private void BindDronesCount() =>
            Container.BindInstance(_dronesCount).AsSingle();

        protected override void BindAiming() =>
            Container.BindInterfacesAndSelfTo<DroneAiming>().AsSingle();

        protected override void BindGameplayBootstrapper() =>
            Container.BindInterfacesTo<DroneLevelBootstrapper>().AsSingle().NonLazy();
    }
}
