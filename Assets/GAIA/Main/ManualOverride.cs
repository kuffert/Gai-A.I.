using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManualOverride : MonoBehaviour
{
    public bool useManualOverride = false;
    private static bool overrideEnabled = false;

    void Start()
    {
        if (useManualOverride)
        {
            GAIASimulationManager.setManualOverride();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void enableOverride()
    {
        overrideEnabled = true;
    }
}
