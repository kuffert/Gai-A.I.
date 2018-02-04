using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InceptorNode : Node
{
    void Awake()
    {
        calculateNodeOrderMapping();
    }

    void Start()
    {
        GameObject inceptorNodePrefab = nodePrefabsData.inceptorNodePrefab;
        nodeRender = Instantiate(inceptorNodePrefab, transform.position, transform.rotation);
        nodeRender.transform.parent = gameObject.transform;
        generateNodeColliderAndSetCollisionLayer();
        generateNeighbors();
    }

    void Update()
    {
        switch (GAIASimulationManager.getSimState())
        {
            case GAIASimulationManager.SimState.EnvGen:
                findNeighbors();
                break;
            case GAIASimulationManager.SimState.GAIAControl:
                updateCurrentLifeLevel(currentLifeDelta);
                break;
        }
    }

    public static GameObject generateInceptorNode(float initialLifeRes, float initialLifeThreshold, int maxFractals, bool validationMode, Vector3 transformPosition, NodePrefabs nodePrefabsData)
    {
        GameObject newInceptorNodeObj = new GameObject("Inceptor Node");
        newInceptorNodeObj.transform.position = transformPosition;

        InceptorNode inceptorNodeScript = newInceptorNodeObj.AddComponent<InceptorNode>();
        inceptorNodeScript.incrementTotalNodes();
        inceptorNodeScript.lifeResistance = initialLifeRes;
        inceptorNodeScript.lifeThreshhold = initialLifeThreshold;
        inceptorNodeScript.nodePrefabsData = nodePrefabsData;
        inceptorNodeScript.setMaxFractals(maxFractals);
        inceptorNodeScript.setValidationMode(validationMode);

        ALLNODEOBJECTS.Add(newInceptorNodeObj);
        return newInceptorNodeObj;
    }

    override protected void generateNeighbors()
    {
        NorthNode = StandardNode.generateStandardNode(gameObject, lifeResistance, lifeThreshhold, NodeDirection.South, transform.position + new Vector3(NODESIZE, 0, 0), nodePrefabsData);
        EastNode = StandardNode.generateStandardNode(gameObject, lifeResistance, lifeThreshhold, NodeDirection.West, transform.position + new Vector3(0, 0, -NODESIZE), nodePrefabsData);
        SouthNode = StandardNode.generateStandardNode(gameObject, lifeResistance, lifeThreshhold, NodeDirection.North, transform.position + new Vector3(-NODESIZE, 0, 0), nodePrefabsData);
        WestNode = StandardNode.generateStandardNode(gameObject, lifeResistance, lifeThreshhold, NodeDirection.East, transform.position + new Vector3(0, 0, NODESIZE), nodePrefabsData);
    }

    private void calculateNodeOrderMapping()
    {
        List<int[]> pairs = new List<int[]> { new int[] { 1, 1 }, new int[] { 6, 2 }, new int[] { 11, 3 }, new int[] { 16, 4 } };
        int[] nVal = pairs[Random.Range(0, pairs.Count)];
        pairs.Remove(nVal);
        int[] eVal = pairs[Random.Range(0, pairs.Count)];
        pairs.Remove(eVal);
        int[] sVal = pairs[Random.Range(0, pairs.Count)];
        pairs.Remove(sVal);
        int[] wVal = pairs[Random.Range(0, pairs.Count)];
        nodeOrderMapping.Add(NodeDirection.North, nVal);
        nodeOrderMapping.Add(NodeDirection.East, eVal);
        nodeOrderMapping.Add(NodeDirection.South, sVal);
        nodeOrderMapping.Add(NodeDirection.West, wVal);
        Soliloquy.envLog("Inceptor time mapping: ");
        Soliloquy.envLog("N:" + nVal[1] + " " + "E:" + eVal[1] + " " + "S:" + sVal[1] + " " + "W:" + wVal[1]);
    }
}
