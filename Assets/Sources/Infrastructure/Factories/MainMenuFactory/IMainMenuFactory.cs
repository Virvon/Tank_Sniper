using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.Factories.MainMenuFactory
{
    public interface IMainMenuFactory
    {
        UniTask CreateDesk();
    }
}