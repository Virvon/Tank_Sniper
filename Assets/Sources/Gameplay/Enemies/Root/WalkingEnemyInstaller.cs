using UnityEngine;

namespace Assets.Sources.Gameplay.Enemies.Root
{
    public class WalkingEnemyInstaller : EnemyInstaller
    {
        [SerializeField] private Walking _walking;

        public override void InstallBindings()
        {
            base.InstallBindings();
            BindWalking();
        }

        protected override void BindEnemyBootstrapper() =>
            Container.BindInterfacesTo<WalkingEnemyBootstrapper>().AsSingle().NonLazy();

        private void BindWalking() =>
            Container.BindInstance(_walking).AsSingle();
    }
}
