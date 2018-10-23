using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// Sets the content of several UI elements in the same panel
/// This class expects the textboxPanel, responsePanel, and preferably the ButtonPrototype to
/// be set up with layout groups
/// </summary>
public class DialogueBox : MonoBehaviour {

    [SerializeField] RectTransform textboxPanel;
    [SerializeField] Text titleText;
    [SerializeField] Image portraitImage;
    [SerializeField] Text dialogueText;
    [SerializeField] RectTransform responsePanel;
    [SerializeField] GameObject ButtonPrototype;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowBox()
    {
        textboxPanel.gameObject.SetActive(true);
		RebuildLayout();
        // might add effects here
    }

    public void HideBox()
    {
        // might add effects here
        textboxPanel.gameObject.SetActive(false);
    }

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void SetPortrait(Sprite image)
    {
        portraitImage.sprite = image;
    }

    public void SetDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public Button AddButton(string text, UnityAction onclick = null)
    {
        GameObject buttonClone = GameObject.Instantiate(ButtonPrototype, responsePanel);
        Button buttonScript = buttonClone.GetComponent<Button>();
        Text buttonText = buttonClone.GetComponentInChildren<Text>();

        buttonText.text = text;
        if(onclick != null)
        {
            buttonScript.onClick.AddListener(onclick);
        }

        return buttonScript;
    }

    public void ClearButtons()
    {
        foreach (Transform child in responsePanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void RebuildLayout()
    {
		LayoutRebuilder.ForceRebuildLayoutImmediate(textboxPanel);
    }
}
