using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGsys {
	public class EnemyUI : MonoBehaviour {
		float CharacterCurrentHP;
		float CharacterMaxHP;
		float CharacterCurrentMP;
		float CharacterMaxMP;
		Character chara;

		public Image hp;
		public Image mp;
		public Image bg;
		Image bg1;
		Image bg2;
		public GameObject canvas;

		private void Awake() {
			chara = GetComponent<Character>();
		}

		private void Start() {


			hp.type = Image.Type.Filled;
			hp.fillMethod = Image.FillMethod.Horizontal;
			mp.type = Image.Type.Filled;
			mp.fillMethod = Image.FillMethod.Horizontal;

			CharacterCurrentHP = GetComponent<Character>().Hp;
			CharacterMaxHP = GetComponent<Character>().hpStat;
			CharacterCurrentMP = GetComponent<Character>().Mp;
			CharacterMaxMP = GetComponent<Character>().mpStat;
		}

		private void Update() {
			CharacterCurrentHP = GetComponent<Character>().Hp;
			CharacterMaxHP = GetComponent<Character>().hpStat;
			CharacterCurrentMP = GetComponent<Character>().Mp;
			CharacterMaxMP = GetComponent<Character>().mpStat;

			float hpScale = Mathf.Clamp01(CharacterCurrentHP / CharacterMaxHP);
			//hp.transform.forward = Camera.main.transform.forward;
			hp.fillAmount = hpScale;

			float mpScale = Mathf.Clamp01(CharacterCurrentMP / CharacterMaxMP);
			//mp.transform.forward = Camera.main.transform.forward;
			mp.fillAmount = mpScale;
		}

		public void enemyUISetup() {
			//hp/mp bars/bg
			GameObject tmpbg1 = Instantiate(bg.gameObject);
			bg1 = tmpbg1.GetComponent<Image>();
			bg1.transform.SetParent(canvas.transform, false);
			bg1.transform.position = chara.transform.position + Vector3.up * 2;

			GameObject tmpHp = Instantiate(hp.gameObject);
			hp = tmpHp.GetComponent<Image>();
			hp.transform.SetParent(canvas.transform, false);
			hp.transform.position = chara.transform.position + Vector3.up * 2;

			GameObject tmpbg2 = Instantiate(bg.gameObject);
			bg2 = tmpbg2.GetComponent<Image>();
			bg2.transform.SetParent(canvas.transform, false);
			bg2.transform.position = chara.transform.position + Vector3.up * 2.25f;

			GameObject tmpMp = Instantiate(mp.gameObject);
			mp = tmpMp.GetComponent<Image>();
			mp.transform.SetParent(canvas.transform, false);
			mp.transform.position = chara.transform.position + Vector3.up * 2.25f;
		}

		public void ShowUI() {
			hp.gameObject.SetActive(true);


			mp.gameObject.SetActive(true);


			bg1.gameObject.SetActive(true);
			bg2.gameObject.SetActive(true);
		}

		public void HideUI() {
			hp.gameObject.SetActive(false);
			mp.gameObject.SetActive(false);
			bg1.gameObject.SetActive(false);
			bg2.gameObject.SetActive(false);
		}
	}
}

