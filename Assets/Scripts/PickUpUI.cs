using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpUI : MonoBehaviour
{
    private Animation myAnimation;
    public TextMeshProUGUI value;
    public bool playDelayed;
    public float time;

    

    public void Setup(int _value)
    {
        
        value.SetText(_value.ToString());
        myAnimation = GetComponent<Animation>();
        if(playDelayed)
        {
            StartCoroutine("PlayDelayed");
        }
        else
        {
            myAnimation.Play();
            Finish();
        }
        
    }

    IEnumerator PlayDelayed()
    {
        yield return new WaitForSeconds(time);
        myAnimation.Play();
        Finish();
    }

    void Finish()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
