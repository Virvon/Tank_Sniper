using Assets.Sources.Gameplay.Player;
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

        public PlayerTankWrapper PlayerTankWrapper { get; private set; }
        public Aiming Aiming { get; private set; }
        public LayerMask LayerMask { get; private set; }

        [Inject]
        private void Construct(PlayerTankWrapper playerTankWrapper, Aiming aiming, IStaticDataService staticDataService)
        {
            PlayerTankWrapper = playerTankWrapper;
            Aiming = aiming;
            LayerMask = staticDataService.EnemiesSettingsConfig.LayerMask;
        }

        protected void OnDestructed() =>
            Destructed?.Invoke();

        public class Factory : PlaceholderFactory<AssetReferenceGameObject, Vector3, Quaternion, UniTask<Enemy>>
        {
        }
    }
}
