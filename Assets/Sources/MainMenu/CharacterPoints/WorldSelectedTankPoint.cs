using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Assets.Sources.Infrastructure.Factories.TankFactory;
using Assets.Sources.Services.PersistentProgress;
using Assets.Sources.Tanks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Sources.MainMenu.CharacterPoints
{
    public class WorldSelectedTankPoint : SelectedTankPoint
    {
        [SerializeField] private TMP_Text _tankLevelValue;

        private readonly Vector3 _offset = new Vector3(0, 2, 0);

        protected override async UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            ITankFactory tankFactory)
        {
            TankShootingWrapper tankWrapper = await tankFactory.CreateTankShootingWrapper(tankData.Level, position, rotation, parent);

            Tank tank = await tankFactory.CreateTank(
                tankData.Level,
                position,
                tankWrapper.transform.rotation,
                tankWrapper.transform,
                tankData.SkinId,
                tankData.DecalId,
                true);

            await tankFactory.CreatePlayerCharacter(
                PersistentProgressService.Progress.SelectedPlayerCharacterId,
                tank.transform.position + _offset,
                tank.transform.rotation,
                tank.transform);

            tankWrapper.SetBulletPoints(tank.BulletPoints);

            _tankLevelValue.text = tankData.Level.ToString();

            return tankWrapper.gameObject;
        }

        protected override Transform GetParent() =>
            TankPoint.transform;
    }
}