using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterNode : Node
{

    private int tick;
    private int convertNeighborsAtTick;
    private bool convertingNeighborsToWater = false;
    private bool convertedNeighbors = false;
    private int neighborsConverted = 0;
    private int fractalID;


    void Start()
    {
        GameObject waterNodePrefab = nodePrefabsData.waterNodePrefab;
        nodeRender = Instantiate(waterNodePrefab, transform.position, transform.rotation);
        nodeRender.transform.parent = gameObject.transform;
        setPrevNodeNeighbor(prevNode, prevNodeDir);
        generateNodeColliderAndSetCollisionLayer();
        findNeighbors();
        convertNeighborsAtTick += nodeOrderMapping[prevNodeDir][0];
        nodeRender.layer = 8;
    }

    void Update()
    {
        switch (GAIASimulationManager.getSimState())
        {
            case GAIASimulationManager.SimState.WaterGen:
                tick++;
                findNeighbors();
                convertNeighbors();
                break;

            case GAIASimulationManager.SimState.GAIAControl:
                tick++;
                disperseLifeToNeighbors();
                break;
        }
    }

    public static GameObject generateWaterNode(
        Node prevNode,
        NodeDirection prevNodeDir,
        int fractalID,
        Vector3 transformPosition,
        NodePrefabs nodePrefabsData)
    {
        GameObject newWaterNodeObj = new GameObject("WaterNode" + WATERFRACTALS + " Fractal# " + fractalID);
        newWaterNodeObj.transform.position = transformPosition;

        WaterNode waterNodeScript = newWaterNodeObj.AddComponent<WaterNode>();
        waterNodeScript.nodePrefabsData = nodePrefabsData;
        waterNodeScript.nodeName = "WaterNode" + WATERFRACTALS + " Fractal# " + fractalID;
        waterNodeScript.prevNode = prevNode;
        waterNodeScript.prevNodeDir = prevNodeDir;
        waterNodeScript.fractalID = fractalID;
        waterNodeScript.lifeResistance = 0;
        waterNodeScript.lifeThreshhold = 0;
        waterNodeScript.currentLifeLevel = 100;

        ALLNODEOBJECTS.Add(newWaterNodeObj);
        ALLNODESCRIPTS.Add(waterNodeScript);
        return newWaterNodeObj;
    }

    private void convertNeighbors()
    {
        if (!convertedNeighbors && tick % convertNeighborsAtTick == 0)
        {
            convertedNeighbors = true;
            if (fractalID <= MAXINDIVIDUALWATERFRACTALS)
            {
                incrementWaterFractals();
                convertingNeighborsToWater = true;
            }
            else
            {
                convertingNeighborsToWater = false;
            }
        }

        if (convertingNeighborsToWater)
        {
            int nVal = convertNeighborsAtTick + nodeOrderMapping[NodeDirection.North][1];
            int eVal = convertNeighborsAtTick + nodeOrderMapping[NodeDirection.East][1];
            int sVal = convertNeighborsAtTick + nodeOrderMapping[NodeDirection.South][1];
            int wVal = convertNeighborsAtTick + nodeOrderMapping[NodeDirection.West][1];

            if (tick == nVal)
            {
                NorthNode.generateWater(nodeOrderMapping[NodeDirection.North][1], fractalID + 1);
                neighborsConverted++;
            }
            if (tick == eVal)
            {
                EastNode.generateWater(nodeOrderMapping[NodeDirection.East][1], fractalID + 1);
                neighborsConverted++;
            }
            if (tick == sVal)
            {
                SouthNode.generateWater(nodeOrderMapping[NodeDirection.South][1], fractalID + 1);
                neighborsConverted++;
            }
            if (tick == wVal)
            {
                WestNode.generateWater(nodeOrderMapping[NodeDirection.West][1], fractalID + 1);
                neighborsConverted++;
            }
            if (neighborsConverted >= 4)
            {
                convertingNeighborsToWater = false;
            }
        }
    }

    protected override void generateNeighbors()
    {
        throw new System.NotImplementedException();
    }

    override protected void disperseLifeToNeighbors()
    {
        float dispersalAmout = (currentLifeDelta * currentLifeLevel);
        if (tick % LIFEDISPERSALINTERVAL == 0)
        {
            NorthNode.gainLife(dispersalAmout);
            EastNode.gainLife(dispersalAmout);
            SouthNode.gainLife(dispersalAmout);
            WestNode.gainLife(dispersalAmout);
        }
    }

    override public void generateWater(int tickInterval, int fractalID) { }
    override protected void updateCurrentLifeLevel(float change) { }
    override protected void updateCurrentLifeState(LifeState newLifeState) { }


    private bool calculateConversionOdds()
    {
        return Random.Range(1, 100) / 100f <= WATERFRACTALCHANCE - fractalID * WATERFRACTALCHANCEDECAYRATE;
    }
}
