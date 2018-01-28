using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InceptorNode : Node
{
    GameObject inceptorNodePrefab = nodePrefabs.inceptorNodePrefab;
    public InceptorNode(int initialLifeResistance, int initialLifeThreshold)
    {
        incrementTotalNodes();
        this.lifeResistance = initialLifeResistance;
        this.lifeThreshhold = initialLifeThreshold;
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log(inceptorNodePrefab);
        nodeRender = Instantiate(inceptorNodePrefab, transform.position, transform.rotation);
        generateNeighbors();
    }

    // Update is called once per frame
    void Update()
    {

    }

    override protected Node generateStandardNode(int prevNodeLifeRes, int prevNodeLifeThresh, NodeDirection prevNodeDir, Vector3 transformPosition)
    {
        throw new UnityException("Standard node generator unsupported by Inceptor Node");
    }
    override protected Node generateBoundaryNode(Vector3 transformPosition)
    {
        throw new UnityException("Standard node generator unsupported by Inceptor Node");
    }

    override protected void generateNeighbors()
    {
        NorthNode = generateStandardNode(lifeResistance, lifeThreshhold, NodeDirection.South, transform.position + new Vector3(3, 0, 0));
        EastNode = generateStandardNode(lifeResistance, lifeThreshhold, NodeDirection.West, transform.position + new Vector3(0, 0, 3));
        SouthNode = generateStandardNode(lifeResistance, lifeThreshhold, NodeDirection.North, transform.position + new Vector3(-3, 0, 0));
        WestNode = generateStandardNode(lifeResistance, lifeThreshhold, NodeDirection.East, transform.position + new Vector3(0, 0, -3));
 
    }
}
