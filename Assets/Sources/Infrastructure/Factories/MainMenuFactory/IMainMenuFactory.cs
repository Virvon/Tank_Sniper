using Assets.Sources.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public interface IMainMenuFactory
    {
        UniTask CreateDesk();
        UniTask<Tank> CreateTank(Vector3 position, Quaternion rotation, Transform parent);
    }
}