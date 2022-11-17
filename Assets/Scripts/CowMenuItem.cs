using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CowMenuItem : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerExitHandler
{

    public int Id { get; set; }
    [SerializeField] private RectTransform myTransform;
    //[SerializeField] private Image itemFrame;
    //[SerializeField] private Sprite itemSelected;
    //[SerializeField] private Sprite itemDeselected;
    [SerializeField] private Color selected;
    [SerializeField] private Color deselected;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject coin;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject lockedImage;
    [SerializeField] private TextMeshProUGUI lockedLevelText;
    [SerializeField] private TextMeshProUGUI colectedNumberText;
    //private MainMenu mainMenuReference;
    CowData cow;
    private float currentScale = 1f;
    private float maxScale = 1f;
    private float minScale = 0.5f;
    private float scaleSpeed = 4f;
    private bool up, down, exit;
    public delegate void cowDelegate(CowMenuItem cow);
    private cowDelegate m_delegate;

    //public void InitalizeItem(cowDelegate _method, int index, MainMenu reference)
    public void InitalizeItem(cowDelegate _method, int index)
    {
        m_delegate = _method;
        //mainMenuReference = reference;
        Id = index;
        //itemFrame.sprite = itemDeselected;
        //itemFrame.color = deselected;
        cow = GameControl.Instance.CowByIndex(index);
        itemIcon.sprite = cow.icon;
        UpdateCow(Id);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
        down = true;
        StartCoroutine("ScaleDown");
    }
    
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
            if(up || exit) break;
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
        //itemFrame.sprite = itemSelected;
        //itemFrame.color = selected;
        //mainMenuReference.CowAreaSelectItem(this);
        m_delegate(this);
    }

    public void DeselectItem()
    {
        //itemFrame.sprite = itemDeselected;
        //itemFrame.color = deselected;
    }

    public void purchasedItem()
    {
        coin.SetActive(false);
    }

    public void UpdateCow(int index)
    {
        
        coin.SetActive(false);
        colectedNumberText.gameObject.SetActive(false);
        if (GameControl.Instance.playerData.level < cow.levelToUnlock)
        {
            lockedImage.SetActive(true);
            lockedLevelText.SetText("Lv. " + cow.levelToUnlock);
        }
        else
        {
            lockedImage.SetActive(false);
            if (GameControl.Instance.playerData.purchasedCows.Contains(index))
            {
                colectedNumberText.gameObject.SetActive(true);
                colectedNumberText.SetText(GameControl.Instance.playerData.numCows[index].ToString());
            }
            else
            {
                coin.SetActive(true);
                coinText.SetText(cow.value.ToString());
            }
        }
    }

    public CowData ItemCowData()
    {
        return cow;
    }
}
