using Assets.Sources.Gameplay.Enemies;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Assets.Sources.Editor
{
    [CustomEditor(typeof(EnemyPoint))]
    internal class UniqueIdEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            EnemyPoint uniqueId = (EnemyPoint)target;

            if (string.IsNullOrEmpty(uniqueId.Id))
            {
                Generate(uniqueId);
            }
            else
            {
                EnemyPoint[] uniqueIds = FindObjectsOfType<EnemyPoint>();

                if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
                    Generate(uniqueId);
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
}