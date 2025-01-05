using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs
{
    [CreateAssetMenu(fileName = "EnviromentExplosionsConfig", menuName = "Configs/Create new enviroment explosions config", order = 51)]
    public class EnviromentExplosionsConfig : ScriptableObject
    {
        public float ExplosionRadius;
        public uint Damage;
        public uint ExplosionForce;
    }
}