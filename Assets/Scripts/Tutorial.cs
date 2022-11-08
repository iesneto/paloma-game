using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject controllers;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject tutorial01;
    [SerializeField] private int tutorialIndex;

    private void Awake()
    {
        tutorialIndex = 0;
        controllers.SetActive(false);
        skipButton.SetActive(false);
        tutorial01.SetActive(false);
    }


    public void WakeUp()
    {
        if (!GameControl.Instance.playerData.tutorials[0])
        {
            controllers.SetActive(true);         
            //skipButton.SetActive(true);
            GameControl.Instance.PauseGame();
            GameControl.Instance.GameControlUI().ClosePlayerStartedUI();
        }
            
    }

    public void OnPointerEnter()
    {
        controllers.SetActive(false);
        skipButton.SetActive(false);
        tutorial01.SetActive(false);
        GameControl.Instance.playerData.tutorials[tutorialIndex] = true;
        
        if(tutorialIndex == 0)
        {
            PlayTutorial01();
        }
    }

    void PlayTutorial01()
    {       
        tutorial01.SetActive(true);
        tutorial01.GetComponentInChildren<Animator>().SetBool("Open", true);
        GameControl.Instance.PauseGame();
        tutorialIndex = 1;
    }

    public void CloseTutorialWindow()
    {
        GameControl.Instance.UnpauseGame();
        tutorial01.GetComponentInChildren<Animator>().SetBool("Open", false);
        GameControl.Instance.GameControlUI().ShowWaitPlayerStartUI();
    }



    public void SkipButton()
    {
        
    }
}
