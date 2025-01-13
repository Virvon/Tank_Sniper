using Assets.Sources.Infrastructure.Factories.UiFactory;
using Cysharp.Threading.Tasks;

namespace Assets.Sources.UI.Gameplay.BulletsPanel
{
    public class TankBulletsPanel : BulletsPanel
    {
        protected override async UniTask<BulletIcon> CreateBulletIcon(IUiFactory uiFactory) =>
            await uiFactory.CreateTankBulletIcon(transform);
    }
}
