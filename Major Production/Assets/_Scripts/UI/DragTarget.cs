﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class DragTarget : MonoBehaviour
{
	bool hovering = false;
	bool hoverFlag = false;

	//TODO add arguments
	public void Hover(Draggable dragged)
	{
		if (!hovering)
		{
			OnHoverEnter(dragged);
		}
		hovering = true;
		hoverFlag = true;
	}

	protected void Update()
	{
		if (hovering && !hoverFlag)
		{
			hovering = false;
			OnHoverLeave();
		}
		hoverFlag = false;
	}

	protected abstract void OnHoverEnter(Draggable dragged);

	protected abstract void OnHoverLeave();

	public abstract bool Drop(Draggable dragged);
}
