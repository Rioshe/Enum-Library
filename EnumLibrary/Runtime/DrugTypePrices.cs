using System.Collections.Generic;
using UnityEngine;

namespace TC.EnumLibrary {
    //[CreateAssetMenu(fileName = "DrugTypePrices", menuName = "AssetLibrary/DrugTypePrices")]
    public class DrugTypePrices : ScriptableObject {
        Dictionary<DrugType, float> m_prices;
        public void Init() {
            m_prices = new Dictionary<DrugType, float> {
                { DrugType.Marijuana, 0 }, { DrugType.Cocaine, 0 },
                { DrugType.Heroin, 0 }, { DrugType.Meth, 0 },
                { DrugType.Lsd, 0 }, { DrugType.Ecstasy, 0 },
                { DrugType.Shrooms, 0 }, { DrugType.Pcp, 0 },
                { DrugType.Speed, 0 }, { DrugType.Crack, 0 },
                { DrugType.Opium, 0 }, { DrugType.Fentanyl, 0 },
                { DrugType.Ketamine, 0 }, { DrugType.BathSalts, 0 },
                { DrugType.Steroids, 0 }, { DrugType.Xanax, 0 },
                { DrugType.Adderall, 0 }, { DrugType.Ritalin, 0 },
                { DrugType.Morphine, 0 }, { DrugType.Codeine, 0 }
            };
        }
        public float GetPrice(DrugType drugType) => m_prices[drugType];
    }
}