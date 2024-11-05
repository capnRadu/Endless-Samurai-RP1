using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ActionControl : MonoBehaviour
{
    public HeroKnight playerScript;
    public Sprite[] actionKeys;
    public Image[] icons;

    private void OnEnable()
    {
        SetupSprites();
    }

    public void SetupSprites()
    {
        string activeKeyName = playerScript.activeKey.ToString();
        var activeKeySprite = actionKeys[0];

        foreach (var actionKey in actionKeys)
        {
            if (actionKey.name == activeKeyName)
            {
                activeKeySprite = actionKey;
                break;
            }
        }

        foreach (var icon in icons)
        {
            icon.sprite = activeKeySprite;
        }
    }
}
