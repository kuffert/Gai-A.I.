using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIASimulationManager : MonoBehaviour
{

    public enum SimState { Standby, EnvGen, GAIAControl, Override, PostSim, End }

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
    private static bool manualOverride = false;

    void Awake()
    {
        iterationStamp = System.DateTime.Now;
    }

    void Start()
    {
        changeSimState(SimState.EnvGen);
    }

    void Update()
    {
        checkChangeSimState();
    }


    public static SimState getSimState()
    {
        return SimulationState;
    }

    public static void setManualOverride()
    {
        manualOverride = true;
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
                    Observer.updateLoadingBarActive(true, "Relinqushing control to GAIA", new Color(213f/255f, 64f/255f, 255f/255f));
                    loadGaiaControl = true;
                    break;
                }
                if (loadGaiaControl && loadGaiaControlTick % 60 == 0)
                {
                    loadGaiaControl = false;
                    finishedLoadingGaiaControl = true;
                    if (manualOverride)
                    {
                        changeSimState(SimState.Override);
                    }
                    else
                    {
                        changeSimState(SimState.GAIAControl);
                    }
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
                Observer.updateLoadingBarActive(true, "Generating Environment", new Color(64f/255f, 255f/255f, 57f/255f));
                break;

            case SimState.GAIAControl:
                Soliloquy.envLog("Environment Generation Complete.");
                Observer.updateLoadingBarActive(true, "GAIA control enabled", new Color(213f/255f, 64f/255f, 255f/255f));
                break;

            case SimState.Override:
                Soliloquy.overrideLog("Manual Override enabled");
                Observer.updateLoadingBarActive(true, "Override Enabled", new Color(213f/255f, 64f/255f, 255f/255f));
                ManualOverride.enableOverride();
                break;
        }
    }
}
