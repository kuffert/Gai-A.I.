using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public enum NodeDirection { None, North, East, South, West };

    protected static bool VALIDATIONMODE = false;
    public NodePrefabs nodePrefabsData;
    public float lifeResistance;
    public float lifeThreshhold;
    public int currentLifeLevel;

    protected GameObject NorthNode = null;
    protected GameObject EastNode = null;
    protected GameObject SouthNode = null;
    protected GameObject WestNode = null;
    protected GameObject nodeRender;
    protected GameObject prevNode = null;
    protected NodeDirection prevNodeDir = NodeDirection.None;
    protected static Dictionary<NodeDirection, int[]> nodeOrderMapping = new Dictionary<NodeDirection, int[]>();

    protected static int TOTALNODES = 0;
    protected static int FRACTALS = 1;
    protected static int NODESIZE = 3;
    protected static int MAXFRACTALS = 15;
    protected static List<GameObject> ALLNODEOBJECTS = new List<GameObject> { };

    private static bool VALIDATED = false;
    private int maxLifeLevel;

    protected void setMaxFractals(int n)
    {
        MAXFRACTALS = n;
    }

    protected void setValidationMode(bool validationMode)
    {
        VALIDATIONMODE = validationMode;
    }
    
    protected void incrementTotalNodes()
    {
        TOTALNODES++;
    }

    protected void incrementFractals()
    {
        FRACTALS++;
    }

    protected abstract void generateNeighbors();


    protected void setPrevNodeNeighbor(GameObject prevNode, NodeDirection prevNodeDir)
    {
        switch (prevNodeDir)
        {
            case NodeDirection.North:
                NorthNode = prevNode;
                break;
            case NodeDirection.East:
                EastNode = prevNode;
                break;
            case NodeDirection.South:
                SouthNode = prevNode;
                break;
            case NodeDirection.West:
                WestNode = prevNode;
                break;
        }
    }
    protected void findNeighbors()
    {
        int nodeLayerMask = 1 << 8;
        RaycastHit hit;
        NorthNode = NorthNode ? NorthNode : (Physics.Raycast(transform.position, Vector3.right, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject : null;
        EastNode = EastNode ? EastNode : (Physics.Raycast(transform.position, -Vector3.forward, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject : null;
        SouthNode = SouthNode ? SouthNode : (Physics.Raycast(transform.position, -Vector3.right, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject : null;
        WestNode = WestNode ? WestNode : (Physics.Raycast(transform.position, Vector3.forward, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject : null;
    }

    protected void generateNodeColliderAndSetCollisionLayer()
    {
        gameObject.AddComponent<BoxCollider>().size = new Vector3(.75f * NODESIZE, .75f * NODESIZE, .75f * NODESIZE);
        gameObject.layer = LayerMask.NameToLayer("Nodes");
    }


    protected int VALIDATEdoublingUpNodes()
    {
        if (VALIDATED) return -1;
        VALIDATED = true;
        int totalDupes = 0;
        for (int i = 0; i < ALLNODEOBJECTS.Count; i++)
        {
            Vector3 nodePos = ALLNODEOBJECTS[i].transform.position;
            for (int k = i + 1; k < ALLNODEOBJECTS.Count; k++)
            {
                totalDupes += (nodePos == ALLNODEOBJECTS[k].transform.position) ? 1 : 0;
            }
        }
        return totalDupes;
    }

}
