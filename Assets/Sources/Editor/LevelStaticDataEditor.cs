using Assets.Sources.Gameplay.Enemies;
using Assets.Sources.Services.StaticDataService.Configs.Building;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public static Dictionary<string, LevelConfig> LevelDatas = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelConfig levelData = (LevelConfig)target;

            if (GUILayout.Button("Collect"))
            {
                List<EnemyPointConfig> collectedEnemyPoints = FindObjectsOfType<EnemyPoint>().Select(value => new EnemyPointConfig(value.Id, value.transform.position, value.transform.rotation)).ToList();

                //foreach (var value in collectedEnemyPoints)
                //{
                //    foreach (var value2 in levelData.EquipmentSpawners)
                //    {
                //        if (value.Id == value2.Id)
                //            value.SpawnType = value2.SpawnType;
                //    }
                //}

                levelData.EnemyPoints = collectedEnemyPoints;

                levelData.LevelKey = SceneManager.GetActiveScene().name;
            }

            if (LevelDatas.ContainsKey(levelData.LevelKey) == false)
                LevelDatas.Add(levelData.LevelKey, levelData);

            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(target);
        }
    }
}