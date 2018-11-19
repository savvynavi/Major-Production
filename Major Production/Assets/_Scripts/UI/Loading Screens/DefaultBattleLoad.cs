using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
	public class DefaultBattleLoad : BattleLoadEffect
	{
		public Image blackoutImage;

		public List<Texture> wipeTextures;

		public Texture battleWipeInTexture;

		public Texture battleOutroStartTexture;
		public Texture battleOutroEndTexture;

		public AudioSource battleStart;

		public float wipeOutTime = 1;
		public float battleWipeInTime = 0.5f;

		public float battleOutroStartTime = 0.5f;
		public float battleOutroEndTime = 0.5f;

		bool battleReady;
		bool loadFinished;

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		public override void BattleSceneReady()
		{
			battleReady = true;
		}

		public override void BeginBattleLoad()
		{
			// Turn on loading effects
			gameObject.SetActive(true);
			blackoutImage.gameObject.SetActive(false);
			battleReady = false;
			IsActivationAllowed = false;
			// TODO further initialization

			StartCoroutine(BattleLoadRoutine());
		}

		IEnumerator BattleLoadRoutine()
		{
			CameraWipe wipe = Camera.main.GetComponent<CameraWipe>();
			if(wipe == null)
			{
				Debug.LogError("CameraWipe component not found");
				IsActivationAllowed = true;
				gameObject.SetActive(false);
				yield break;
			}
			wipe.enabled = true;
			wipe.Cutoff = 0;
			wipe.WipeTexture = wipeTextures[Random.Range(0, wipeTextures.Count - 1)];
			float cutoffTime = 0;


			while(cutoffTime < wipeOutTime)
			{
				cutoffTime += Time.deltaTime;
				wipe.Cutoff = cutoffTime / wipeOutTime;
				yield return new WaitForEndOfFrame();
			}

			battleStart.Play();

			// Load effect goes in
			blackoutImage.gameObject.SetActive(true);

			// When it's done going in, allow activation (or maybe after it's ready)
			IsActivationAllowed = true;

			// wait until battle ready
			yield return new WaitUntil(() => { return this.battleReady; });

			// wait until load has finished
			yield return new WaitUntil(() => { return this.loadFinished; });

			wipe = Camera.main.GetComponent<CameraWipe>();
			if (wipe == null)
			{
				Debug.LogError("CameraWipe component not found");
				gameObject.SetActive(false);
				yield break;
			}
			wipe.enabled = true;
			wipe.Cutoff = 1;
			wipe.WipeTexture = battleWipeInTexture;
			blackoutImage.gameObject.SetActive(false);

			float battleCutoffTime = battleWipeInTime;
			while(battleCutoffTime > 0)
			{
				battleCutoffTime -= Time.deltaTime;
				wipe.Cutoff = battleCutoffTime / battleWipeInTime;
				yield return new WaitForEndOfFrame();
			}
			wipe.Cutoff = 0;
			wipe.enabled = false;

			// load effect goes out
			gameObject.SetActive(false);
		}

		public override void FinishBattleLoad()
		{
			loadFinished = true;
		}

		public override void LoadFailed()
		{
			gameObject.SetActive(false);
		}

		public override void StartBattleOutro()
		{
			gameObject.SetActive(true);
			IsOutroDone = false;
			loadFinished = false;
			StartCoroutine(OutroRoutine());
		}

		IEnumerator OutroRoutine()
		{
			CameraWipe wipe = Camera.main.GetComponent<CameraWipe>();
			if (wipe == null)
			{
				Debug.LogError("CameraWipe component not found");
				IsOutroDone = true;
				gameObject.SetActive(false);
				yield break;
			}
			wipe.enabled = true;
			wipe.Cutoff = 0;
			wipe.WipeTexture = battleOutroStartTexture;
			float cutoffTime = 0;

			while(cutoffTime < battleOutroStartTime)
			{
				cutoffTime += Time.deltaTime;
				wipe.Cutoff = cutoffTime / battleOutroStartTime;
				yield return new WaitForEndOfFrame();
			}
			IsOutroDone = true;
			blackoutImage.gameObject.SetActive(true);
			yield return new WaitUntil(() => { return this.loadFinished; });
			blackoutImage.gameObject.SetActive(false);
			wipe = Camera.main.GetComponent<CameraWipe>();
			if (wipe == null)
			{
				Debug.LogError("CameraWipe component not found");
				gameObject.SetActive(false);
				yield break;
			}
			wipe.enabled = true;
			wipe.Cutoff = 1;
			wipe.WipeTexture = battleOutroEndTexture;
			float outroCutoffTime = battleOutroEndTime;
			while(outroCutoffTime > 0)
			{
				outroCutoffTime -= Time.deltaTime;
				wipe.Cutoff = outroCutoffTime / battleOutroEndTime;
				yield return new WaitForEndOfFrame();
			}

			wipe.enabled = false;
			wipe.Cutoff = 0;
			gameObject.SetActive(false);
		}

		public override void UpdateLoadProgress(float progress)
		{
			// Don't need to display this
		}

		public override void FinishBattleOutro()
		{
			loadFinished = true;
		}
	}
}