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
    //Update 28/10/2022 - Ivo Seitenfus
    //[SerializeField] private GameObject[] PlayMenuPanel_Upgrades;
    //[SerializeField] private GameObject[] PlayMenuPanel_CowCounter;
    [SerializeField] private TextMeshProUGUI PlayMenuPanel_Coins;
    [SerializeField] private Button PlayMenuPanel_PlayButton;
    [SerializeField] private GameObject modalConfirmPurchase;
    [SerializeField] private Image modalIcon;
    [SerializeField] private Image modalLevelIcon;
    [SerializeField] private TextMeshProUGUI modalTextValue;
    [SerializeField] private Upgrade upgradeToPurchase;
    [SerializeField] private GameObject flyingSaucerDisplayLocation;
    [SerializeField] private int currentFlyingsaucer;
    [SerializeField] private Image flyingSaucerSelectButtonImage;
    [SerializeField] private TextMeshProUGUI flyingSaucerSelectButtonText;
    [SerializeField] private Color flyingSaucerSelectedColor;
    [SerializeField] private Color flyingSaucerUnselectedColor;
    [SerializeField] private GameObject flyingSaucerTickConfirm;
    [SerializeField] private GameObject flyingSaucerLockedImage;
    [SerializeField] private GameObject flyingSaucerPrice;
    [SerializeField] private TextMeshProUGUI flyingSaucerPriceText;
    [SerializeField] private TextMeshProUGUI flyingSaucerLevelToUnlockText;
    [SerializeField] private GameObject flyingSaucerWindowItemPrefab;
    [SerializeField] private GameObject flyingSaucerListContentView;
    private FlyingSaucerMenuItem currentItem;
    private CowMenuItem currentCow;
    private bool buildedCowsArea = false;
    private GameObject windowOpen;

    [System.Serializable]
    private struct WindowFlyingSaucer
    {
        public GameObject window;
        public Animator windowAnimator;
        public TextMeshProUGUI flyingSaucerSelectButtonText;
        public Image flyingSaucerSelectButtonImage;
        public GameObject tickConfirm;
        public GameObject flyingSaucerLockedImage;
        public TextMeshProUGUI flyingSaucerLevelToUnlockText;
        public FlyingSaucerMenuItem selectedItem;
        public GameObject itemPrice;
        public GameObject itemPriceMask;
        public TextMeshProUGUI itemPriceText;
        public GameObject flyingSaucerMask;
        public GameObject itemPriceButton;
        public GameObject popDownPrice;
        public TextMeshProUGUI popDownPriceText;
    }
    [SerializeField] private WindowFlyingSaucer windowFlyingSaucer;


    [System.Serializable]
    private struct CowsArea
    {       
        public GameObject cowItemPrefab;
        public GameObject contentArea;
    }
    [SerializeField] private CowsArea cowsArea;

    [System.Serializable]
    private struct WindowCow
    {
        public GameObject window;
        public Animator windowAnimator;
        public GameObject cowModelLocation;
        public GameObject lockedImage;
        public TextMeshProUGUI lockedLevelText;
        public GameObject itemPrice;
        public GameObject itemPriceMask;
        public TextMeshProUGUI itemPriceText;
        public GameObject cowMask;
        public GameObject itemPriceButton;
        public GameObject popDownPrice;
        public TextMeshProUGUI popDownPriceText;
    }
    [SerializeField] private WindowCow windowCow;


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
        windowFlyingSaucer.window.SetActive(false);
        windowCow.window.SetActive(false);
        PlayMenuPanel_Coins.SetText(GameControl.Instance.playerData.currentCoins.ToString());
        LoadAndDisplayFlyingSaucer();
        if (!buildedCowsArea) 
        {
            buildedCowsArea = true;
            BuildCowArea();
        }
        
        //Update 28/10/2022 - Ivo Seitenfus
        //foreach (GameObject obj in PlayMenuPanel_Upgrades)
        //{
        //    obj.GetComponent<Upgrade>().SetupUpgrade();
        //}
        //int i = 0;
        //foreach(GameObject obj in PlayMenuPanel_CowCounter)
        //{
        //    TextMeshProUGUI counter = obj.transform.Find("Counter").GetComponent<TextMeshProUGUI>();
        //    //switch(i)
        //    //{
        //    //    case 0: 
        //    //        counter.SetText(GameControl.Instance.playerData.numCow01.ToString());
        //    //        break;
        //    //    case 1:
        //    //        counter.SetText(GameControl.Instance.playerData.numCow02.ToString());
        //    //        break;
        //    //    case 2:
        //    //        counter.SetText(GameControl.Instance.playerData.numCow03.ToString());
        //    //        break;
        //    //    case 3:
        //    //        counter.SetText(GameControl.Instance.playerData.numCow04.ToString());
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}
        //    counter.SetText(GameControl.Instance.playerData.numCows[i].ToString());
        //    i++;
        //}
    }

    void LoadAndDisplayFlyingSaucer()
    {
        currentFlyingsaucer = GameControl.Instance.playerData.flyingSaucerModelId;
        if(flyingSaucerDisplayLocation.transform.childCount > 0)
        {
            Destroy(flyingSaucerDisplayLocation.transform.GetChild(0).gameObject);
        }
        Instantiate(GameControl.Instance.FlyingSaucerModelByIndex(currentFlyingsaucer), flyingSaucerDisplayLocation.transform);
        VerifyFlyingSaucerAvailability();
    }

    public void LoadAndDisplayNextFlyingSaucer()
    {
        Destroy(flyingSaucerDisplayLocation.transform.GetChild(0).gameObject);
        currentFlyingsaucer++;
        if (currentFlyingsaucer == GameControl.Instance.FlyingSaucersNumber()) currentFlyingsaucer = 0;
        Instantiate(GameControl.Instance.FlyingSaucerModelByIndex(currentFlyingsaucer), flyingSaucerDisplayLocation.transform);
        VerifyFlyingSaucerAvailability();
    }

    public void LoadAndDisplayPreviousFlyingSaucer()
    {
        Destroy(flyingSaucerDisplayLocation.transform.GetChild(0).gameObject);
        currentFlyingsaucer--;
        if (currentFlyingsaucer == -1) currentFlyingsaucer = GameControl.Instance.FlyingSaucersNumber() - 1;
        Instantiate(GameControl.Instance.FlyingSaucerModelByIndex(currentFlyingsaucer), flyingSaucerDisplayLocation.transform);
        VerifyFlyingSaucerAvailability();
    }

    void VerifyFlyingSaucerAvailability()
    {
        
        flyingSaucerLockedImage.SetActive(false);
        flyingSaucerSelectButtonImage.gameObject.SetActive(true);
        flyingSaucerPrice.SetActive(false);
        flyingSaucerTickConfirm.SetActive(false);
        //if(currentFlyingsaucer == GameControl.Instance.playerData.flyingSaucerModelId)
        //{
        //    flyingSaucerSelectButtonImage.color = flyingSaucerSelectedColor;
        //    flyingSaucerSelectButtonText.SetText("Selecionado");
        //    tickConfirm.SetActive(true);

        //}
        //else
        //{
        //    flyingSaucerSelectButtonImage.color = flyingSaucerUnselectedColor;
        //    flyingSaucerSelectButtonText.SetText("Selecionar");
        //    tickConfirm.SetActive(false);
        //    if(!GameControl.Instance.IsFlyingSaucerUnlocked(currentFlyingsaucer))
        //    {
        //        flyingSaucerLockedImage.SetActive(true);
        //        flyingSaucerLevelToUnlockText.SetText("Level " + GameControl.Instance.LeveltoUnlockFlyingSaucer(currentFlyingsaucer).ToString());
        //    }
        //}
        FlyingSaucerData flyingSaucerData = GameControl.Instance.FlyingSaucerDataByIndex(currentFlyingsaucer);

        if (!GameControl.Instance.IsFlyingSaucerUnlocked(currentFlyingsaucer))
        {
            flyingSaucerLockedImage.SetActive(true);
            flyingSaucerLevelToUnlockText.SetText("Level " + flyingSaucerData.levelToUnlock.ToString());
        }
        else
        {
            if (!GameControl.Instance.playerData.purchasedFlyingSaucerModels.Contains(currentFlyingsaucer) /*&& (flyingSaucerData.value != 0)*/)
            {
                
                flyingSaucerPrice.SetActive(true);
                flyingSaucerPriceText.SetText(flyingSaucerData.value.ToString());                              
                flyingSaucerSelectButtonImage.gameObject.SetActive(false);
            }
            else if (currentFlyingsaucer == GameControl.Instance.playerData.flyingSaucerModelId)
            {
                flyingSaucerSelectButtonImage.color = flyingSaucerSelectedColor;
                flyingSaucerSelectButtonText.SetText("Selecionado");
                flyingSaucerTickConfirm.SetActive(true);
            }
            else
            {
                flyingSaucerSelectButtonImage.color = flyingSaucerUnselectedColor;
                flyingSaucerSelectButtonText.SetText("Selecionar");
                

            }
        }



    }

    void BuildFlyingSaucerWindow()
    {
        for(int i = 0; i < GameControl.Instance.FlyingSaucersNumber(); i++)
        {
            FlyingSaucerMenuItem flyingSaucerItem = Instantiate(flyingSaucerWindowItemPrefab, flyingSaucerListContentView.transform).GetComponent<FlyingSaucerMenuItem>();
            flyingSaucerItem.InitalizeItem(i, this);
            if (i == currentFlyingsaucer) 
            {
                windowFlyingSaucer.selectedItem = flyingSaucerItem;
                flyingSaucerItem.SelectItem();            
            }
            
        }
    }

    public void WindowFlyingSaucerSelectItem(FlyingSaucerMenuItem item)
    {
        Destroy(flyingSaucerDisplayLocation.transform.GetChild(0).gameObject);
        currentFlyingsaucer = item.Id;
        currentItem = item;
        Instantiate(GameControl.Instance.FlyingSaucerModelByIndex(currentFlyingsaucer), flyingSaucerDisplayLocation.transform);

        VerifyFlyingSaucerAvailability();


        WindowFlyingSaucerVerifyAvailability(item);


    }

    void WindowFlyingSaucerVerifyAvailability(FlyingSaucerMenuItem item)
    {
        windowFlyingSaucer.itemPrice.SetActive(false);
        windowFlyingSaucer.popDownPrice.SetActive(false);
        windowFlyingSaucer.flyingSaucerLockedImage.SetActive(false);
        windowFlyingSaucer.flyingSaucerSelectButtonImage.gameObject.SetActive(true);
        windowFlyingSaucer.tickConfirm.SetActive(false);
        if (item != windowFlyingSaucer.selectedItem)
        {
            windowFlyingSaucer.selectedItem.DeselectItem();
            windowFlyingSaucer.selectedItem = item;

        }
        FlyingSaucerData itemData = GameControl.Instance.FlyingSaucerDataByIndex(item.Id);

        if (!GameControl.Instance.IsFlyingSaucerUnlocked(itemData.id))
        {
            windowFlyingSaucer.flyingSaucerLockedImage.SetActive(true);
            windowFlyingSaucer.flyingSaucerLevelToUnlockText.SetText("Level " + itemData.levelToUnlock);
        }
        else
        {
            if (!GameControl.Instance.playerData.purchasedFlyingSaucerModels.Contains(item.Id) /* && (itemData.value != 0)*/)
            {
                windowFlyingSaucer.itemPrice.SetActive(true);               
                windowFlyingSaucer.itemPriceText.SetText(itemData.value.ToString());
                if (GameControl.Instance.playerData.currentCoins >= itemData.value)
                {
                    windowFlyingSaucer.itemPriceMask.SetActive(false);
                }
                windowFlyingSaucer.flyingSaucerSelectButtonImage.gameObject.SetActive(false);
            }
            else if (currentFlyingsaucer == GameControl.Instance.playerData.flyingSaucerModelId)
            {
                windowFlyingSaucer.flyingSaucerSelectButtonImage.color = flyingSaucerSelectedColor;
                windowFlyingSaucer.flyingSaucerSelectButtonText.SetText("Selecionado");
                windowFlyingSaucer.tickConfirm.SetActive(true);
            }
            else
            {                
                windowFlyingSaucer.flyingSaucerSelectButtonImage.color = flyingSaucerUnselectedColor;
                windowFlyingSaucer.flyingSaucerSelectButtonText.SetText("Selecionar");
                
                
            }
        }
    }

    public void OpenFlyingSaucerWindow()
    {
        windowFlyingSaucer.window.SetActive(true);
        BuildFlyingSaucerWindow();
        windowFlyingSaucer.windowAnimator.SetBool("Open", true);
        windowOpen = windowFlyingSaucer.window;
    }

    public void CloseFlyingSaucerWindow()
    {        
        windowFlyingSaucer.windowAnimator.SetBool("Open", false);
    }

    public void FinishCloseWindow()
    {
        //windowFlyingSaucer.window.SetActive(false);
        windowOpen.SetActive(false);
        ClearFlyingSaucerWindow();     
    }
    void ClearFlyingSaucerWindow()
    {
        
        int childCount = flyingSaucerListContentView.transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            Destroy(flyingSaucerListContentView.transform.GetChild(i).gameObject);
        }
        
    }

    public void SelectFlyingSaucer()
    {

        GameControl.Instance.SelectFlyingSaucer(currentFlyingsaucer);
        flyingSaucerSelectButtonImage.color = flyingSaucerSelectedColor;
        flyingSaucerSelectButtonText.SetText("Selecionado");
        flyingSaucerTickConfirm.SetActive(true);

        windowFlyingSaucer.flyingSaucerSelectButtonImage.color = flyingSaucerSelectedColor;
        windowFlyingSaucer.flyingSaucerSelectButtonText.SetText("Selecionado");
        windowFlyingSaucer.tickConfirm.SetActive(true);
    }

    public void PurchaseFlyingSaucer()
    {
        GameControl.Instance.PurchaseFlyingSaucer(currentFlyingsaucer);
        StartCoroutine("PlayPurchaseFlyingSaucerAnimation");
        
    }
    
    IEnumerator PlayPurchaseFlyingSaucerAnimation()
    {
        windowFlyingSaucer.flyingSaucerMask.GetComponent<Animation>().Play();
        windowFlyingSaucer.itemPriceButton.GetComponent<Animation>().Play();
        windowFlyingSaucer.popDownPrice.SetActive(true);
        windowFlyingSaucer.popDownPrice.GetComponent<Animation>().Play();
        windowFlyingSaucer.popDownPriceText.SetText(GameControl.Instance.FlyingSaucerDataByIndex(currentFlyingsaucer).value.ToString());
        yield return new WaitForSeconds(1.5f);
        windowFlyingSaucer.itemPrice.SetActive(false);
        windowFlyingSaucer.flyingSaucerMask.GetComponent<Animation>().Rewind();
        windowFlyingSaucer.flyingSaucerMask.GetComponent<Animation>().Play();
        windowFlyingSaucer.flyingSaucerMask.GetComponent<Animation>().Sample();
        windowFlyingSaucer.flyingSaucerMask.GetComponent<Animation>().Stop();
        windowFlyingSaucer.itemPriceButton.GetComponent<Animation>().Rewind();
        windowFlyingSaucer.itemPriceButton.GetComponent<Animation>().Play();
        windowFlyingSaucer.itemPriceButton.GetComponent<Animation>().Sample();
        windowFlyingSaucer.itemPriceButton.GetComponent<Animation>().Stop();
        windowFlyingSaucer.popDownPrice.GetComponent<Animation>().Rewind();
        windowFlyingSaucer.popDownPrice.GetComponent<Animation>().Play();
        windowFlyingSaucer.popDownPrice.GetComponent<Animation>().Sample();
        windowFlyingSaucer.popDownPrice.GetComponent<Animation>().Stop();

        VerifyFlyingSaucerAvailability();
        WindowFlyingSaucerVerifyAvailability(currentItem);
        currentItem.purchasedItem();
    }

    public void PurchaseCow()
    {
        GameControl.Instance.PurchaseCow(currentCow.Id);
        StartCoroutine("PlayPurchaseCowAnimation");

    }

    IEnumerator PlayPurchaseCowAnimation()
    {
        windowCow.cowMask.GetComponent<Animation>().Play();
        windowCow.itemPriceButton.GetComponent<Animation>().Play();
        windowCow.popDownPrice.SetActive(true);
        windowCow.popDownPrice.GetComponent<Animation>().Play();
        windowCow.popDownPriceText.SetText(currentCow.ItemCowData().value.ToString());
        yield return new WaitForSeconds(1.5f);
        windowCow.itemPrice.SetActive(false);
        windowCow.popDownPrice.SetActive(false);
        windowCow.cowMask.GetComponent<Animation>().Rewind();
        windowCow.cowMask.GetComponent<Animation>().Play();
        windowCow.cowMask.GetComponent<Animation>().Sample();
        windowCow.cowMask.GetComponent<Animation>().Stop();
        windowCow.itemPriceButton.GetComponent<Animation>().Rewind();
        windowCow.itemPriceButton.GetComponent<Animation>().Play();
        windowCow.itemPriceButton.GetComponent<Animation>().Sample();
        windowCow.itemPriceButton.GetComponent<Animation>().Stop();
        windowCow.popDownPrice.GetComponent<Animation>().Rewind();
        windowCow.popDownPrice.GetComponent<Animation>().Play();
        windowCow.popDownPrice.GetComponent<Animation>().Sample();
        windowCow.popDownPrice.GetComponent<Animation>().Stop();
    }

    void BuildCowArea()
    {
        for (int i = 0; i < GameControl.Instance.cowsNumber(); i++)
        {
            CowMenuItem cowItem = Instantiate(cowsArea.cowItemPrefab, cowsArea.contentArea.transform).GetComponent<CowMenuItem>();
            cowItem.InitalizeItem(i, this);            

        }
    }

    public void UpdateCows()
    {
        for(int i = 0; i < cowsArea.contentArea.transform.childCount; i++)
        {
            CowMenuItem _cowItem = cowsArea.contentArea.transform.GetChild(i).gameObject.GetComponent<CowMenuItem>();
            _cowItem.UpdateCow(i);
        }
    }



    public void ShowModalPurchase(Upgrade _upgrade)
    {
        upgradeToPurchase = _upgrade;
        modalConfirmPurchase.SetActive(true);
        modalIcon.sprite = upgradeToPurchase.GetUpgradeIcon();
        modalTextValue.SetText(upgradeToPurchase.GetUpgradeValue().ToString());
        modalLevelIcon.sprite = upgradeToPurchase.GetPurchaseButtonIcon().sprite;
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

    public void CowAreaSelectItem(CowMenuItem cow)
    {
        currentCow = cow;
        windowOpen = windowCow.window;
        windowOpen.SetActive(true);                
        windowCow.windowAnimator.SetBool("Open", true);
        CowData m_cow = cow.ItemCowData();        
        Instantiate(m_cow.menuPrefab, windowCow.cowModelLocation.transform);
        BuildCowWindow(m_cow);
    }

    void BuildCowWindow(CowData cow)
    {
        windowCow.itemPrice.SetActive(false);
        windowCow.popDownPrice.SetActive(false);
        windowCow.lockedImage.SetActive(false);
        windowCow.itemPriceMask.SetActive(true);

        if (!GameControl.Instance.IsCowUnlocked(cow.id))
        {
            windowCow.lockedImage.SetActive(true);
            windowCow.lockedLevelText.SetText("Level " + cow.levelToUnlock);
        }
        else
        {
            if (!GameControl.Instance.playerData.purchasedCows.Contains(cow.id) /* && (itemData.value != 0)*/)
            {
                windowCow.itemPrice.SetActive(true);
                windowCow.itemPriceText.SetText(cow.value.ToString());
                if (GameControl.Instance.playerData.currentCoins >= cow.value)
                {
                    windowCow.itemPriceMask.SetActive(false);
                }
                
            }            
        }
    }


    public void CloseCowWindow()
    {
        windowCow.windowAnimator.SetBool("Open", false);
        Destroy(windowCow.cowModelLocation.transform.GetChild(0).gameObject, 1f);
    }

}
