using Assets.Sources.Gameplay.Destructions.Building;
using UnityEditor;
using UnityEngine;

namespace Assets.Sources.Editor
{
    [CustomEditor(typeof(Building))]
    public class BuildingEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var building = (Building)target;

            if (GUILayout.Button("Collect"))
            {
                building.Collect();
            }

            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(target);
        }
    }
}