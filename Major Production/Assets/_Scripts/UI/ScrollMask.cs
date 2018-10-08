using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScrollMask : MonoBehaviour {

	[SerializeField] float openWidth;
	[SerializeField] float closedWidth;
	[SerializeField] float partialWidth;
	[SerializeField] float effectTime;
	RectTransform rect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SnapOpen()
	{
		Vector2 size = rect.sizeDelta;
		size.x = openWidth;
		rect.sizeDelta = size;
	}

	public void SnapClosed(bool partialClose = false)
	{
		Vector2 size = rect.sizeDelta;
		size.x = partialClose ? partialWidth : closedWidth;
		rect.sizeDelta = size;
	}

	public IEnumerator OpenScroll()
	{
		yield return ScrollSmooth(openWidth);
	}

	public IEnumerator CloseScroll(bool partialClose = false)
	{
		yield return ScrollSmooth(partialClose ? partialWidth : closedWidth);
	}

	protected IEnumerator ScrollSmooth(float endWidth)
	{
		Vector2 startSize = rect.sizeDelta;
		Vector2 currentSize = startSize;
		float elapsedTime = 0;
		while (elapsedTime < effectTime)
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.unscaledDeltaTime;
			float proportion = Mathf.Min(elapsedTime / effectTime, 1.0f);
			// TODO figure out nicer interpolation effect?
			currentSize.x = Mathf.SmoothStep(startSize.x, endWidth, proportion);
			rect.sizeDelta = currentSize;
		}
	}
}
