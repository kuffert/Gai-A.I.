using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIASimulationManager : MonoBehaviour
{

    public enum SimState { Standby, EnvGen, GAIAControl, PostSim, End }

    public NodePrefabs nodePrefabsData;
    public int initialLifeRes;
    public int intitialLifeThresh;
    public int maxFractals;
    public bool validationMode;
    public static System.DateTime iterationStamp;

    private GameObject inceptorNode;
    private static SimState SimulationState = SimState.Standby;

    private bool loadGaiaControl = false;
    private bool finishedLoadingGaiaControl = false;
    private int loadGaiaControlTick = 0;

    void Awake()
    {
        iterationStamp = System.DateTime.Now;
    }

    // Use this for initialization
    void Start()
    {
        changeSimState(SimState.EnvGen);
    }

    // Update is called once per frame
    void Update()
    {
        checkChangeSimState();
    }


    public static SimState getSimState()
    {
        return SimulationState;
    }

    private void checkChangeSimState()
    {
        if (loadGaiaControl)
        {
            loadGaiaControlTick++;
        }

        switch (SimulationState)
        {
            case SimState.EnvGen:
                if (InceptorNode.nodeGenerationCompleted() && !finishedLoadingGaiaControl && !loadGaiaControl)
                {
                    loadGaiaControl = true;
                    break;
                }
                if (loadGaiaControl && loadGaiaControlTick % 60 == 0)
                {
                    loadGaiaControl = false;
                    finishedLoadingGaiaControl = true;
                    changeSimState(SimState.GAIAControl);
                    break;
                }
				break;
        }
    }

    private void changeSimState(SimState newState)
    {
        SimulationState = newState;
        switch (SimulationState)
        {
            case SimState.EnvGen:
                inceptorNode = InceptorNode.generateInceptorNode(initialLifeRes, intitialLifeThresh, maxFractals, validationMode, new Vector3(0, 0, 0), nodePrefabsData);
                Soliloquy.envLog("Beginning Environment Generation...");
                break;

            case SimState.GAIAControl:
                Soliloquy.envLog("Environment Generation Complete.");
                break;
        }
    }
}
