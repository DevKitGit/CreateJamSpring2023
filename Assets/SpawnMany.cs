using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class SpawnMany : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    [SerializeField] private Vector2Int dimensions;
    [SerializeField] private float scale;

    
    // Update is called once per frame
    [ContextMenu("Spawn things")]
    private void Spawn()
    {
        var transforms = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        for (var i = 0; i < transforms.Length; i++)
        {
            DestroyImmediate(transforms[i].gameObject);
        }
        var middlepos = gameObject.transform.position;
        var localscale = new Vector3(scale, 1f, scale);
        for (int x = -dimensions.x; x < dimensions.x; x++)
        {
            for (int y = -dimensions.y; y < dimensions.y; y++)
            {
                var go =Instantiate(prefab, middlepos + new Vector3(x * 10 * localscale.x, 0, y * 10 * localscale.z), Quaternion.identity,transform);
                go.transform.localScale = localscale;
            }
        }
    }
}
