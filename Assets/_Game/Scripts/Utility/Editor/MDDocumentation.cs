using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility.Editor
{
    public class MDDocumentation : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField, TextArea(15, 15)]
        [Tooltip("Supports levels 1, 2, and 3 markdown headers along with plain text. Click the button below to render it!")]
        internal string description = string.Empty;
        #endif
    }
}
