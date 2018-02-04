using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Soliloquy : ScriptableObject {

	public static void envLog(string msg)
	{
		using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"F:\Unity Projects\GAIA Logs\EnvironmentLog.txt", append:true))
		{
			logFile.WriteLine(GAIASimulationManager.iterationStamp + ": " + msg + "\n");
		}
	}

	public static void memoryLog(string msg)
	{
		using (System.IO.StreamWriter logFile = new System.IO.StreamWriter(@"F:\Unity Projects\GAIA Logs\MemoryLog.txt", append:true))
		{
			logFile.WriteLine(GAIASimulationManager.iterationStamp + ": " + msg + "\n");
		}
	}

	public static void commandLog(string msg)
	{
		
	}
}
