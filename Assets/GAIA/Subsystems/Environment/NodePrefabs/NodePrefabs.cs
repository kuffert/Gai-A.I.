using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="NodePrefabs", menuName = "GAIA Prefabs", order = 1)]
public class NodePrefabs : ScriptableObject
{
    //Node prefabs
    public GameObject inceptorNodePrefab;
    public GameObject boundaryNodePrefab;
    public GameObject standardNodePrefab;
    public GameObject validatedNodePrefab;
    public GameObject dullNodePrefabV1;
    public GameObject dullNodePrefabV2;
    public GameObject deadNodePrefab;
    public GameObject stage1NodePrefab;
    public GameObject stage2NodePrefab;
    public GameObject stage3NodePrefab;
    public GameObject flourishingNodePrfab;

// Particle effect prefabs
    public GameObject changeLifeLevelEffectPrefab;

    public GameObject getRandomDullNode()
    {
        GameObject[] dulls = new GameObject[] {dullNodePrefabV1, dullNodePrefabV2};
        return dulls[Random.Range(0, dulls.Length)];
    }

    public void playChangeLifeLevelEffect(Transform transform)
    {
        GameObject instance = Instantiate(changeLifeLevelEffectPrefab, transform.position, transform.rotation);
        ParticleSystem ps = instance.GetComponent<ParticleSystem>();
        ps.Play();
        Destroy(instance, ps.main.duration);
        
    }
}
