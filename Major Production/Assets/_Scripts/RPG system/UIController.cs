using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys
{
	public class UIController : MonoBehaviour
	{
		//public Button button;
		//public Text font;
		public GameObject canvas;
		public Image hpOrb;
		public Image mpOrb;
		//public Image BattleHudMain;
		//public Image tabBackground;
		//public Image infoBackground;
		//public Image backButtonBackground;
		//public Image confirmationMenu;


		public Transform position;


		// Use this for initialization
		void Start()
		{
			hpOrb.type = Image.Type.Filled;
			mpOrb.type = Image.Type.Filled;
			hpOrb.fillMethod = Image.FillMethod.Radial180;
			mpOrb.fillMethod = Image.FillMethod.Radial180;

			GameObject tmpbg1 = Instantiate(hpOrb.gameObject);
			hpOrb = tmpbg1.GetComponent<Image>();
			hpOrb.transform.SetParent(canvas.transform, false);
			hpOrb.transform.position = position.transform.position;
		}

		// Update is called once per frame
		void Update()
		{



			//hpOrb.transform.position = position.transform.position;
			//mpOrb.transform.position = position.transform.position;
			//mpOrb.transform.Rotate(new Vector3(0, 0, 180));

			//hpOrb.transform.SetParent(canvas.transform, false);
			//mpOrb.transform.SetParent(canvas.transform, false);

		}
	}

}