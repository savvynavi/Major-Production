using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IDragTarget
{
	//TODO add arguments
	void Hover(Draggable dragged);

	bool Drop(Draggable dragged);
}
