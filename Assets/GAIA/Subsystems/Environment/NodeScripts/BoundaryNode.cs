using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryNode : Node {

	GameObject boundaryNodePrefab = nodePrefabs.boundaryNodePrefab;

	private BoundaryNode(Vector3 transformPostion)
	{

		nodeRender = Instantiate(boundaryNodePrefab, transform.position, transform.rotation);
	}

	// Use this for initialization
	void Start () 
	{
		nodeRender = Instantiate(boundaryNodePrefab, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	override protected Node generateStandardNode(int prevNodeLifeRes, int prevNodeLifeThresh, NodeDirection prevNodeDir, Vector3 transformPosition)
    {
		throw new UnityException("Boundary Nodes cannot generate a Standard Node.");
    }

	override protected Node generateBoundaryNode(Vector3 transformPosition)
    {
		return new BoundaryNode(transformPosition);
    }

	 override protected void generateNeighbors()
    {
		throw new UnityException("Boundary Nodes do not support generation of neighbor Nodes.");
    }
}
