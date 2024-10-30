using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playerHud;

    public void Play()
    {
        GameManager.Instance.UpdateMenu(false);
        mainMenu.SetActive(false);
        
        foreach (Transform child in playerHud.transform)
        {
            if (!child.name.Contains("Combat"))
            {
                child.gameObject.SetActive(true);

                if (child.transform.childCount > 0)
                {
                    foreach (Transform grandChild in child.transform)
                    {
                        grandChild.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
