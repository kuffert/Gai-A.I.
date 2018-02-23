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

    void Start()
    {
        xLoc = loadingBar.GetComponent<RectTransform>().anchoredPosition.x;
        yLoc = loadingBar.GetComponent<RectTransform>().anchoredPosition.y;
        loadingBarText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 12f, 0f);
        limit = xLoc + 272;
        curDis = 0;
        loadingBarTextScript = loadingBarText.GetComponent<Text>();
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
