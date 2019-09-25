using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsCode.Models {
    /// <summary>
    /// Time Periods are used to group models together so that users have a better chance of finding waht they are looking for.
    /// </summary>
    [CreateAssetMenu(fileName = "Time Period", menuName = "3DTBD/Time Period", order = 1)]
    public class TimePeriod : ScriptableObject
    {
        [SerializeField]
        [Tooltip("The title of the time period is shown in the user interface.")]
        string title;

    }
}
