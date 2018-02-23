using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryNode : Node
{
    void Start()
    {
        GameObject boundaryNodePrefab = nodePrefabsData.boundaryNodePrefab;
        nodeRender = Instantiate(boundaryNodePrefab, transform.position, transform.rotation);
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
    public static GameObject generateBoundaryNode(Node prevNode, NodeDirection prevNodeDir, Vector3 transformPosition, NodePrefabs nodePrefabsData)
    {
        GameObject newBoundaryNodeObj = new GameObject("BoundaryNode");
        newBoundaryNodeObj.transform.position = transformPosition;

        BoundaryNode boundaryNodeScript = newBoundaryNodeObj.AddComponent<BoundaryNode>();
        boundaryNodeScript.nodePrefabsData = nodePrefabsData;
        boundaryNodeScript.prevNode = prevNode;
        boundaryNodeScript.prevNodeDir = prevNodeDir;
        boundaryNodeScript.lifeResistance = 100f;
        boundaryNodeScript.lifeThreshhold = 100f;

        ALLNODEOBJECTS.Add(newBoundaryNodeObj);
        ALLNODESCRIPTS.Add(boundaryNodeScript);
        return newBoundaryNodeObj;
    }

    override protected void generateNeighbors()
    {
        throw new UnityException("Boundary Nodes do not support generation of neighbor Nodes.");
    }

    override public void generateWater(int tickInterval, int fractalID)
    {

    }

    override protected void disperseLifeToNeighbors()
    {
        
    }
}
