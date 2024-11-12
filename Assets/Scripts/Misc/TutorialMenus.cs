using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenus : MonoBehaviour
{
    public List<GameObject> tutorialMenus = new List<GameObject>();
    private int currentMenuIndex = 0;

    public void NextMenu()
    {
        tutorialMenus[currentMenuIndex].SetActive(false);
        currentMenuIndex++;
        if (currentMenuIndex >= tutorialMenus.Count)
        {
            currentMenuIndex = 0;
        }
        tutorialMenus[currentMenuIndex].SetActive(true);
    }

    public void PreviousMenu()
    {
        tutorialMenus[currentMenuIndex].SetActive(false);
        currentMenuIndex--;
        if (currentMenuIndex < 0)
        {
            currentMenuIndex = tutorialMenus.Count - 1;
        }
        tutorialMenus[currentMenuIndex].SetActive(true);
    }
}
