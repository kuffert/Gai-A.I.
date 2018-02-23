﻿using System.Collections;
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
        }
    }

    public static GameObject generateWaterNode(
        GameObject prevNode,
        NodeDirection prevNodeDir,
        GameObject northNode,
        GameObject eastNode,
        GameObject westNode,
        GameObject southNode,
        int fractalID,
        Vector3 transformPosition,
        NodePrefabs nodePrefabsData)
    {
        GameObject newWaterNodeObj = new GameObject("WaterNode" + TOTALNODES + " Fractal# " + FRACTALS);
        newWaterNodeObj.transform.position = transformPosition;

        WaterNode waterNodeScript = newWaterNodeObj.AddComponent<WaterNode>();
        waterNodeScript.nodePrefabsData = nodePrefabsData;
        waterNodeScript.prevNode = prevNode;
        waterNodeScript.prevNodeDir = prevNodeDir;
        waterNodeScript.NorthNode = northNode;
        waterNodeScript.EastNode = eastNode;
        waterNodeScript.SouthNode = southNode;
        waterNodeScript.WestNode = westNode;
        waterNodeScript.fractalID = fractalID;

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
                Node northScript = NorthNode.GetComponent<StandardNode>();
                if (northScript && calculateConversionOdds())
                {
                    northScript.generateWater(nodeOrderMapping[NodeDirection.North][1], fractalID + 1);
                }
                neighborsConverted++;
            }
            if (tick == eVal)
            {
                Node eastScript = EastNode.GetComponent<StandardNode>();
                if (eastScript && calculateConversionOdds())
                {
                    eastScript.generateWater(nodeOrderMapping[NodeDirection.East][1], fractalID + 1);
                }
                neighborsConverted++;
            }
            if (tick == sVal)
            {
                Node southScript = SouthNode.GetComponent<StandardNode>();
                if (southScript && calculateConversionOdds())
                {
                    southScript.generateWater(nodeOrderMapping[NodeDirection.South][1], fractalID + 1);
                }
                neighborsConverted++;
            }
            if (tick == wVal)
            {
                Node westScript = WestNode.GetComponent<StandardNode>();
                if (westScript && calculateConversionOdds())
                {
                    westScript.generateWater(nodeOrderMapping[NodeDirection.West][1], fractalID + 1);
                }
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

    override public void generateWater(int tickInterval, int fractalID)
    {

    }

    override protected void disperseLifeToNeighbors()
    {
        
    }

    private bool calculateConversionOdds()
    {
        return Random.Range(1, 100) / 100f <= WATERFRACTALCHANCE - fractalID * WATERFRACTALCHANCEDECAYRATE;
    }
}
