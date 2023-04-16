using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class SpawnMany : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    [SerializeField] private Vector2Int dimensions;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    [ContextMenu("Spawn things")]
    private void Spawn()
    {
        var middlepos = gameObject.transform.position;
        var localscale = gameObject.transform.localScale;
        for (int x = -dimensions.x; x < dimensions.x; x++)
        {
            for (int y = -dimensions.y; y < dimensions.y; y++)
            {
                Instantiate(prefab, middlepos + new Vector3(x * 10 * localscale.x, 0, y * 10 * localscale.z), Quaternion.identity,transform);
                
            }
        }
    }
}
