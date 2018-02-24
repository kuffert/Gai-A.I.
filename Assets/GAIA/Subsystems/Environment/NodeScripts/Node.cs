using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public enum NodeDirection { None, North, East, South, West };
    public enum LifeState { Dead, Stage1, Stage2, Stage3, Flourishing }

    protected static bool VALIDATIONMODE = false;
    public string nodeName;
    public NodePrefabs nodePrefabsData;
    public float lifeResistance;
    public float lifeThreshhold;
    public float currentLifeLevel;
    public LifeState currentLifeState = LifeState.Dead;

    protected Node NorthNode = null;
    protected Node EastNode = null;
    protected Node SouthNode = null;
    protected Node WestNode = null;
    protected GameObject nodeRender;
    protected Node prevNode = null;
    protected NodeDirection prevNodeDir = NodeDirection.None;
    protected float currentLifeDelta = .05f;
    protected static Dictionary<NodeDirection, int[]> nodeOrderMapping = new Dictionary<NodeDirection, int[]>();

    protected static int NODESIZE = 3;
    protected static int TOTALNODES = 0;

    protected static float DULLNODECHANCE;

    protected static int FRACTALS = 1;
    protected static int MAXFRACTALS;
    protected static int LIFEDISPERSALINTERVAL;

    protected static int WATERFRACTALS = 1;
    protected static int MAXWATERGENERATIONS;
    protected static int MAXINDIVIDUALWATERFRACTALS;
    protected static float WATERFRACTALCHANCE;
    protected static float WATERFRACTALCHANCEDECAYRATE;

    protected static List<GameObject> ALLNODEOBJECTS = new List<GameObject> { };
    protected static List<Node> ALLNODESCRIPTS = new List<Node> { };
    protected static int[] LIFESTATEVALUES = new int[] { 0, 10, 25, 65, 75 };

    private static bool VALIDATED = false;
    private int maxLifeLevel;



    #region ENVGEN

    public static bool nodeGenerationCompleted()
    {
        return FRACTALS >= MAXFRACTALS;
    }

    public static bool waterNodeGenerationCompleted()
    {
        return WATERFRACTALS >= MAXINDIVIDUALWATERFRACTALS * MAXWATERGENERATIONS;
    }

    protected void setMaxFractals(int n)
    {
        MAXFRACTALS = n;
    }

    protected void setLifeDispersalInterval(int n)
    {
        LIFEDISPERSALINTERVAL = n;
    }

    protected void setMaxWaterGenerations(int n)
    {
        MAXWATERGENERATIONS = n;
    }

    protected void setDullNodeChance(float f)
    {
        DULLNODECHANCE = f;
    }

    protected void setMaxWaterFractals(int n)
    {
        MAXINDIVIDUALWATERFRACTALS = n;
    }

    protected void setWaterFractalChance(float f)
    {
        WATERFRACTALCHANCE = f;
    }
    protected void setWaterFractalChanceDecayRate(float f)
    {
        WATERFRACTALCHANCEDECAYRATE = f;
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

    protected void incrementWaterFractals()
    {
        WATERFRACTALS++;
    }

    protected abstract void generateNeighbors();
    public abstract void generateWater(int tickInterval, int fractalID);

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Observer.updateNodeInfo(nodeName, lifeResistance, lifeThreshhold, currentLifeLevel);
        }
    }

    protected void setPrevNodeNeighbor(Node prevNode, NodeDirection prevNodeDir)
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
        NorthNode = NorthNode ? NorthNode : (Physics.Raycast(transform.position, Vector3.right, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject.GetComponent<Node>() : null;
        EastNode = EastNode ? EastNode : (Physics.Raycast(transform.position, -Vector3.forward, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject.GetComponent<Node>() : null;
        SouthNode = SouthNode ? SouthNode : (Physics.Raycast(transform.position, -Vector3.right, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject.GetComponent<Node>() : null;
        WestNode = WestNode ? WestNode : (Physics.Raycast(transform.position, Vector3.forward, out hit, NODESIZE, nodeLayerMask)) ? hit.collider.gameObject.GetComponent<Node>() : null;
    }

    protected void generateNodeColliderAndSetCollisionLayer()
    {
        gameObject.AddComponent<BoxCollider>().size = new Vector3(.75f * NODESIZE, 2f * NODESIZE, .75f * NODESIZE);
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

    #endregion

    #region SIMULATION

    public void gainLife(float amount)
    {
        updateCurrentLifeLevel(amount);
    }

    protected abstract void disperseLifeToNeighbors();

    protected abstract void updateCurrentLifeLevel(float change);

    protected abstract void updateCurrentLifeState(LifeState newLifeState);

    #endregion
}
