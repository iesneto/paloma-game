using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject PlayMenuPanel;
    [SerializeField] private GameObject IntroPanel;
    [SerializeField] private Button IntroPanel_OptionsButton;
    [SerializeField] private Button IntroPanel_PlayButton;
    [SerializeField] private Text PlayMenuPanel_PlayerLevel;
    [SerializeField] private GameObject[] PlayMenuPanel_Upgrades;
    [SerializeField] private GameObject[] PlayMenuPanel_CowCounter;
    [SerializeField] private Text PlayMenuPanel_Coins;
    [SerializeField] private Button PlayMenuPanel_PlayButton;
    [SerializeField] private GameObject modalConfirmPurchase;
    [SerializeField] private Image modalIcon;
    [SerializeField] private Text modalTextValue;
    [SerializeField] private Text modalTextValueShadow;
    [SerializeField] private Upgrade upgradeToPurchase;


    private void Start()
    {
        GameControl.Instance.SetupMainMenu(this);
    }

   
    public void ShowIntroMenu()
    {
        IntroPanel.SetActive(true);
        PlayMenuPanel.SetActive(false);
    }

    public void ShowPlayMenu()
    {
        IntroPanel.SetActive(false);
        PlayMenuPanel.SetActive(true);
        PlayMenuPanel_Coins.text = GameControl.Instance.playerData.currentPoints.ToString();
        foreach(GameObject obj in PlayMenuPanel_Upgrades)
        {
            obj.GetComponent<Upgrade>().SetupUpgrade();
        }

        int i = 0;
        foreach(GameObject obj in PlayMenuPanel_CowCounter)
        {
            Text counter = obj.transform.Find("Counter").GetComponent<Text>();
            switch(i)
            {
                case 0: 
                    counter.text = GameControl.Instance.playerData.numCow01.ToString();
                    break;
                case 1:
                    counter.text = GameControl.Instance.playerData.numCow02.ToString();
                    break;
                case 2:
                    counter.text = GameControl.Instance.playerData.numCow03.ToString();
                    break;
                case 3:
                    counter.text = GameControl.Instance.playerData.numCow04.ToString();
                    break;
                default:
                    break;
            }
            i++;
        }
    }

    public void LoadLevel()
    {
        GameControl.Instance.StartPlaying();
        GameControl.Instance.gameObject.GetComponent<SceneController>().LoadRandomLevel();
    }

    public void ShowModalPurchase(Upgrade _upgrade)
    {
        upgradeToPurchase = _upgrade;
        modalConfirmPurchase.SetActive(true);
        modalIcon.sprite = upgradeToPurchase.GetUpgradeIcon();
        modalTextValue.text = upgradeToPurchase.GetUpgradeValue().ToString();
        modalTextValueShadow.text = modalTextValue.text;
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
