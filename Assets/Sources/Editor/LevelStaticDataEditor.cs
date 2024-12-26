using Assets.Sources.Gameplay.Enemies.Points;
using Assets.Sources.Gameplay.Player;
using Assets.Sources.Services.StaticDataService.Configs.Level;
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

            LevelConfig levelConfig = (LevelConfig)target;

            if (GUILayout.Button("Collect"))
            {
                List<string> collectedIds = new();

                List<EnemyCarPointConfig> collectedEnemyCarPointConfigs = FindObjectsOfType<EnemyCarPoint>().Select(value => new EnemyCarPointConfig(value.Id, value.StartPoint.position, value.StartPoint.rotation, value.EnemyType, value.Path)).ToList();
                collectedIds.AddRange(collectedEnemyCarPointConfigs.Select(value => value.Id));

                List<PatrolingEnemyPointConfig> collectedPatrolingEnemyPointConfigs = FindObjectsOfType<PatrolingEnemyPoint>().Where(value => collectedIds.Contains(value.Id) == false).Select(value => new PatrolingEnemyPointConfig(value.Id, value.StartPoint.position, value.StartPoint.rotation, value.EnemyType, value.Path)).ToList();
                collectedIds.AddRange(collectedPatrolingEnemyPointConfigs.Select(value => value.Id));

                List<StaticEnemyPointConfig> collectedStaticEnemyPoints = FindObjectsOfType<StaticEnemyPoint>().Where(value => collectedIds.Contains(value.Id) == false).Select(value => new StaticEnemyPointConfig(value.Id, value.StartPoint.position, value.StartPoint.rotation, value.EnemyType)).ToList();
                collectedIds.AddRange(collectedStaticEnemyPoints.Select(value => value.Id));

                Transform playerPoint = FindObjectOfType<PlayerPoint>().transform;

                levelConfig.EnemyCarPoints = collectedEnemyCarPointConfigs;
                levelConfig.PatrolingEnemyPoints = collectedPatrolingEnemyPointConfigs;
                levelConfig.StaticEnemyPoints = collectedStaticEnemyPoints;

                levelConfig.LevelKey = SceneManager.GetActiveScene().name;
            }

            if (LevelDatas.ContainsKey(levelConfig.LevelKey) == false)
                LevelDatas.Add(levelConfig.LevelKey, levelConfig);

            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(target);
        }
    }
}