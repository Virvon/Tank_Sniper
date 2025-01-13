using Assets.Sources.Gameplay.Bullets;
using Assets.Sources.Services.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Infrastructure.Factories.BulletFactory
{
    public class BulletFactoryInstaller : Installer<BulletFactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BulletFactory>().AsSingle();

            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<CollidingBullet>, BulletFactory.Factory<CollidingBullet>>()
               .FromFactory<ReferencePrefabFactoryAsync<CollidingBullet>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<HomingBullet>, BulletFactory.Factory<HomingBullet>>()
               .FromFactory<ReferencePrefabFactoryAsync<HomingBullet>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Laser>, BulletFactory.Factory<Laser>>()
               .FromFactory<ReferencePrefabFactoryAsync<Laser>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TargetingLaser>, BulletFactory.Factory<TargetingLaser>>()
               .FromFactory<ReferencePrefabFactoryAsync<TargetingLaser>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<TransmittingLaser>, BulletFactory.Factory<TransmittingLaser>>()
               .FromFactory<ReferencePrefabFactoryAsync<TransmittingLaser>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<CompositeBullet>, BulletFactory.Factory<CompositeBullet>>()
               .FromFactory<ReferencePrefabFactoryAsync<CompositeBullet>>();
            
            Container
               .BindFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Muzzle>, Muzzle.Factory>()
               .FromFactory<ReferencePrefabFactoryAsync<Muzzle>>();
        }
    }
}