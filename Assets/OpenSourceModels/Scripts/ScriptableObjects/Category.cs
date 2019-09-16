using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.Models {
    /// <summary>
    /// Categories are used to group models together so that users have a better chance of finding waht they are looking for.
    /// </summary>
    [CreateAssetMenu(fileName = "Category", menuName = "3DTBD/Category", order = 1)]
    public class Category : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The title of the category is shown in the user interface.")]
        string title;

        [SerializeField]
        [Tooltip("The description of the category is used to provide more detail about what users can expect to find in this category.")]
        string description;
    }
}
