using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.Utilities;

namespace Assets.Sources.Services.StaticDataService
{
    public interface IStaticDataService
    {
        LevelConfig GetLevel(string key);
        UniTask InitializeAsync();
    }
}