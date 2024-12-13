using Assets.Sources.Data;
using Assets.Sources.Infrastructure.Factories.MainMenuFactory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.MainMenu
{
    public class WorldSelectedTankPoint : SelectedTankPoint
    {
        protected override async UniTask<GameObject> CreateTank(
            TankData tankData,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            IMainMenuFactory mainMenuFactory)
        {
            TankShootingWrapper tankWrapper = await mainMenuFactory.CreateTankShootingWrapper(tankData.Level, position, rotation, parent);

            Tank tank = await mainMenuFactory.CreateTank(
                tankData.Level,
                position,
                tankWrapper.transform.rotation,
                tankWrapper.transform,
                tankData.SkinType,
                tankData.DecalType,
                true);

            tankWrapper.SetBulletPoints(tank.BulletPoints);

            return tankWrapper.gameObject;
        }

        protected override Transform GetParent() =>
            TankPoint.transform;
    }
}