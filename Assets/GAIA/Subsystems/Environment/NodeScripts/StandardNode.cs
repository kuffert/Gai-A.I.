using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardNode : Node
{
    GameObject standardNodePrefab = nodePrefabs.standardNodePrefab;

    private StandardNode(int prevNodeLifeRes, int prevNodeLifeThresh, NodeDirection prevNodeDir)
    {
        incrementTotalNodes();
        this.lifeResistance = generatePsuedoRandomLifeRes(prevNodeLifeRes);
        this.lifeThreshhold = generatePsuedoRandomLifeThresh(prevNodeLifeThresh);
    }

    // Use this for initialization
    void Start()
    {
        nodeRender = Instantiate(standardNodePrefab, transform.position, transform.rotation);
		generateNeighbors();
    }

    // Update is called once per frame
    void Update()
    {

    }

    override protected Node generateStandardNode(int prevNodeLifeRes, int prevNodeLifeThresh, NodeDirection prevNodeDir, Vector3 transformPosition)
    {
        return new StandardNode(prevNodeLifeRes, prevNodeLifeThresh, prevNodeDir);
    }

    override protected Node generateBoundaryNode(Vector3 transformPosition)
    {
        throw new UnityException("Standard Nodes do not support generation of Boundary Nodes.");
    }

    override protected void generateNeighbors()
    {
        NorthNode = generateBoundaryNode(transform.position + new Vector3(3, 0, 0));
        EastNode = generateBoundaryNode(transform.position + new Vector3(0, 0, 3));
        SouthNode = generateBoundaryNode(transform.position + new Vector3(-3, 0, 0));
        WestNode = generateBoundaryNode(transform.position + new Vector3(0, 0, -3));
    }

    private int generatePsuedoRandomLifeRes(int prevLR)
    {
        return 0;
    }

    private int generatePsuedoRandomLifeThresh(int prevLT)
    {
        return 0;
    }
}
