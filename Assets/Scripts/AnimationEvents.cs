using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;
using System;

namespace Gamob
{
    public class AnimationEvents : MonoBehaviour
    {
        [Serializable]
        public class AnimationFinishEvent : UnityEvent { }

        [FormerlySerializedAs("onAnimationFinish")]
        [SerializeField]
        private AnimationFinishEvent m_OnFinish = new AnimationFinishEvent();

        public GameObject parent;


        public void FinishAnimation()
        {
            m_OnFinish.Invoke();
        }

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
            parent.GetComponent<UIManager>().DisableOpenedWindow();
        }

        //public void CloseTutorialWindow()
        //{
        //    parent.GetComponent<Tutorial>().OnPointerEnter();
        //}

        public void CloseMainMenuWindow()
        {
            parent.GetComponent<MainMenu>().FinishCloseWindow();
        }


    }
}