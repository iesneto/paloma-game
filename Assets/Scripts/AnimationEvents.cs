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
    

}
