using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public enum NodeDirection { North, East, South, West };

    /// Something to set prefab

    public NodePrefabs nodePrefabs; 
    public int lifeResistance;
    public int lifeThreshhold;
    public int currentLifeLevel;

    protected Node NorthNode = null;
    protected Node EastNode = null;
    protected Node SouthNode = null;
    protected Node WestNode = null;
    protected GameObject nodeRender;

    private int maxLifeLevel;
    private static int TOTALNODES = 0;

    protected void incrementTotalNodes()
    {
        TOTALNODES++;
    }

    protected abstract Node generateStandardNode(int prevNodeLifeRes, int prevNodeLifeThresh, NodeDirection prevNodeDir, Vector3 transformPosition);
    protected abstract Node generateBoundaryNode(Vector3 transformPosition);
    protected abstract void generateNeighbors();

    protected bool canGenerateNeighbors()
    {
        return TOTALNODES < 101;
    }

    protected int getTotalNodes()
    {
        return TOTALNODES;
    }
}
