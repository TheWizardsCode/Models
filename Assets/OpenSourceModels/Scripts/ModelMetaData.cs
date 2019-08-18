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
    [CreateAssetMenu(fileName = "Model Meta Data", menuName = "Wizards Code/Meta Data/Model")]
    public class ModelMetaData : ScriptableObject
    {
        public enum Category { Uncategorized, Other, Building, Prop, Rock, Human, Tree };
        public enum TimePeriod { Untimed, Other, Medieval, Present };

        public Category m_category = Category.Uncategorized;
        public TimePeriod m_timePeriod = TimePeriod.Untimed;

        public GameObject m_prefab;

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
