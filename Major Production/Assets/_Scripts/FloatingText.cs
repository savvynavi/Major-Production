using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FloatingText : MonoBehaviour {

    public Animator animator;
    private Text popupText;

    private void Awake()
    {
        Debug.Log("waking up");
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        popupText = animator.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        // lazy initialisation in cas ethis gets called before start
        if (popupText == null)
            popupText = animator.GetComponent<Text>();

        popupText.text = text;
    }
}
