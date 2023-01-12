using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gamob
{
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
        //[SerializeField] private GameObject adsIconRandomPlayButton;
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
        private StageMenuItem currentStage;
        private bool buildedContentAreas = false;
        private GameObject windowOpen;
        private bool levelUp;

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
            public TextMeshProUGUI coinText;
            public TextMeshProUGUI xpText;
        }
        [SerializeField] private WindowCow windowCow;


        [System.Serializable]
        private struct StagesArea
        {
            public GameObject stageItemPrefab;
            public GameObject contentArea;
        }
        [SerializeField] private StagesArea stagesArea;

        [System.Serializable]
        private struct WindowStage
        {
            public GameObject window;
            public Animator windowAnimator;
            public Image stageImage;
            public TextMeshProUGUI stageName;
            public GameObject lockedImage;
            public TextMeshProUGUI lockedLevelText;
            public GameObject itemPrice;
            public GameObject itemPriceMask;
            public TextMeshProUGUI itemPriceText;
            public GameObject itemMask;
            public GameObject itemPriceButton;
            public GameObject popDownPrice;
            public TextMeshProUGUI popDownPriceText;
            public GameObject playButton;
            public GameObject adsImage;
            public TextMeshProUGUI xpText;
            public GameObject adsIconOnPlayButton;
        }
        [SerializeField] private WindowStage windowStage;

        [System.Serializable]
        private struct WindowLevelUp
        {
            public GameObject unlockedItemPrefab;
            public GameObject speedItemPrefab;
            public GameObject raySizeItemPrefab;
            public GameObject rayMultiplierItemPrefab;
            public GameObject window;
            public Animator windowAnimator;
            public TextMeshProUGUI levelText;
            public GameObject unlockedItemsArea;
            public GameObject upgradeItemArea;
            public bool m_speedItem;
            public bool m_raySizeItem;
            public bool m_rayMultiplierItem;
        }
        [SerializeField] private WindowLevelUp windowLevelUp;

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
            //AdsIconOnPlayRandom();
            IntroPanel.SetActive(false);
            PlayMenuPanel.SetActive(true);
            windowFlyingSaucer.window.SetActive(false);
            windowCow.window.SetActive(false);
            windowStage.window.SetActive(false);
            windowLevelUp.window.SetActive(false);
            PlayMenuPanel_Coins.SetText(GameControl.Instance.playerData.currentCoins.ToString());
            LoadAndDisplayFlyingSaucer();
            if (!buildedContentAreas)
            {
                buildedContentAreas = true;
                BuildCowArea();
                BuildStagesArea();
            }
            if (levelUp)
            {

                SetupLevelUpWindow();
                StartCoroutine(OpenLevelUpWindow());
                levelUp = false;
            }
        }

        void LoadAndDisplayFlyingSaucer()
        {
            currentFlyingsaucer = GameControl.Instance.playerData.flyingSaucerModelId;
            if (flyingSaucerDisplayLocation.transform.childCount > 0)
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
            for (int i = 0; i < GameControl.Instance.FlyingSaucersNumber(); i++)
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
            for (int i = 0; i < childCount; i++)
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
            currentItem.purchasedItem();
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
            for (int i = 0; i < GameControl.Instance.CowsNumber(); i++)
            {
                CowMenuItem cowItem = Instantiate(cowsArea.cowItemPrefab, cowsArea.contentArea.transform).GetComponent<CowMenuItem>();
                //cowItem.InitalizeItem(i, this);            
                cowItem.InitalizeItem(CowAreaSelectItem, i);

            }
        }



        void BuildStagesArea()
        {
            for (int i = 0; i < GameControl.Instance.StagesNumber(); i++)
            {
                StageMenuItem stageItem = Instantiate(stagesArea.stageItemPrefab, stagesArea.contentArea.transform).GetComponent<StageMenuItem>();
                //stageItem.InitalizeItem(i, this);
                stageItem.InitalizeItem(StagesAreaSelectItem, i);

            }
        }

        public void UpdateCows()
        {
            for (int i = 0; i < cowsArea.contentArea.transform.childCount; i++)
            {
                CowMenuItem _cowItem = cowsArea.contentArea.transform.GetChild(i).gameObject.GetComponent<CowMenuItem>();
                _cowItem.UpdateCow(i);
            }
        }

        public void UpdateStages()
        {
            for (int i = 0; i < stagesArea.contentArea.transform.childCount; i++)
            {
                StageMenuItem _stageItem = stagesArea.contentArea.transform.GetChild(i).gameObject.GetComponent<StageMenuItem>();
                _stageItem.UpdateStage(i);
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
            GameControl.Instance.AudioManager().OpenWindow();
        }

        void BuildCowWindow(CowData cow)
        {
            windowCow.itemPrice.SetActive(false);
            windowCow.popDownPrice.SetActive(false);
            windowCow.lockedImage.SetActive(false);
            windowCow.itemPriceMask.SetActive(true);
            windowCow.coinText.SetText(cow.reward.ToString());
            windowCow.xpText.SetText(cow.xp.ToString());

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


        public void StagesAreaSelectItem(StageMenuItem _stage)
        {
            currentStage = _stage;
            windowOpen = windowStage.window;
            windowOpen.SetActive(true);
            windowStage.windowAnimator.SetBool("Open", true);
            StageData m_stage = currentStage.ItemStageData();
            BuildStageWindow(m_stage);
            GameControl.Instance.AudioManager().OpenWindow();
        }

        void BuildStageWindow(StageData _stage)
        {
            windowStage.itemPrice.SetActive(false);
            windowStage.popDownPrice.SetActive(false);
            windowStage.lockedImage.SetActive(false);
            windowStage.itemPriceMask.SetActive(true);
            windowStage.playButton.SetActive(false);
            windowStage.stageImage.sprite = _stage.image;
            windowStage.stageName.SetText("Cenário " + _stage.nameID);
            windowStage.xpText.SetText(_stage.xp.ToString());

            if (!GameControl.Instance.IsStageUnlocked(_stage.id))
            {
                windowStage.lockedImage.SetActive(true);
                windowStage.lockedLevelText.SetText("Level " + _stage.levelToUnlock);
            }
            else
            {
                if (!GameControl.Instance.playerData.purchasedStages.Contains(_stage.id) /* && (itemData.value != 0)*/)
                {
                    windowStage.itemPrice.SetActive(true);
                    windowStage.itemPriceText.SetText(_stage.value.ToString());
                    if (GameControl.Instance.playerData.currentCoins >= _stage.value)
                    {
                        windowStage.itemPriceMask.SetActive(false);
                    }

                }
                else
                {
                    windowStage.playButton.SetActive(true);
                    //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<AdsManager>().interstitialLoaded);
                    //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<Advertisements>().interstitialLoaded);
                    AdsIconOnStagePlay();
                }
            }
        }

        public void CloseStageWindow()
        {
            windowStage.windowAnimator.SetBool("Open", false);
        }

        public void PurchaseStage()
        {
            GameControl.Instance.PurchaseStage(currentStage.Id);
            StartCoroutine("PlayPurchaseStageAnimation");
        }

        IEnumerator PlayPurchaseStageAnimation()
        {
            windowStage.itemMask.GetComponent<Animation>().Play();
            windowStage.itemPriceButton.GetComponent<Animation>().Play();
            windowStage.popDownPrice.SetActive(true);
            windowStage.popDownPrice.GetComponent<Animation>().Play();
            windowStage.popDownPriceText.SetText(currentStage.ItemStageData().value.ToString());
            yield return new WaitForSeconds(1.5f);
            windowStage.itemPrice.SetActive(false);
            windowStage.popDownPrice.SetActive(false);
            windowStage.itemMask.GetComponent<Animation>().Rewind();
            windowStage.itemMask.GetComponent<Animation>().Play();
            windowStage.itemMask.GetComponent<Animation>().Sample();
            windowStage.itemMask.GetComponent<Animation>().Stop();
            windowStage.itemPriceButton.GetComponent<Animation>().Rewind();
            windowStage.itemPriceButton.GetComponent<Animation>().Play();
            windowStage.itemPriceButton.GetComponent<Animation>().Sample();
            windowStage.itemPriceButton.GetComponent<Animation>().Stop();
            windowStage.popDownPrice.GetComponent<Animation>().Rewind();
            windowStage.popDownPrice.GetComponent<Animation>().Play();
            windowStage.popDownPrice.GetComponent<Animation>().Sample();
            windowStage.popDownPrice.GetComponent<Animation>().Stop();
            windowStage.playButton.SetActive(true);
            //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<AdsManager>().interstitialLoaded);
            //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<Advertisements>().interstitialLoaded);
            AdsIconOnStagePlay();
            windowStage.playButton.GetComponent<Animation>().Rewind();
            windowStage.playButton.GetComponent<Animation>().Play();
        }

        public void LevelUp(bool _speed, bool _raySize, bool _rayMultiplier)
        {
            levelUp = true;
            windowLevelUp.m_rayMultiplierItem = _rayMultiplier;
            windowLevelUp.m_raySizeItem = _raySize;
            windowLevelUp.m_speedItem = _speed;
        }

        public int CurrentStage()
        {
            return currentStage.Id;
        }

        public void InstertitialAdsLoaded()
        {
            EnableStageLoadButton();
            //AdsIconOnPlayRandom();
            AdsIconOnStagePlay();
        }

        void AdsIconOnStagePlay()
        {
            if (GameControl.Instance.CanShowAdsIcon()) ShowAdsIconOnPlayStage();
            else HideAdsIconOnPlayStage();
        }

        //public void AdsIconOnPlayRandom()
        //{
        //    if (GameControl.Instance.CanShowAdsIcon()) ShowAdsIconOnRandomPlay();
        //    else HideAdsIconOnRandomPlay();
        //}

        void EnableStageLoadButton()
        {
            //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<AdsManager>().interstitialLoaded);
            //windowStage.playButton.GetComponent<InteractableInterface>().SetInteractable(GameControl.Instance.gameObject.GetComponent<Advertisements>().interstitialLoaded);
        }

        public void CloseLevelUpWindow()
        {
            windowLevelUp.windowAnimator.SetBool("Open", false);
        }

        public void DisableLevelUpWindow()
        {
            ClearLevelUpWindow();
            windowLevelUp.window.SetActive(false);
        }

        IEnumerator OpenLevelUpWindow()
        {
            yield return new WaitForSeconds(2);
            GameControl.Instance.AudioManager().LevelUpWindow();
            windowLevelUp.window.SetActive(true);
            windowLevelUp.windowAnimator.SetBool("Open", true);
        }



        void SetupLevelUpWindow()
        {
            int playerLevel = GameControl.Instance.playerData.level;
            int flyingSaucersNumber = GameControl.Instance.FlyingSaucersNumber();
            int cowsNumber = GameControl.Instance.CowsNumber();
            int stagesNumber = GameControl.Instance.StagesNumber();
            windowLevelUp.levelText.SetText(playerLevel.ToString());
            for (int i = 0; i < flyingSaucersNumber; i++)
            {
                FlyingSaucerData data = GameControl.Instance.FlyingSaucerDataByIndex(i);
                if (data.levelToUnlock == playerLevel)
                {
                    GameObject item = Instantiate(windowLevelUp.unlockedItemPrefab, windowLevelUp.unlockedItemsArea.transform);
                    item.GetComponent<Image>().sprite = data.icon;
                }
            }

            for (int i = 0; i < cowsNumber; i++)
            {
                CowData data = GameControl.Instance.CowByIndex(i);
                if (data.levelToUnlock == playerLevel)
                {
                    GameObject item = Instantiate(windowLevelUp.unlockedItemPrefab, windowLevelUp.unlockedItemsArea.transform);
                    item.GetComponent<Image>().sprite = data.icon;
                }
            }

            for (int i = 0; i < stagesNumber; i++)
            {
                StageData data = GameControl.Instance.StageDatabyIndex(i);
                if (data.levelToUnlock == playerLevel)
                {
                    GameObject item = Instantiate(windowLevelUp.unlockedItemPrefab, windowLevelUp.unlockedItemsArea.transform);
                    item.GetComponent<Image>().sprite = data.image;
                }
            }

            if (windowLevelUp.m_speedItem)
            {
                GameObject item = Instantiate(windowLevelUp.speedItemPrefab, windowLevelUp.upgradeItemArea.transform);
                int speedValue = 1 + GameControl.Instance.playerData.playerSpeed;
                item.GetComponentInChildren<TextMeshProUGUI>().SetText(speedValue.ToString());
            }

            if (windowLevelUp.m_rayMultiplierItem)
            {
                GameObject item = Instantiate(windowLevelUp.rayMultiplierItemPrefab, windowLevelUp.upgradeItemArea.transform);
                int rayMultiplierValue = 1 + GameControl.Instance.playerData.rayMultiplier;
                item.GetComponentInChildren<TextMeshProUGUI>().SetText(rayMultiplierValue.ToString());
            }

            if (windowLevelUp.m_raySizeItem)
            {
                GameObject item = Instantiate(windowLevelUp.raySizeItemPrefab, windowLevelUp.upgradeItemArea.transform);
                float raySizeValue = 1 + GameControl.Instance.playerData.rayRadius;
                item.GetComponentInChildren<TextMeshProUGUI>().SetText(raySizeValue.ToString());
            }
        }

        void ClearLevelUpWindow()
        {
            int childCount = windowLevelUp.unlockedItemsArea.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(windowLevelUp.unlockedItemsArea.transform.GetChild(i).gameObject);
            }

            childCount = windowLevelUp.upgradeItemArea.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(windowLevelUp.upgradeItemArea.transform.GetChild(i).gameObject);
            }
            windowLevelUp.m_rayMultiplierItem = windowLevelUp.m_raySizeItem = windowLevelUp.m_speedItem = false;

        }

        //void ShowAdsIconOnRandomPlay()
        //{
        //    adsIconRandomPlayButton.SetActive(true);
        //}

        //void HideAdsIconOnRandomPlay()
        //{
        //    adsIconRandomPlayButton.SetActive(false);
        //}

        void ShowAdsIconOnPlayStage()
        {
            //adsIconRandomPlayButton.SetActive(true);
            windowStage.adsIconOnPlayButton.SetActive(true);
        }

        void HideAdsIconOnPlayStage()
        {
            //adsIconRandomPlayButton.SetActive(false);
            windowStage.adsIconOnPlayButton.SetActive(false);
        }

        //public void TestLevelUpWindow()
        //{
        //    SetupLevelUpWindow();
        //    StartCoroutine(OpenLevelUpWindow());
        //}
    }
}