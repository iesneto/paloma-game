using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Gamob
{
    public class Upgrade : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UpgradeData upgradeData;
        [SerializeField] private int currentLevel;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private List<Image> stars;
        //[SerializeField] private Text valueShadow;
        [SerializeField] private TextMeshProUGUI value;
        [SerializeField] private Image mask;
        [SerializeField] private Color colorOn;
        [SerializeField] private Color maskColorOn;
        [SerializeField] private Color maskColorOff;
        [SerializeField] private Button upgradeButton;
        //[SerializeField] private MainMenu mainMenu;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameObject maxLevelLabel;
        [SerializeField] private GameObject levelIconsArea;
        [SerializeField] private GameObject levelIconPrefab;
        [SerializeField] private Image purchaseButtonIcon;

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void SetupUpgrade()
        {
            icon.sprite = upgradeData.icon;

            SetCurrentLevel();
            SetLanguage();
            if (levelIconsArea.transform.childCount == 0)
            {
                AddLevelIconsPrefabs();
            }
            LightOnLevelIcons();
            SetPurchaseButton();

        }

        private void SetCurrentLevel()
        {
            switch (upgradeData.type)
            {
                case UpgradeData.ControlVar.RAYRADIUS:
                    currentLevel = GameControl.Instance.playerData.rayRadius;
                    break;
                case UpgradeData.ControlVar.RAYFORCE:
                    currentLevel = GameControl.Instance.playerData.rayForce;
                    break;
                case UpgradeData.ControlVar.RAYMULTIPLIER:
                    currentLevel = GameControl.Instance.playerData.rayMultiplier;
                    break;
                case UpgradeData.ControlVar.PLAYERSPEED:
                    currentLevel = GameControl.Instance.playerData.playerSpeed;
                    break;
                case UpgradeData.ControlVar.PLAYERENERGY:
                    currentLevel = GameControl.Instance.playerData.playerEnergy;
                    break;
                case UpgradeData.ControlVar.PLAYERENERGYCONSUME:
                    currentLevel = GameControl.Instance.playerData.playerEnergyConsume;
                    break;
                default:
                    break;
            }
        }

        private void SetLanguage()
        {
            switch (GameControl.Instance.playerData.language)
            {
                case SystemLanguage.Portuguese:
                    description.SetText(upgradeData.title[0]);
                    break;
                default:
                    description.SetText(upgradeData.title[1]);
                    break;
            }
        }

        private void AddLevelIconsPrefabs()
        {
            for (int i = 0; i < upgradeData.value.Length; i++)
            {
                var child = Instantiate(levelIconPrefab);
                child.transform.SetParent(levelIconsArea.transform);
                Image childImage = child.GetComponent<Image>();
                stars.Add(childImage);
                if (upgradeData.valueIcon.Length > 1)
                {
                    childImage.sprite = upgradeData.valueIcon[i];
                }
                else
                {
                    childImage.sprite = upgradeData.valueIcon[0];
                }
            }
        }


        private void LightOnLevelIcons()
        {
            for (int i = 0; i < currentLevel; i++)
            {
                if (stars.Count > i)
                {
                    stars[i].color = colorOn;
                }

            }
        }

        public void SetPurchaseButton()
        {
            if (upgradeData.valueIcon.Length > 1 && currentLevel < upgradeData.valueIcon.Length)
            {
                purchaseButtonIcon.sprite = upgradeData.valueIcon[currentLevel];
            }
            else
            {
                purchaseButtonIcon.sprite = upgradeData.valueIcon[0];
            }

            if (currentLevel >= upgradeData.value.Length) maxLevelLabel.SetActive(true);
            else
            {
                value.SetText(upgradeData.value[currentLevel].ToString());
                // valueShadow.text = value.text;

                if (GameControl.Instance.playerData.currentCoins >= upgradeData.value[currentLevel])
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
            // mainMenu.ShowModalPurchase(this);
            uiManager.MainMenuShowModalPurchase(this);
        }

        public void PurchaseUpgrade()
        {
            GameControl.Instance.PurchaseUpgrade(upgradeData.type, upgradeData.value[currentLevel]);
        }

        public Sprite GetUpgradeIcon()
        {
            return upgradeData.icon;
        }

        public long GetUpgradeValue()
        {
            return upgradeData.value[currentLevel];
        }

        public Image GetPurchaseButtonIcon()
        {
            return purchaseButtonIcon;
        }
    }
}