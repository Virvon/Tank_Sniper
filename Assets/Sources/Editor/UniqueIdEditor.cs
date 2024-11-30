﻿using Assets.Sources.Gameplay.Enemies.Points;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Sources.Editor
{
    internal class UniqueIdEditor<TType> : UnityEditor.Editor
        where TType : EnemyPoint
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
                EnemyPoint[] uniqueIds = FindObjectsOfType<EnemyPoint>();

                if (uniqueIds.Any(other => other != enemyPoint && other.Id == enemyPoint.Id))
                    Generate(enemyPoint);
            }
        }

        private void Generate(EnemyPoint uniqueId)
        {
            uniqueId.Id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid()}";

            if (Application.isPlaying == false)
            {
                EditorUtility.SetDirty(uniqueId);
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        } 
    }

    [CustomEditor(typeof(EnemyPoint))]
    internal class UniqueIdEnemyPointEditor : UniqueIdEditor<EnemyPoint>
    {
    }

    [CustomEditor(typeof(WalkingEnemyPoint))]
    internal class UniqueIdWalkingEnemyPointEditor : UniqueIdEditor<WalkingEnemyPoint>
    {
    }

    [CustomEditor(typeof(EnemyCarPoint))]
    internal class UniqueIdEnemyCarPointEditor : UniqueIdEditor<EnemyCarPoint>
    {
    }
}