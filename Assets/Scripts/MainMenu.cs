using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayMenuPanel;
    [SerializeField] private GameObject IntroPanel;
    [SerializeField] private Button IntroPanel_OptionsButton;
    [SerializeField] private Button IntroPanel_PlayButton;
    [SerializeField] private TextMeshProUGUI PlayMenuPanel_PlayerLevel;
    [SerializeField] private GameObject[] PlayMenuPanel_Upgrades;
    [SerializeField] private GameObject[] PlayMenuPanel_CowCounter;
    [SerializeField] private TextMeshProUGUI PlayMenuPanel_Coins;
    [SerializeField] private Button PlayMenuPanel_PlayButton;
    [SerializeField] private GameObject modalConfirmPurchase;
    [SerializeField] private Image modalIcon;
    [SerializeField] private TextMeshProUGUI modalTextValue;
    [SerializeField] private Upgrade upgradeToPurchase;


    //private void StartMenuUI()
    //{
    //    //GameControl.Instance.SetupMainMenu(this);
    //   //GameControl.Instance.gameObject.GetComponent<UIManager>().SetupMainMenu(this);
    //}

   
    public void ShowIntroMenu()
    {
        IntroPanel.SetActive(true);
        PlayMenuPanel.SetActive(false);
        CloseModal();
    }

    public void ShowPlayMenu()
    {
        CloseModal();
        IntroPanel.SetActive(false);
        PlayMenuPanel.SetActive(true);
        PlayMenuPanel_Coins.SetText(GameControl.Instance.playerData.currentPoints.ToString());
        foreach(GameObject obj in PlayMenuPanel_Upgrades)
        {
            obj.GetComponent<Upgrade>().SetupUpgrade();
        }

        int i = 0;
        foreach(GameObject obj in PlayMenuPanel_CowCounter)
        {
            TextMeshProUGUI counter = obj.transform.Find("Counter").GetComponent<TextMeshProUGUI>();
            switch(i)
            {
                case 0: 
                    counter.SetText(GameControl.Instance.playerData.numCow01.ToString());
                    break;
                case 1:
                    counter.SetText(GameControl.Instance.playerData.numCow02.ToString());
                    break;
                case 2:
                    counter.SetText(GameControl.Instance.playerData.numCow03.ToString());
                    break;
                case 3:
                    counter.SetText(GameControl.Instance.playerData.numCow04.ToString());
                    break;
                default:
                    break;
            }
            i++;
        }
    }

   

    public void ShowModalPurchase(Upgrade _upgrade)
    {
        upgradeToPurchase = _upgrade;
        modalConfirmPurchase.SetActive(true);
        modalIcon.sprite = upgradeToPurchase.GetUpgradeIcon();
        modalTextValue.SetText(upgradeToPurchase.GetUpgradeValue().ToString());
    }

    public void CanPurchaseUpgrade()
    {
        
        upgradeToPurchase.PurchaseUpgrade();
        CloseModal();
        
    }

    public void CloseModal()
    {
        upgradeToPurchase = null;
        modalConfirmPurchase.SetActive(false);
    }
}
