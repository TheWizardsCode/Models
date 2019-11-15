using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode.Assets
{
    public class ModelBrowserWindow : AssetBrowserWindow<GameObject>
    {

        [MenuItem("Tools/3D TBD/Model Browser")]
        public static void ShowWindow()
        {
            window = EditorWindow.GetWindow(typeof(ModelBrowserWindow));
            GUIContent titleContent = new GUIContent("Open Source Models");
            window.titleContent = titleContent;
        }

        
    }
}