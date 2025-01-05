using Assets.Sources.Types;
using UnityEngine;

namespace Assets.Sources.Services.StaticDataService.Configs.Level.Sequence
{
    [CreateAssetMenu(fileName = "LevelsSequenceConfig", menuName = "Configs/Create new levels sequence config", order = 51)]
    public class LevelsSequenceConfig : ScriptableObject, IConfig<BiomeType>
    {
        public BiomeType Type;
        public string[] Sequence;
        public string MainMenuScene;

        public BiomeType Key => Type;

        public string GetLevel(uint index)
        {
            if (index >= Sequence.Length)
            {
                Debug.LogError("Level index incorrect");
                return string.Empty;
            }

            return Sequence[index];
        }
    }
}