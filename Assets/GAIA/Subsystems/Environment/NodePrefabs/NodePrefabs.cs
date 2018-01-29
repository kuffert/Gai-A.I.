using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NodePrefabs", menuName = "GAIA Prefabs", order = 1)]
public class NodePrefabs : ScriptableObject
{
    public GameObject inceptorNodePrefab;
    public GameObject boundaryNodePrefab;
    public GameObject standardNodePrefab;
    public GameObject validatedNodePrefab;
    public GameObject dullNodePrefabV1;
    public GameObject dullNodePrefabV2;

    public GameObject getRandomDullNode()
    {
        GameObject[] dulls = new GameObject[] {dullNodePrefabV1, dullNodePrefabV2};
        return dulls[Random.Range(0, dulls.Length)];
    }
}
