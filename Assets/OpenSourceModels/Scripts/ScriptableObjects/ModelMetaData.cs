using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WizardsCode.Models
{
    /// <summary>
    /// The ModelMetaData captures information about an object in the database.
    /// The Models application uses this data to make it easier to find the model you want.
    /// </summary>
    [CreateAssetMenu(fileName = "Model Meta Data", menuName = "3DTBD/Model Meta Data")]
    public class ModelMetaData : ScriptableObject
    {
        public GameObject m_prefab;

        [Header("New Categorization (WIP)")]
        [SerializeField]
        internal Category category;
        [SerializeField]
        internal TimePeriod timePeriod;


        public class MetaDataOptionData : Dropdown.OptionData
        {
            internal ModelMetaData data;

            public MetaDataOptionData(ModelMetaData data)
            {
                this.data = data;
                text = data.name;
            }
        }

        public class CategoryOptionData : Dropdown.OptionData
        {
            internal Category m_category;

            public CategoryOptionData(Category category)
            {
                this.m_category = category;
                text = category.ToString();
            }
        }
    }
}
