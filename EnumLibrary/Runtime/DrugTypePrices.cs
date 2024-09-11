using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TC.EnumLibrary {
    public class DrugTypePrices : ScriptableObject {
        public float m_defaultPrice = 10.00f;
        Dictionary<DrugType, float> m_prices;

        public void Init() {
            m_prices = new Dictionary<DrugType, float> {
                { DrugType.Marijuana, m_defaultPrice }, { DrugType.Cocaine, m_defaultPrice },
                { DrugType.Heroin, m_defaultPrice }, { DrugType.Meth, m_defaultPrice },
                { DrugType.Lsd, m_defaultPrice }, { DrugType.Ecstasy, m_defaultPrice },
                { DrugType.Shrooms, m_defaultPrice }, { DrugType.Pcp, m_defaultPrice },
                { DrugType.Speed, m_defaultPrice }, { DrugType.Crack, m_defaultPrice },
                { DrugType.Opium, m_defaultPrice }, { DrugType.Fentanyl, m_defaultPrice },
                { DrugType.Ketamine, m_defaultPrice }, { DrugType.BathSalts, m_defaultPrice },
                { DrugType.Steroids, m_defaultPrice }, { DrugType.Xanax, m_defaultPrice },
                { DrugType.Adderall, m_defaultPrice }, { DrugType.Ritalin, m_defaultPrice },
                { DrugType.Morphine, m_defaultPrice }, { DrugType.Codeine, m_defaultPrice }
            };
        }
        public float GetPrice(DrugType drugType) => m_prices[drugType];
    }
}