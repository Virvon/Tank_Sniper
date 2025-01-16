using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.MainMenu.Animations;
using Assets.Sources.Services.PersistentProgress;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.CharacterPoints
{
    public abstract class SelectedTankPoint : MonoBehaviour
    {
        [SerializeField] private Transform _tankPoint;
        [SerializeField] private Quaternion _spawnRotation;
        [SerializeField] private TankScalingAnimator _scalingAnimator;

        private ITankFactory _tankFactory;

        protected GameObject SelectedTank { get; private set; }
        protected IPersistentProgressService PersistentProgressService { get; private set; }
        protected Transform TankPoint => _tankPoint;

        [Inject]
        private void Construct(IPersistentProgressService persistentProgressService, ITankFactory tankFactory)
        {
            PersistentProgressService = persistentProgressService;
            _tankFactory = tankFactory;

            PersistentProgressService.Progress.SelectedTankChanged += OnSelectedTankChanged;
        }

        private async void Start() =>
            await OnStart();

        private void OnDestroy() =>
            PersistentProgressService.Progress.SelectedTankChanged -= OnSelectedTankChanged;

        private async void OnSelectedTankChanged(uint level) =>
            await ChangeSelectedTank(level, true);

        protected virtual async UniTask ChangeSelectedTank(uint level, bool needToAnimate)
        {
            TankData tankData = PersistentProgressService.Progress.GetTank(level);

            if (SelectedTank != null)
                Destroy(SelectedTank);

            SelectedTank = await CreateTank(tankData, _tankPoint.position, GetRotation(), GetParent(), _tankFactory);

            if (needToAnimate)
                _scalingAnimator.Play();
        }

        protected abstract UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory);

        protected virtual Quaternion GetRotation() =>
            _spawnRotation;

        protected async virtual UniTask OnStart() =>
            await ChangeSelectedTank(PersistentProgressService.Progress.SelectedTankLevel, false);

        protected virtual Transform GetParent() =>
            transform;
    }
}