using System.Collections.Generic;
using UnityEngine;
namespace TC.EnumLibrary {
    public class SpriteLibrary : ScriptableObject {
        public List<string> m_spriteCategories = new();
        
        //public List<SpriteFileObject> m_sprites = new();
        
        [Tooltip("Allows you to customize the enum and namespace names for your generated audio. For advanced users.")]
        public bool m_useCustomNames = false;
        
        public string SafeName => name.ConvertToAlphanumeric();
        public string m_generatedName;
        public string m_spriteEnum;
        public string DefaultSpriteEnum => name.ConvertToAlphanumeric();
        public string m_spriteEnumGenerated;
        public string m_spriteNamespace;
        public string m_spriteNamespaceGenerated;
        
        //[SerializeField] public List<CategoryToList> m_spriteCategoriesToList = new();

        void Reset() {
            m_spriteCategories.Add(string.Empty);
            
            //var ctl = new CategoryToList();
            //ctl.m_name = string.Empty;
            //ctl.m_foldout = true;
            //m_spriteCategoriesToList.Add(ctl);
        }

        public void InitializeValues() {
            m_spriteEnum = DefaultSpriteEnum;
        }
    }
}