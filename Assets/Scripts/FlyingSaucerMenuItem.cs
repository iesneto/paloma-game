using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


namespace Gamob
{
    public class FlyingSaucerMenuItem : MonoBehaviour, IPointerClickHandler
    {
        public int Id { get; set; }
        [SerializeField] private Image itemFrame;
        [SerializeField] private Sprite itemSelected;
        [SerializeField] private Sprite itemDeselected;
        [SerializeField] private Color selected;
        [SerializeField] private Color deselected;
        [SerializeField] private Image itemIcon;
        [SerializeField] private GameObject coin;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private GameObject lockedImage;
        [SerializeField] private TextMeshProUGUI lockedLevelText;
        private MainMenu mainMenuReference;

        public void InitalizeItem(int index, MainMenu reference)
        {
            mainMenuReference = reference;
            Id = index;
            itemFrame.sprite = itemDeselected;
            itemFrame.color = deselected;
            FlyingSaucerData flyingSaucer = GameControl.Instance.FlyingSaucerDataByIndex(index);
            itemIcon.sprite = flyingSaucer.icon;
            coin.SetActive(false);
            if (GameControl.Instance.playerData.level < flyingSaucer.levelToUnlock)
            {
                lockedImage.SetActive(true);
                lockedLevelText.SetText("Lv. " + flyingSaucer.levelToUnlock);
            }
            else
            {
                lockedImage.SetActive(false);
                if (!GameControl.Instance.playerData.purchasedFlyingSaucerModels.Contains(index))
                {
                    coin.SetActive(true);
                    coinText.SetText(flyingSaucer.value.ToString());
                }
            }
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            SelectItem();
            GameControl.Instance.AudioManager().PlayMenuFlyingSaucerNavigationButton();
        }

        public void SelectItem()
        {
            itemFrame.sprite = itemSelected;
            itemFrame.color = selected;
            mainMenuReference.WindowFlyingSaucerSelectItem(this);

        }

        public void DeselectItem()
        {
            itemFrame.sprite = itemDeselected;
            itemFrame.color = deselected;
        }

        public void purchasedItem()
        {
            coin.SetActive(false);
        }
    }
}