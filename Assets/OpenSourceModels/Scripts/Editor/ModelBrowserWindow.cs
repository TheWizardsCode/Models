using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode.Models
{
    public class ModelBrowserWindow : EditorWindow
    {
        private Category[] categories;
        bool[] toggles;
        ModelMetaData[] modelData;
        GameObject gameObject;
        Editor gameObjectEditor;
        private static EditorWindow window;

        [MenuItem("Tools/3D TBD/Model Browser")]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow(typeof(ModelBrowserWindow));
            GUIContent titleContent = new GUIContent("Open Source Models");
            window.titleContent = titleContent;
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("Filter Settings", EditorStyles.boldLabel);
            for (int i = 0; i < categories.Length; i++) {
                toggles[i] = EditorGUILayout.Toggle(categories[i].title, toggles[i]);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Available Models", EditorStyles.boldLabel);
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i])
                {
                    ModelMetaData[] models = Array.FindAll(modelData, row => row.category == categories[i]);
                    for (int idx = 0; idx < models.Length; idx++)
                    {
                        if (GUILayout.Button(models[idx].m_prefab.name))
                        {
                            gameObject = models[idx].m_prefab;
                            EditorGUIUtility.PingObject(gameObject);
                        }
                    }
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            if (gameObject != null || GameObject.ReferenceEquals(gameObject, gameObjectEditor.target))
            {
                if (gameObjectEditor == null || !GameObject.ReferenceEquals(gameObject, gameObjectEditor.target))
                {
                    gameObjectEditor = Editor.CreateEditor(gameObject);
                }

                gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void OnEnable()
        {
            categories = Resources.LoadAll<Category>("Categories");
            toggles = new bool[categories.Length];

            modelData = FindAllModels();
        }

        internal string ProjectFolder
        {
            get
            {
                string folder = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
                folder = folder.Substring(0, folder.IndexOf("/Scripts/"));
                return folder;
            }
        }

        internal ModelMetaData[] FindAllModels()
        {
            List<ModelMetaData> metaData = new List<ModelMetaData>();

            string[] prefabGUIDs = AssetDatabase.FindAssets("t:prefab", new[] { ProjectFolder });
            foreach (string guid in prefabGUIDs)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                string dataPath = prefabPath.Replace(".prefab", ".asset");
                ModelMetaData data = AssetDatabase.LoadAssetAtPath<ModelMetaData>(dataPath);

                if (data == null)
                {
                    data = ScriptableObject.CreateInstance<ModelMetaData>();
                    data.m_prefab = prefab;
                    data.name = prefab.name;
                    AssetDatabase.CreateAsset(data, dataPath);
                }

                metaData.Add(data);
            }

            AssetDatabase.SaveAssets();

            return metaData.ToArray();
        }
    }
}