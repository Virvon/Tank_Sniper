using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Sources.MainMenu.CharacterPoints
{
    public class WorldSelectedTankPoint : SelectedTankPoint
    {
        [SerializeField] private TMP_Text _tankLevelValue;

        private readonly Vector3 _offset = new Vector3(0, 2, 0);

        private PlayerCharacter _playerCharacter;
        private Tank _tank;

        protected override async UniTask OnStart()
        {
            await base.OnStart();
            PersistentProgressService.Progress.CharacterSkinChanged += OnCharacterSkinChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PersistentProgressService.Progress.CharacterSkinChanged -= OnCharacterSkinChanged;
        }

        protected override async UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory)
        {
            TankShootingWrapper tankWrapper = await tankFactory.CreateTankShootingWrapper(tankData.Level, position, rotation, parent);

            _tank = await tankFactory.CreateTank(
                tankData.Level,
                position,
                tankWrapper.transform.rotation,
                tankWrapper.transform,
                tankData.SkinId,
                tankData.DecalId,
                true);

            _playerCharacter = await CreatePlayerCharacter(tankFactory, _tank);

            tankWrapper.SetBulletPoints(_tank.BulletPoints);

            _tankLevelValue.text = tankData.Level.ToString();

            return tankWrapper.gameObject;
        }

        protected override Transform GetParent() =>
            TankPoint.transform;

        private async void OnCharacterSkinChanged(string characterId)
        {
            if (_playerCharacter != null)
                Destroy(_playerCharacter.gameObject);

            _playerCharacter = await CreatePlayerCharacter(TankFactory, _tank);
        }

        private async UniTask<PlayerCharacter> CreatePlayerCharacter(ITankFactory tankFactory, Tank tank)
        {
            return await tankFactory.CreatePlayerCharacter(
                PersistentProgressService.Progress.SelectedPlayerCharacterId,
                tank.transform.position + _offset,
                tank.transform.rotation,
                tank.transform);
        }
    }
}