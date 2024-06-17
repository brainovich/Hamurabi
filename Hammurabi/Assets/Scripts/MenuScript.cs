using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScript : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject manualScreen;
    public GameObject[] manualScreens;
    public TextMeshProUGUI manualScreenNumText;
    private int manualScreenNum;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Manual()
    {
        mainScreen.SetActive(false);
        manualScreen.SetActive(true);
        manualScreenNum = 0;
        manualScreens[manualScreenNum].SetActive(true);
        manualScreenNumText.text = (manualScreenNum + 1).ToString();
    }

    public void ExitManual()
    {
        mainScreen.SetActive(true);
        manualScreen.SetActive(false);
        manualScreens[manualScreenNum].SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ManualLeftButton()
    {
        if(manualScreenNum-1 >= 0)
        {
            manualScreens[manualScreenNum].SetActive(false);
            manualScreenNum--;
            manualScreenNumText.text = (manualScreenNum + 1).ToString();
            manualScreens[manualScreenNum].SetActive(true);
        }
    }
    public void ManualRightButton()
    {
        if (manualScreenNum + 1 <= manualScreens.Length - 1)
        {
            manualScreens[manualScreenNum].SetActive(false);
            manualScreenNum++;
            manualScreenNumText.text = (manualScreenNum + 1).ToString();
            manualScreens[manualScreenNum].SetActive(true);
        }
    }

}
