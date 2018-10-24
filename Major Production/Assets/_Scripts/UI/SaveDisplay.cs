using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDisplay : MonoBehaviour {

	[SerializeField] Text saveText;
	public float messageDisplayTime = 1;
	public float messageFadeoutTime = 1;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BeginSave()
	{
		gameObject.SetActive(true);
		saveText.text = "Saving...";
	}

	public void FinishSave()
	{
		saveText.text = "Save Successful!";
		StartCoroutine(SaveFinishRoutine());
	}

	IEnumerator SaveFinishRoutine()
	{
		yield return new WaitForSecondsRealtime(messageDisplayTime);
		float elapsedTime = 0;
		Color textColor = saveText.color;
		while(elapsedTime < messageFadeoutTime)
		{
			elapsedTime += Time.unscaledDeltaTime;
			saveText.color = Color.Lerp(textColor, Color.clear, Mathf.Min(elapsedTime / messageFadeoutTime, 1));
			yield return new WaitForEndOfFrame();
		}
		saveText.color = textColor;
		gameObject.SetActive(false);
	}
}

