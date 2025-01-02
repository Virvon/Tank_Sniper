using Assets.Sources.Gameplay.Enemies.Points;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Sources.Editor
{
    internal class UniqueIdEditor<TType> : UnityEditor.Editor
        where TType : StaticEnemyPoint
    {
        private void OnEnable()
        {
            TType enemyPoint = (TType)target;

            if (string.IsNullOrEmpty(enemyPoint.Id))
            {
                Generate(enemyPoint);
            }
            else
            {
                StaticEnemyPoint[] uniqueIds = FindObjectsOfType<StaticEnemyPoint>();

                if (uniqueIds.Any(other => other != enemyPoint && other.Id == enemyPoint.Id))
                    Generate(enemyPoint);
            }
        }

        private void Generate(StaticEnemyPoint uniqueId)
        {
            uniqueId.Id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid()}";

            if (Application.isPlaying == false)
            {
                EditorUtility.SetDirty(uniqueId);
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        } 
    }

    [CustomEditor(typeof(StaticEnemyPoint))]
    internal class UniqueIdEnemyPointEditor : UniqueIdEditor<StaticEnemyPoint>
    {
    }

    [CustomEditor(typeof(PatrolingEnemyPoint))]
    internal class UniqueIdWalkingEnemyPointEditor : UniqueIdEditor<PatrolingEnemyPoint>
    {
    }

    [CustomEditor(typeof(EnemyCarPoint))]
    internal class UniqueIdEnemyCarPointEditor : UniqueIdEditor<EnemyCarPoint>
    {
    }
    [CustomEditor(typeof(HelicopterPoint))]
    internal class UniqueIdHelicopterPointEditor : UniqueIdEditor<HelicopterPoint>
    {
    }
}