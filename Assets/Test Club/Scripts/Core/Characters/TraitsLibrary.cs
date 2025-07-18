using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "TraitsLibrary", menuName = "Scriptable Objects/Traits Library")]
    public class TraitsLibrary : ScriptableObject
    {
        public List<Trait> AppearanceTraits;
        public List<Trait> PersonalityTraits;

        public Trait GetRandomTrait(TraitCategory category)
        {
            var list = category == TraitCategory.Appearance ? AppearanceTraits : PersonalityTraits;
            return list[Random.Range(0, list.Count)];
        }
    }
}
