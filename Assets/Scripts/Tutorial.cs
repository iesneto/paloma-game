using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamob
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private GameObject controllers;
        [SerializeField] private GameObject skipButton;
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject tutorial01;
        [SerializeField] private GameObject tutorial02;
        //[SerializeField] private int tutorialIndex;
        [SerializeField] private int tutorialsNumber;
        private bool tutorial02Permission;

        private void Awake()
        {
            
            controllers.SetActive(false);
            skipButton.SetActive(false);
            tutorial01.SetActive(false);
            tutorial02.SetActive(false);
        }

        public void ShowTutorial02()
        {
            tutorial02Permission = true;
        }

        public int TutorialsNumber()
        {
            return tutorialsNumber;
        }


        public void WakeUp()
        {
            if (!GameControl.Instance.playerData.tutorials[0])
            {
                this.gameObject.SetActive(true);
                controllers.SetActive(true);
                //skipButton.SetActive(true);
                GameControl.Instance.PauseGame();
                GameControl.Instance.GameControlUI().ClosePlayerStartedUI();
            }
            if (tutorial02Permission) 
            {
                this.gameObject.SetActive(true);
                GameControl.Instance.GameControlUI().ClosePlayerStartedUI();
                
                PlayTutorial02();
            } 
        }

        public void OnPointerEnter()
        {
            GameControl.Instance.UnpauseGame();
            controllers.SetActive(false);
            skipButton.SetActive(false);
            
            GameControl.Instance.playerData.tutorials[0] = true;

            PlayTutorial01();
        }

        void PlayTutorial01()
        {
            tutorial01.SetActive(true);
            tutorial01.GetComponentInChildren<Animator>().SetBool("Open", true);
            GameControl.Instance.AudioManager().OpenWindow();
            GameControl.Instance.PauseGame();
            
        }

        public void PlayTutorial02()
        {
            
            tutorial02.SetActive(true);
            tutorial02.GetComponentInChildren<Animator>().SetBool("Open", true);
            GameControl.Instance.AudioManager().OpenWindow();
            tutorial02Permission = false;
            GameControl.Instance.PauseGame();
        }

        public void CloseTutorial02Window()
        {
            GameControl.Instance.UnpauseGame();
            tutorial02.GetComponentInChildren<Animator>().SetBool("Open", false);
            GameControl.Instance.GameControlUI().ShowWaitPlayerStartUI();
            GameControl.Instance.AudioManager().CloseWindow();
            Invoke("FinishTutorial02", 0.7f);
        }

        public void CloseTutorialWindow()
        {
            GameControl.Instance.UnpauseGame();
            tutorial01.GetComponentInChildren<Animator>().SetBool("Open", false);
            GameControl.Instance.GameControlUI().ShowWaitPlayerStartUI();
            GameControl.Instance.AudioManager().CloseWindow();
            Invoke("FinishTutorial01", 0.7f);
        }

        void FinishTutorial01()
        {
            tutorial01.SetActive(false);
            GameControl.Instance.playerData.tutorials[1] = true;
        }

        void FinishTutorial02()
        {
            tutorial02.SetActive(false);
            GameControl.Instance.playerData.tutorials[2] = true;
        }



        public void SkipButton()
        {

        }
    }
}