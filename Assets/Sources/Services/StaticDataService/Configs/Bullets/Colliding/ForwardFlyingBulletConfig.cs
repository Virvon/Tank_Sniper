using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Bullets.Colliding
{
    [CreateAssetMenu(fileName = "ForwardFlyingBulletConfig", menuName = "Configs/Bullets/Create new forward flying bullet config", order = 51)]
    public class ForwardFlyingBulletConfig : CollidingBulletConfig, IConfig<ForwardFlyingBulletType>
    {
        public ForwardFlyingBulletType Type;

        public ForwardFlyingBulletType Key => Type;
    }
}