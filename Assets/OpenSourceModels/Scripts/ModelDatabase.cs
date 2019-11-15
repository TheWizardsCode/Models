using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using wizardscode.camera;

namespace WizardsCode.Assets
{
    /// <summary>
    /// The model database collects and manages information about all the models available in the system.
    /// </summary>
    public class ModelDatabase : MonoBehaviour
    {
        [Header("User Interfacr")]
        public Dropdown CategoryDropdown;
        public Dropdown ModelDropdown;

        Category[] categories;
        TimePeriod[] timePeriods;

        GameObject currentObject;
        GameObject lookAt;
        List<ModelMetaData> modelData = new List<ModelMetaData>();

        private void Awake()
        {
            categories = Resources.LoadAll<Category>("Categories");
            timePeriods = Resources.LoadAll<TimePeriod>("Time Periods");
        }

        private void Start()
        {
            ModelMetaData[] data = FindAllModels();
            foreach (ModelMetaData model in data)
            {
                modelData.Add(model);
            }

            PopulateCategoryList();
            PopulateModelList();
        }

        internal string ProjectFolder
        {
            get
            {
                string folder = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this));
                folder = folder.Substring(0, folder.IndexOf("/Scripts/"));
                return folder;
            }
        }

        /// <summary>
        /// Fill the category selection list with all known categories. This will allow the user
        /// to filter the list of objects by category.
        /// </summary>
        private void PopulateCategoryList()
        {
            CategoryDropdown.options.Clear();
            List<Dropdown.OptionData> categoryData = new List<Dropdown.OptionData>();
            foreach (Category category in categories)
            {
                categoryData.Add(new ModelMetaData.CategoryOptionData(category));
            }

            CategoryDropdown.AddOptions(categoryData);
        }

        /// <summary>
        /// Populate the model list with all the models in the selected category.
        /// </summary>
        public void PopulateModelList()
        {
            ModelDropdown.options.Clear();
            List<Dropdown.OptionData> items = new List<Dropdown.OptionData>();
            foreach (ModelMetaData data in modelData)
            {
                int idx = CategoryDropdown.value;
                if (data.category == ((ModelMetaData.CategoryOptionData)CategoryDropdown.options[idx]).m_category)
                {
                    items.Add(new ModelMetaData.MetaDataOptionData(data));
                }
            }

            ModelDropdown.AddOptions(items);

            SpawnObject(0);
        }

        public void SpawnObject(int index)
        {
            if (ModelDropdown.options.Count == 0)
            {
                return;
            }

            if (currentObject != null)
            {
                DestroyImmediate(currentObject);
            }

            GameObject prefab = ((ModelMetaData.MetaDataOptionData)ModelDropdown.options[ModelDropdown.value]).data.asset as GameObject;
            currentObject = Instantiate(prefab);

            PositionCamera();

            EditorGUIUtility.PingObject(prefab);
        }

        private void PositionCamera()
        {
            Camera camera = Camera.main;
            camera.transform.position = new Vector3(0, 1.8f, 0);

            Bounds bounds = new Bounds();
            Renderer[] renderers = currentObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            Vector3 center = bounds.center;

            float r = Math.Max(Math.Max(bounds.extents.x, bounds.extents.z), bounds.extents.y);
            float v = Camera.main.fieldOfView;
            float h = Camera.main.fieldOfView;

            float z = (float)Math.Max(r / Math.Sin(v), r / Math.Sin(h)); // = r / Math.Sin(Math.Min(v, h));

            Vector3 pos = camera.transform.position;
            pos.z = Math.Abs(z);
            //pos.y = Math.Abs(z);

            camera.transform.position = pos;

            if (lookAt != null)
            {
                DestroyImmediate(lookAt);
            }
            lookAt = new GameObject("Look At Me");
            lookAt.transform.position = center;
            FlyCameraController controller = camera.GetComponent<FlyCameraController>();
            controller.LookAt = currentObject;
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
                    data.asset = prefab;
                    data.name = prefab.name;
                    AssetDatabase.CreateAsset(data, dataPath);
                }

                metaData.Add(data);
            }

            AssetDatabase.SaveAssets();

            return metaData.ToArray();
        }

        private void OnDestroy()
        {
            DestroyImmediate(lookAt);
            DestroyImmediate(currentObject);
        }
    }
}
