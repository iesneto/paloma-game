using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerStartMoving : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameControl.Instance.StartMoving();
    }
}
