using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Observer : MonoBehaviour
{

    public GameObject loadingBar;
    public GameObject loadingBarText;
    public static Text loadingBarTextScript;
    private static bool loadingBarActive = false;
    public float moveSpeed;
    private float limit;
    private float curDis;
    private float xLoc;
    private float yLoc;

    public GameObject nodeInfo;
    public GameObject nodeInfoText;
    public GameObject nodeInfoLifeRes;
    public GameObject nodeInfoLifeThresh;
    public GameObject nodeInfoCurLife;
    public static Text nodeInfoTextScript;
    public static Text nodeInfoLifeResScript;
    public static Text nodeInfoLifeThreshScript;
    public static Text nodeInfoCurLifeScript;

    void Start()
    {
        xLoc = loadingBar.GetComponent<RectTransform>().anchoredPosition.x;
        yLoc = loadingBar.GetComponent<RectTransform>().anchoredPosition.y;
        loadingBarText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 12f, 0f);
        limit = xLoc + 272;
        curDis = 0;
        loadingBarTextScript = loadingBarText.GetComponent<Text>();
        nodeInfoTextScript = nodeInfoText.GetComponent<Text>();
        nodeInfoLifeResScript = nodeInfoLifeRes.GetComponent<Text>();
        nodeInfoLifeThreshScript = nodeInfoLifeThresh.GetComponent<Text>();
        nodeInfoCurLifeScript = nodeInfoCurLife.GetComponent<Text>();
    }

    void Update()
    {
        updateUIComponentLocations();
    }

    public static void updateLoadingBarActive(bool newState, string newText, Color newColor)
    {
        loadingBarActive = newState;
        loadingBarTextScript.color = newColor;
        loadingBarTextScript.text = newText;
    }

    public static void updateNodeInfo(string newNodeName, float newLifeRes, float newLifeThresh, float newCurLife)
    {
        nodeInfoTextScript.text = newNodeName;
        nodeInfoLifeResScript.text = "Life Resistance: " + newLifeRes.ToString();
        nodeInfoLifeThreshScript.text = "Life Threshold: " + newLifeThresh.ToString();
        nodeInfoCurLifeScript.text = "Current Life Level: " +  newCurLife.ToString();
    }

    private void updateUIComponentLocations()
    {
        if (loadingBarActive && (curDis <= limit))
        {

            loadingBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(moveSpeed + loadingBar.GetComponent<RectTransform>().anchoredPosition.x, yLoc, 0f);
            curDis += moveSpeed;
        }
        if (!loadingBarActive && (curDis >= 0))
        {
            loadingBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(-moveSpeed + loadingBar.GetComponent<RectTransform>().anchoredPosition.x, yLoc, 0f);
            curDis -= moveSpeed;
        }
    }
}
