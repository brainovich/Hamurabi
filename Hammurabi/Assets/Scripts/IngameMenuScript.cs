using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IngameMenuScript : MonoBehaviour
{

    public GameObject menuScreen;
    public GameObject manualScreen;
    public GameObject[] manualScreens;
    public TextMeshProUGUI manualScreenNumText;
    private int manualScreenNum;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuScreen.activeSelf == false && !manualScreen.activeSelf)
                menuScreen.SetActive(true);
            else if (manualScreen.activeSelf)
                ExitManual();
            else
                menuScreen.SetActive(false);
        }
    }

    public void MenuScreen()
    {
        if (menuScreen.activeSelf == false)
            menuScreen.SetActive(true);
        else
            menuScreen.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Manual()
    {
        manualScreen.SetActive(true);
        menuScreen.SetActive(false);
        manualScreenNum = 0;
        manualScreens[manualScreenNum].SetActive(true);
        manualScreenNumText.text = (manualScreenNum + 1).ToString();
    }

    public void ExitManual()
    {
        manualScreen.SetActive(false);
        menuScreen.SetActive(true);
        manualScreens[manualScreenNum].SetActive(false);
    }

    public void ManualLeftButton()
    {
        if (manualScreenNum - 1 >= 0)
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

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
