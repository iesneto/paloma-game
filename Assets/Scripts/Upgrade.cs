using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UpgradeData upgradeData;
    [SerializeField] private int controlVariableRef;
    [SerializeField] private Image icon;
    [SerializeField] private Text description;
    [SerializeField] private Image[] stars;
    [SerializeField] private Text valueShadow;
    [SerializeField] private Text value;
    [SerializeField] private Image mask;
    [SerializeField] private Color colorOn;
    [SerializeField] private Color maskColorOn;
    [SerializeField] private Color maskColorOff;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameObject maxLevelLabel;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void SetupUpgrade()
    {
        icon.sprite = upgradeData.icon;
        switch (upgradeData.type)
        {
            case UpgradeData.ControlVar.RAYRADIUS: 
                controlVariableRef = GameControl.Instance.playerData.rayRadius;
                break;
            case UpgradeData.ControlVar.RAYFORCE:
                controlVariableRef = GameControl.Instance.playerData.rayForce;
                break;
            case UpgradeData.ControlVar.RAYMULTIPLIER:
                controlVariableRef = GameControl.Instance.playerData.rayMultiplier;
                break;
            case UpgradeData.ControlVar.PLAYERSPEED:
                controlVariableRef = GameControl.Instance.playerData.playerSpeed;
                break;
            case UpgradeData.ControlVar.PLAYERENERGY:
                controlVariableRef = GameControl.Instance.playerData.playerEnergy;
                break;
            case UpgradeData.ControlVar.PLAYERENERGYCONSUME:
                controlVariableRef = GameControl.Instance.playerData.playerEnergyConsume;
                break;
            default:
                break;
        }
        switch(GameControl.Instance.playerData.language)
        {
            case SystemLanguage.Portuguese: description.text = upgradeData.title[0];
                break;
            default: description.text = upgradeData.title[1];
                break;
        }
        for(int i = 0; i < controlVariableRef; i++)
        {
            stars[i].color = colorOn;
        }

        if (controlVariableRef == upgradeData.value.Length) maxLevelLabel.SetActive(true);
        else
        {
            value.text = upgradeData.value[controlVariableRef].ToString();
            valueShadow.text = value.text;

            if (GameControl.Instance.playerData.currentPoints >= upgradeData.value[controlVariableRef])
            {
                mask.color = maskColorOn;
                upgradeButton.enabled = true;
            }
            else
            {
                mask.color = maskColorOff;
                upgradeButton.enabled = false;
            }
        }
    }

    public void ConfirmPurchaseWindow()
    {
        mainMenu.ShowModalPurchase(this);
    }

    public void PurchaseUpgrade()
    {
        GameControl.Instance.PurchaseUpgrade(upgradeData.type, upgradeData.value[controlVariableRef]);
    }

    public Sprite GetUpgradeIcon()
    {
        return upgradeData.icon;
    }

    public long GetUpgradeValue()
    {
        return upgradeData.value[controlVariableRef];
    }
}
