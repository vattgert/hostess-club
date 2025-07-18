using UnityEngine;

namespace Characters
{
    public enum TraitCategory
    {
        Appearance,
        Personality
    }

    [CreateAssetMenu(fileName = "Trait", menuName = "Scriptable Objects/Trait")]
    public class Trait : ScriptableObject
    {
        [field: SerializeField] public TraitCategory Category { get; private set; }

        [field: SerializeField] public string Name { get; private set;  }
    }
}

