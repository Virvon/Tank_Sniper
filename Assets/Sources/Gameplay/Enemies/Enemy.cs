using Assets.Sources.Gameplay.Player.Aiming;
using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Assets.Sources.Gameplay.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public event Action Destructed;

        public PlayerWrapper PlayerWrapper { get; private set; }
        public IShootedAiming Aiming { get; private set; }
        public LayerMask LayerMask { get; private set; }
        public bool IsDestructed { get; private set; }

        [Inject]
        private void Construct(PlayerWrapper playerWrapper, IShootedAiming aiming, IStaticDataService staticDataService)
        {
            PlayerWrapper = playerWrapper;
            Aiming = aiming;
            LayerMask = staticDataService.GameplaySettingsConfig.EnemyLayerMask;

            IsDestructed = false;
        }

        protected void OnDestructed()
        {
            IsDestructed = true;
            Destructed?.Invoke();
        }

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
