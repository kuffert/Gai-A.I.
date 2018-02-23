using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardNode : Node
{

    private int tick;
    private int spawnChildrenAtTick = 0;
    private bool spawnedChildren = false;
    private bool generatingChildren = false;
    private int neighborsGenerated = 0;

    void Start()
    {
        GameObject standardNodePrefab = nodePrefabsData.standardNodePrefab;
        nodeRender = Instantiate(standardNodePrefab, transform.position, transform.rotation);
        nodeRender.transform.parent = gameObject.transform;
        setPrevNodeNeighbor(prevNode, prevNodeDir);
        generateNodeColliderAndSetCollisionLayer();
        findNeighbors();
        spawnChildrenAtTick += nodeOrderMapping[prevNodeDir][0];
        nodeRender.layer = 8;
    }

    void Update()
    {
        switch (GAIASimulationManager.getSimState())
        {
            case GAIASimulationManager.SimState.EnvGen:
                tick++;
                VALIDATIONCheckIfNodeHas4Neighbors(nodePrefabsData);
                findNeighbors();
                generateNeighbors();
                break;

            case GAIASimulationManager.SimState.WaterGen:
                findNeighbors();
                break;

            case GAIASimulationManager.SimState.GAIAControl:
                tick++;
                disperseLifeToNeighbors();
                break;

            case GAIASimulationManager.SimState.Override:
                break;
        }

    }

    #region ENVGEN

    public static GameObject generateStandardNode(Node prevNode, float prevNodeLifeRes, float prevNodeLifeThresh, NodeDirection prevNodeDir, Vector3 transformPosition, NodePrefabs nodePrefabsData)
    {
        if (Random.Range(1, 100) / 100f <= DULLNODECHANCE)
        {
            return DullNode.generateDullNode(prevNode, prevNodeDir, transformPosition, nodePrefabsData);
        }
        GameObject newStandardNodeObj = new GameObject("StandardNode" + TOTALNODES + " Fractal# " + FRACTALS);
        newStandardNodeObj.transform.position = transformPosition;

        StandardNode standardNodeScript = newStandardNodeObj.AddComponent<StandardNode>();
        standardNodeScript.incrementTotalNodes();
        standardNodeScript.lifeResistance = standardNodeScript.generatePsuedoRandomLifeRes(prevNodeLifeRes);
        standardNodeScript.lifeThreshhold = standardNodeScript.generatePsuedoRandomLifeThresh(prevNodeLifeThresh);
        standardNodeScript.nodePrefabsData = nodePrefabsData;
        standardNodeScript.prevNode = prevNode;
        standardNodeScript.prevNodeDir = prevNodeDir;

        ALLNODEOBJECTS.Add(newStandardNodeObj);
        ALLNODESCRIPTS.Add(standardNodeScript);
        return newStandardNodeObj;
    }

    override protected void generateNeighbors()
    {
        if (!spawnedChildren && tick % spawnChildrenAtTick == 0)
        {
            spawnedChildren = true;
            if (FRACTALS < MAXFRACTALS)
            {
                incrementFractals();
                generatingChildren = true;
            }
            else
            {
                NorthNode = NorthNode ? NorthNode : BoundaryNode.generateBoundaryNode(this, NodeDirection.South, transform.position + new Vector3(NODESIZE, 0, 0), nodePrefabsData).GetComponent<Node>();
                EastNode = EastNode ? EastNode : BoundaryNode.generateBoundaryNode(this, NodeDirection.West, transform.position + new Vector3(0, 0, -NODESIZE), nodePrefabsData).GetComponent<Node>();
                SouthNode = SouthNode ? SouthNode : BoundaryNode.generateBoundaryNode(this, NodeDirection.North, transform.position + new Vector3(-NODESIZE, 0, 0), nodePrefabsData).GetComponent<Node>();
                WestNode = WestNode ? WestNode : BoundaryNode.generateBoundaryNode(this, NodeDirection.East, transform.position + new Vector3(0, 0, NODESIZE), nodePrefabsData).GetComponent<Node>();
                Debug.Log(VALIDATEdoublingUpNodes());
                generatingChildren = false;
            }
        }

        if (generatingChildren)
        {
            int nVal = spawnChildrenAtTick + nodeOrderMapping[NodeDirection.North][1];
            int eVal = spawnChildrenAtTick + nodeOrderMapping[NodeDirection.East][1];
            int sVal = spawnChildrenAtTick + nodeOrderMapping[NodeDirection.South][1];
            int wVal = spawnChildrenAtTick + nodeOrderMapping[NodeDirection.West][1];

            if (tick == nVal)
            {
                Debug.Log("Creating node to the north");
                NorthNode = NorthNode ? NorthNode : StandardNode.generateStandardNode(this, lifeResistance, lifeThreshhold, NodeDirection.South, transform.position + new Vector3(NODESIZE, 0, 0), nodePrefabsData).GetComponent<Node>();
                neighborsGenerated++;
            }
            if (tick == eVal)
            {
                Debug.Log("Creating node to the east");
                EastNode = EastNode ? EastNode : StandardNode.generateStandardNode(this, lifeResistance, lifeThreshhold, NodeDirection.West, transform.position + new Vector3(0, 0, -NODESIZE), nodePrefabsData).GetComponent<Node>();
                neighborsGenerated++;
            }
            if (tick == sVal)
            {
                Debug.Log("Creating node to the south");
                SouthNode = SouthNode ? SouthNode : StandardNode.generateStandardNode(this, lifeResistance, lifeThreshhold, NodeDirection.North, transform.position + new Vector3(-NODESIZE, 0, 0), nodePrefabsData).GetComponent<Node>();
                neighborsGenerated++;
            }
            if (tick == wVal)
            {
                Debug.Log("Creating node to the west");
                WestNode = WestNode ? WestNode : StandardNode.generateStandardNode(this, lifeResistance, lifeThreshhold, NodeDirection.East, transform.position + new Vector3(0, 0, NODESIZE), nodePrefabsData).GetComponent<Node>();
                neighborsGenerated++;
            }
            if (neighborsGenerated >= 4)
            {
                generatingChildren = false;
            }
        }
    }

    override public void generateWater(int tickInterval, int fractalID)
    {
        WaterNode.generateWaterNode(prevNode, prevNodeDir, fractalID, gameObject.transform.position, nodePrefabsData);
        Destroy(gameObject);
    }


    protected void VALIDATIONCheckIfNodeHas4Neighbors(NodePrefabs prefabsData)
    {
        if (VALIDATIONMODE && tick % 30 == 0 && NorthNode && SouthNode && WestNode && EastNode)
        {
            Destroy(nodeRender);
            nodeRender = Instantiate(prefabsData.validatedNodePrefab, transform.position, transform.rotation);
            nodeRender.transform.parent = gameObject.transform;
        }
    }

    private float generatePsuedoRandomLifeRes(float prevLR)
    {
        float rangeMod = prevLR * 1.0f;
        float loVal = (prevLR - rangeMod) > 0 ? prevLR - rangeMod : 0;
        float hiVal = (prevLR + rangeMod) < 95 ? prevLR + rangeMod : 95;
        return Random.Range(loVal, hiVal);
    }

    private float generatePsuedoRandomLifeThresh(float prevLT)
    {
        float rangeMod = prevLT * .5f;
        float loVal = (prevLT - rangeMod) > 15 ? prevLT - rangeMod : 15;
        float hiVal = (prevLT + rangeMod) < 85 ? prevLT + rangeMod : 85;
        return Random.Range(loVal, hiVal);
    }

    #endregion

    #region SIMULATION

    override protected void updateCurrentLifeLevel(float change)
    {
        change = change * (1 - lifeResistance / 100f);
        currentLifeLevel = change > 0 ? ((currentLifeLevel + change) < 100 ? currentLifeLevel + change : 100) : ((currentLifeLevel - change) > 0 ? currentLifeLevel + change : 0);
        LifeState newLifeState = currentLifeState;
        switch (currentLifeState)
        {
            case LifeState.Dead:
                newLifeState = currentLifeLevel >= LIFESTATEVALUES[(int)LifeState.Stage1] + lifeThreshhold ? LifeState.Stage1 : newLifeState;
                break;
            case LifeState.Stage1:
                newLifeState = currentLifeLevel >= LIFESTATEVALUES[(int)LifeState.Stage2] + lifeThreshhold ? LifeState.Stage2 : newLifeState;
                newLifeState = currentLifeLevel < LIFESTATEVALUES[(int)LifeState.Stage1] + lifeThreshhold ? LifeState.Dead : newLifeState;
                break;
            case LifeState.Stage2:
                newLifeState = currentLifeLevel >= LIFESTATEVALUES[(int)LifeState.Stage3] + lifeThreshhold ? LifeState.Stage3 : newLifeState;
                newLifeState = currentLifeLevel < LIFESTATEVALUES[(int)LifeState.Stage2] + lifeThreshhold ? LifeState.Stage1 : newLifeState;
                break;
            case LifeState.Stage3:
                newLifeState = currentLifeLevel >= LIFESTATEVALUES[(int)LifeState.Flourishing] + lifeThreshhold ? LifeState.Flourishing : newLifeState;
                newLifeState = currentLifeLevel < LIFESTATEVALUES[(int)LifeState.Stage3] + lifeThreshhold ? LifeState.Stage2 : newLifeState;
                break;
            case LifeState.Flourishing:
                newLifeState = currentLifeLevel < LIFESTATEVALUES[(int)LifeState.Flourishing] + lifeThreshhold ? LifeState.Stage3 : newLifeState;
                break;
        }
        if (newLifeState != currentLifeState)
        {
            updateCurrentLifeState(newLifeState);
        }
    }

    override protected void updateCurrentLifeState(LifeState newLifeState)
    {
        currentLifeState = newLifeState;
        GameObject newPrefab = null;
        switch (currentLifeState)
        {
            case LifeState.Dead:
                newPrefab = nodePrefabsData.deadNodePrefab;
                break;
            case LifeState.Stage1:
                newPrefab = nodePrefabsData.stage1NodePrefab;
                nodePrefabsData.playChangeLifeLevelEffect(transform);
                break;
            case LifeState.Stage2:
                newPrefab = nodePrefabsData.stage2NodePrefab;
                nodePrefabsData.playChangeLifeLevelEffect(transform);
                break;
            case LifeState.Stage3:
                newPrefab = nodePrefabsData.stage3NodePrefab;
                nodePrefabsData.playChangeLifeLevelEffect(transform);
                break;
            case LifeState.Flourishing:
                newPrefab = nodePrefabsData.flourishingNodePrfab;
                nodePrefabsData.playChangeLifeLevelEffect(transform);
                break;
        }
        Destroy(nodeRender);
        nodeRender = Instantiate(newPrefab, transform.position, transform.rotation);
        nodeRender.transform.parent = gameObject.transform;
    }

    override protected void disperseLifeToNeighbors()
    {
        float dispersalAmout = (currentLifeDelta * (currentLifeLevel - lifeThreshhold));
        if (tick % LIFEDISPERSALINTERVAL == 0 && currentLifeLevel > (LIFESTATEVALUES[(int)LifeState.Stage1] + lifeThreshhold))
        {
            NorthNode.gainLife(dispersalAmout);
            EastNode.gainLife(dispersalAmout);
            SouthNode.gainLife(dispersalAmout);
            WestNode.gainLife(dispersalAmout);
        }
    }

    #endregion
}
