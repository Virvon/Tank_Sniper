using Cysharp.Threading.Tasks;

namespace Assets.Sources.Infrastructure.Factories.UiFactory
{
    public interface IUiFactory
    {
        UniTask CreateWindow();
    }
}