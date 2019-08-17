using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode.Models
{
    [CustomEditor(typeof(ModelMetaData))]
    public class ModelMetaDataEditor : Editor
    {
        SerializedProperty prefab;
        SerializedProperty category;
        SerializedProperty timePeriod;

        string prefabFieldName = "m_prefab";
        string categoryFieldName = "m_category";
        string timePeriodFieldName = "m_timePeriod";

        private void OnEnable()
        {
            prefab = serializedObject.FindProperty(prefabFieldName);
            category = serializedObject.FindProperty(categoryFieldName);
            timePeriod = serializedObject.FindProperty(timePeriodFieldName);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(prefab);
            EditorGUILayout.PropertyField(category);
            EditorGUILayout.PropertyField(timePeriod);

            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}