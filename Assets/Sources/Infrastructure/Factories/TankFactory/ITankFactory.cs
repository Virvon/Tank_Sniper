using Assets.Sources.Gameplay.Player;
using Assets.Sources.Gameplay.Player.Wrappers;
using Assets.Sources.MainMenu;
using Assets.Sources.MainMenu.Desk;
using Assets.Sources.Tanks;
using Assets.Sources.Types;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.TankFactory
{
    public interface ITankFactory
    {
        UniTask<DeskTankWrapper> CreateDeskTankWrapper(Vector3 position, Transform parent);
        UniTask<Drone> CreateDrone(Vector3 position, Quaternion rotation);
        UniTask<PlayerCharacter> CreatePlayerCharacter(string id, Vector3 position, Quaternion rotation, Transform parent);
        UniTask CreatePlayerDroneContoller(Vector3 position, Quaternion rotation, Transform parent);
        UniTask CreatePlayerDroneWrapper(Vector3 position, Quaternion rotation);
        UniTask<PlayerAccessor> CreatePlayerGlasses(Vector3 position, Quaternion rotation, Transform parent);
        UniTask<PlayerTankWrapper> CreatePlayerTankWrapper(uint tankLevel, Vector3 position, Quaternion rotation);
        UniTask<Tank> CreateTank(
            uint level,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            string skinId,
            string decalId,
            bool isDecalsChangable = false);
        UniTask<TankShootingWrapper> CreateTankShootingWrapper(uint tankLevel, Vector3 position, Quaternion rotation, Transform parent);
    }
}