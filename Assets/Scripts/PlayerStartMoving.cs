using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerStartMoving : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        
        GameControl.Instance.StartMoving();
    }
}
