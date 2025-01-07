using Assets.Sources.Types;
using System;

namespace Assets.Sources.Gameplay.Enemies.MaterialChangerLogic
{
    [Serializable]
    public class MatchingMaterialToBiome
    {
        public BiomeType BiomeType;
        public MaterialInfo[] MaterialInfos;
    }
}
