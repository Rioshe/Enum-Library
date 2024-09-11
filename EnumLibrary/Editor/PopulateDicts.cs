using System;
using System.Collections.Generic;
using UnityEngine;
namespace TC.EnumLibrary {
    /*public class PopulateDicts {
        void PopulateEnumDict<T>(Dictionary<Enum, T> items)
        {
            if (ExistingEnum == null)
            {
                Debug.LogWarning("An existing enum must be selected to populate the dictionary.");
                return;
            }

            IItemPopulator<T> populator = PopulatorFactory.GetPopulator<T>();
            populator.Populate(items, ExistingEnum);
        }
        public Enum ExistingEnum { get; set; }
    }*/

    public static class PopulatorFactory {
        static readonly Dictionary<Type, object> PopulatorMap = new() {
            //value types
            { typeof(float), new FloatPopulator() },
            { typeof(int), new IntPopulator() },
            { typeof(Vector2), new Vector2Populator() },
            { typeof(Vector3), new Vector3Populator() },
            
            //object types
            { typeof(Sprite), new SpritePopulator() },
            { typeof(TextAsset), new TextAssetPopulator() },
            { typeof(AudioClip), new AudioClipPopulator() },
            { typeof(GameObject), new GameObjectPopulator() },
            { typeof(Color), new ColorPopulator() },
            { typeof(Material), new MaterialPopulator() }
        };

        public static IItemPopulator<T> GetPopulator<T>() {
            if (PopulatorMap.TryGetValue(typeof(T), out object populator)) {
                return (IItemPopulator<T>)populator;
            }

            throw new InvalidOperationException($"No populator available for type {typeof(T)}");
        }
    }

    public interface IItemPopulator<T> {
        void Populate(Dictionary<Enum, T> items, Enum existingEnum);
    }

    public abstract class ItemPopulator<T> : IItemPopulator<T> {
        public virtual void Populate(Dictionary<Enum, T> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, T>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                items[value] = default; // Value types like float, int, etc. will default to 0
            }

            SystemLogging.Log($"{typeof(T).Name} populated with zero or default values for the selected enum.");
        }
    }

    public class FloatPopulator : ItemPopulator<float> {
        public override void Populate(Dictionary<Enum, float> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, float>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                if (items.TryGetValue(value, out float item)) {
                    items[value] = Convert.ToSingle(item);
                }
                else {
                    items[value] = 0;
                }
            }

            SystemLogging.Log("FloatValues populated with float values for the selected enum.");
        }
    }

    public class IntPopulator : ItemPopulator<int> {
        public override void Populate(Dictionary<Enum, int> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, int>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                if (items.TryGetValue(value, out int item)) {
                    items[value] = Convert.ToInt32(item);
                }
                else {
                    items[value] = 0;
                }
            }

            SystemLogging.Log("FloatValues populated with int values for the selected enum.");
        }
    }

    //vector2, vector3, quaternion, color, etc.
    public class Vector2Populator : ItemPopulator<Vector2> {
        public override void Populate(Dictionary<Enum, Vector2> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, Vector2>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                if (items.TryGetValue(value, out var item)) {
                    items[value] = item;
                }
                else {
                    items[value] = Vector2.zero;
                }
            }

            SystemLogging.Log("FloatValues populated with Vector2 values for the selected enum.");
        }
    }

    public class Vector3Populator : ItemPopulator<Vector3> {
        public override void Populate(Dictionary<Enum, Vector3> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, Vector3>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                if (items.TryGetValue(value, out var item)) {
                    items[value] = item;
                }
                else {
                    items[value] = Vector3.zero;
                }
            }

            SystemLogging.Log("FloatValues populated with Vector3 values for the selected enum.");
        }
    }

    public class ColorPopulator : ItemPopulator<Color> {
        public override void Populate(Dictionary<Enum, Color> items, Enum existingEnum) {
            items ??= new Dictionary<Enum, Color>();
            foreach (Enum value in Enum.GetValues(existingEnum.GetType())) {
                if (items.TryGetValue(value, out var item)) {
                    items[value] = item;
                }
                else {
                    items[value] = Color.clear;
                }
            }

            SystemLogging.Log("FloatValues populated with Color values for the selected enum.");
        }
    }


    public class SpritePopulator : ItemPopulator<Sprite> { }
    public class TextAssetPopulator : ItemPopulator<TextAsset> { }
    public class AudioClipPopulator : ItemPopulator<AudioClip> { }
    public class GameObjectPopulator : ItemPopulator<GameObject> { }
    public class MaterialPopulator : ItemPopulator<Material> { }
}