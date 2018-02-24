using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DullNode : Node
{

    void Start()
    {
        GameObject dullNodePrefab = nodePrefabsData.getRandomDullNode();
        nodeRender = Instantiate(dullNodePrefab, transform.position, transform.rotation);
        nodeRender.transform.parent = gameObject.transform;
        setPrevNodeNeighbor(prevNode, prevNodeDir);
        generateNodeColliderAndSetCollisionLayer();
        findNeighbors();
        incrementTotalNodes();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GAIASimulationManager.getSimState())
        {
            case GAIASimulationManager.SimState.EnvGen:
                findNeighbors();
                break;

            case GAIASimulationManager.SimState.WaterGen:
                findNeighbors();
                break;
        }
    }

    // Need to know spawener
    public static GameObject generateDullNode(Node prevNode, NodeDirection prevNodeDir, Vector3 transformPosition, NodePrefabs nodePrefabsData)
    {
        GameObject newDullNodeObject = new GameObject("DullNode");
        newDullNodeObject.transform.position = transformPosition;

        DullNode dullNodeScript = newDullNodeObject.AddComponent<DullNode>();
        dullNodeScript.nodePrefabsData = nodePrefabsData;
        dullNodeScript.nodeName = "Dull Node";
        dullNodeScript.prevNode = prevNode;
        dullNodeScript.prevNodeDir = prevNodeDir;
        dullNodeScript.lifeResistance = 100f;
        dullNodeScript.lifeThreshhold = 100f;

        ALLNODEOBJECTS.Add(newDullNodeObject);
        ALLNODESCRIPTS.Add(dullNodeScript);
        return newDullNodeObject;
    }

    override protected void generateNeighbors()
    {
        throw new UnityException("Boundary Nodes do not support generation of neighbor Nodes.");
    }

    override public void generateWater(int tickInterval, int fractalID) { }
    override protected void disperseLifeToNeighbors() { }
    override protected void updateCurrentLifeLevel(float change) { }
    override protected void updateCurrentLifeState(LifeState newLifeState) { }
    
}
