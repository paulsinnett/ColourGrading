using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateRandomObjects : MonoBehaviour
{
    public Transform prefab;
    public float range = 50.0f;
    public int number;

#if UNITY_EDITOR
    [ContextMenu("Generate Random Spheres")]
    void Generate()
    {
        for (int i = 0; i < number; ++i)
        {
            Transform instance =
                PrefabUtility.InstantiatePrefab(prefab) as Transform;

            instance.position =
                Vector3.Scale(
                    Random.insideUnitSphere + Vector3.up,
                    new Vector3(range, 4.0f, range));
            
            instance.parent = transform;

            Light light = instance.GetComponent<Light>();
            if (light != null)
            {
                Vector3 random =
                    (Random.onUnitSphere + Vector3.one) * 0.5f;

                light.color = new Color(random.x, random.y, random.z);
            }

            EditorUtility.SetDirty(instance);
        }
    }
#endif
}
