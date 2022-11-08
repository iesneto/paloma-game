using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public GameObject parent;

    public void EndAnimation(int i)
    {
        
        parent.GetComponent<CowBehavior>().EndAnimation(i);
    }

    public void OnGrab()
    {
        parent.GetComponent<CowBehavior>().OnGrab();
    }

    public void StartPlayerGrabAnimation()
    {
        parent.GetComponent<CowBehavior>().StartPlayerGrabAnimation();
    }

    public void FinishGrabAnimation()
    {
        parent.GetComponent<PlayerBehavior>().FinishGrabAnimation();
    }

    public void CloseWindow()
    {
        parent.GetComponent<UIManager>().DisableLeaveStage();
    }

    public void CloseTutorialWindow()
    {
        parent.GetComponent<Tutorial>().OnPointerEnter();
    }

    public void CloseMainMenuWindow()
    {
        parent.GetComponent<MainMenu>().FinishCloseWindow();
    }
    

}
