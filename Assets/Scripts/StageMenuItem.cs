using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Gamob
{
    public class StageMenuItem : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler //,IPointerUpHandler
    {

        public int Id { get; set; }
        [SerializeField] private RectTransform myTransform;
        //[SerializeField] private Image itemFrame;
        //[SerializeField] private Sprite itemSelected;
        //[SerializeField] private Sprite itemDeselected;
        //[SerializeField] private Color selected;
        //[SerializeField] private Color deselected;
        //[SerializeField] private Image itemIcon;
        [SerializeField] private GameObject coin;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private GameObject lockedImage;
        [SerializeField] private TextMeshProUGUI lockedLevelText;
        [SerializeField] private TextMeshProUGUI nameText;
        //private MainMenu mainMenuReference;
        public delegate void stageItemMenuDelegate(StageMenuItem _stageMenuItem);
        private stageItemMenuDelegate m_delegate;
        StageData stage;
        private float currentScale = 1f;
        private float maxScale = 1f;
        private float minScale = 0.5f;
        private float scaleSpeed = 4f;
        private bool up, down, exit;

        //public void InitalizeItem(int index, MainMenu reference)
        public void InitalizeItem(stageItemMenuDelegate _method, int index)
        {
            //mainMenuReference = reference;
            m_delegate = _method;
            Id = index;
            stage = GameControl.Instance.StageDatabyIndex(index);
            //itemIcon.sprite = cow.icon;
            UpdateStage(Id);
        }


        public void OnPointerDown(PointerEventData eventData)
        {

            down = true;
            StartCoroutine("ScaleDown");
        }

        //public void OnPointerUp(PointerEventData eventData)
        //{

        //    up = true;
        //    StartCoroutine("ScaleUp");
        //}

        public void OnPointerClick(PointerEventData eventData)
        {
            up = true;
            StartCoroutine("ScaleUp");
        }



        public void OnPointerExit(PointerEventData eventData)
        {
            exit = true;
            StartCoroutine("ScaleUp");
        }

        IEnumerator ScaleDown()
        {

            for (float i = currentScale; i >= minScale; i -= scaleSpeed * Time.deltaTime)
            {

                myTransform.localScale = new Vector3(i, i, i);
                currentScale = i;
                yield return null;
                if (up || exit) break;
            }

        }

        IEnumerator ScaleUp()
        {

            for (float i = currentScale; i <= maxScale; i += scaleSpeed * Time.deltaTime)
            {

                myTransform.localScale = new Vector3(i, i, i);
                currentScale = i;
                yield return null;
            }

            if (up) SelectItem();

            up = exit = false;
        }



        public void SelectItem()
        {

            //mainMenuReference.StagesAreaSelectItem(this);
            m_delegate(this);

        }



        public void purchasedItem()
        {
            coin.SetActive(false);
        }

        public void UpdateStage(int index)
        {

            coin.SetActive(false);
            nameText.SetText(stage.nameID);

            if (GameControl.Instance.playerData.level < stage.levelToUnlock)
            {
                lockedImage.SetActive(true);
                lockedLevelText.SetText("Lv. " + stage.levelToUnlock);
            }
            else
            {
                lockedImage.SetActive(false);
                if (!GameControl.Instance.playerData.purchasedStages.Contains(index))
                {
                    coin.SetActive(true);
                    coinText.SetText(stage.value.ToString());
                }

            }
        }

        public StageData ItemStageData()
        {
            return stage;
        }
    }
}