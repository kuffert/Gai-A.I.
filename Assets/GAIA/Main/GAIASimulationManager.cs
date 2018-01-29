using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAIASimulationManager : MonoBehaviour {

	public enum SimState { Standby, EnvGen, GAIAControl, PostSim, End }
    
	public NodePrefabs nodePrefabsData;
	public int initialLifeRes;
	public int intitialLifeThresh;
	public int maxFractals;
	public bool validationMode;
	
	private GameObject inceptorNode;
	private static SimState SimulationState = SimState.Standby;


    // Use this for initialization
    void Start () 
	{
		changeSimState(SimState.EnvGen);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static SimState getSimState()
	{
		return SimulationState;
	}

	private void changeSimState(SimState newState)
	{
		SimulationState = newState;
		switch(SimulationState)
		{
			case SimState.EnvGen:
			 inceptorNode = InceptorNode.generateInceptorNode(initialLifeRes, intitialLifeThresh, maxFractals, validationMode, new Vector3(0,0,0), nodePrefabsData);
			break;
		}
	}
}
