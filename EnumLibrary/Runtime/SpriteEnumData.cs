using System.Collections.Generic;
using UnityEngine;

namespace TC.EnumLibrary {
    [CreateAssetMenu(fileName = "SpriteEnumData", menuName = "ScriptableObjects/SpriteEnumData", order = 1)]
    public class SpriteEnumData : ScriptableObject {
        [SerializeField]
        List<Sprite> m_spriteList;

        public Sprite GetSprite(int spriteEnum) {
            int index = spriteEnum - 1; // Adjust for 'None'
            if (index >= 0 && index < m_spriteList.Count) {
                return m_spriteList[index];
            }

            Debug.LogWarning("Sprite not found for enum value: " + spriteEnum);
            return null;
        }

        public List<Sprite> GetSpriteList() {
            return m_spriteList;
        }
    }
}