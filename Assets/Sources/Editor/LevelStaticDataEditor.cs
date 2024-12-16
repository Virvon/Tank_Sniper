using Assets.Sources.Gameplay;
using Assets.Sources.Gameplay.Enemies.Points;
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

            LevelConfig levelData = (LevelConfig)target;

            if (GUILayout.Button("Collect"))
            {
                List<string> collectedIds = new();

                List<EnemyCarPointConfig> collectedEnemyCarPoints = FindObjectsOfType<EnemyCarPoint>().Select(value => new EnemyCarPointConfig(value.Id, value.EnemyType, value.Path, value.StartPoint, value.MaxRotationAngle, value.Speed)).ToList();
                collectedIds.AddRange(collectedEnemyCarPoints.Select(value => value.Id));

                List<HelicopterPointConfig> collectedHelicopterPoints = FindObjectsOfType<HelicopterPoint>().Where(value => collectedIds.Contains(value.Id) == false).Select(value => new HelicopterPointConfig(value.Id, value.EnemyType, value.Path, value.StartPoint, value.MaxRotationAngle, value.Speed)).ToList();
                collectedIds.AddRange(collectedEnemyCarPoints.Select(value => value.Id));

                Transform playerPoint = FindObjectOfType<PlayerPoint>().transform;

                levelData.EnemyCarPoints = collectedEnemyCarPoints;
                levelData.HelicopterPoints = collectedHelicopterPoints;

                levelData.LevelKey = SceneManager.GetActiveScene().name;
            }

            if (LevelDatas.ContainsKey(levelData.LevelKey) == false)
                LevelDatas.Add(levelData.LevelKey, levelData);

            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(target);
        }
    }
}