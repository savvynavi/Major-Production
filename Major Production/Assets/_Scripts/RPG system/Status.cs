using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPGsys{
	public abstract class Status : ScriptableObject {
		Material originalMaterial;

		public ParticleSystem particles;
		public Material material;
		public Shader shader;
		protected GameObject partInst;
		protected Material matInst;
		protected Shader shaderInst;
		protected List<Shader> charaShaders;
		bool particleRunning;
		public Equipable equipable;

		//public Animation anim;
		public enum StatusEffectType {
			Buff,
			Debuff,
			Heal,
			DamageOverTime
		}

		public enum StatusEffectTarget {
			Self,
			Target,
			Group
		}

		public enum Equipable {
			False,
			True
		}

		[System.Serializable]
		public struct StatusEffect {
			public StatusEffectType effect;
			public RPGStats.Stats statBuff;
			public float amount;
		}

		public float timer;

		private void OnEnable()
		{
			RPG.Save.Factory<Status>.Register(this);
		}

		//reduces time by 1 turn each time it's called
		virtual public void UpdateEffect(Character chara) {
			timer--;
			if(particles != null && (timer < particles.main.startLifetime.constant || chara.Hp <= 0) && partInst != null) {
				partInst.GetComponent<ParticleSystem>().Stop();
			}
			if(material != null && (timer < 1 || chara.Hp <= 0)) {
				chara.gameObject.GetComponentInChildren<Renderer>().material = originalMaterial;
			}
			if(shader != null && (timer < 1 || chara.Hp <= 0)) {
				foreach(Renderer shad in chara.GetComponentsInChildren<Renderer>()) {
					if(shad.GetComponent<ParticleSystem>() == null) {
						//edit here if models don't use the standard shader (eg are legacy ones)
						shad.material.shader = Shader.Find("Standard");
					}
				}
			}
		}

		public virtual void Apply(Character target, float duration) {

		}

		public virtual void Remove(Character target){
			Destroy(partInst);
			if(material != null) {
				target.gameObject.GetComponentInChildren<Renderer>().material = originalMaterial;
			}
		}

		public virtual void EquipApply(Character target, RPGItems.Item item) {

		}

		public virtual void EquipRemove(Character target, RPGItems.Item item) {

		}

		public void Clone(Character target){
			// make as copy of the particles
			if(particles != null) {
				partInst = Instantiate(particles.gameObject);
				partInst.transform.parent = target.transform;
				partInst.transform.localPosition = Vector3.zero;
			}
			//makes a copy of the materialFIX THIS OR REMOVE AND REPLACE WITH SHADERS
			if(material != null) {
				matInst = Instantiate(material);
				target.GetComponentInChildren<Renderer>().material.EnableKeyword("_METALLICGLOSSMAP");
				originalMaterial = target.GetComponentInChildren<Renderer>().material;
				//matInst.mainTexture = target.gameObject.GetComponentInChildren<Renderer>().material.mainTexture;
				matInst.SetTexture("_MetallicGlossMap", target.GetComponentInChildren<Renderer>().material.mainTexture);
				
				target.gameObject.GetComponentInChildren<Renderer>().material = matInst;
				
			}

			if(shader != null) {
				//shaderInst = Instantiate(shader);
				//CAN'T INSTANTIATE SHADERS OR ELSE IT CRASHES THE BUILD
				shaderInst = shader;
				foreach(Renderer shad in target.GetComponentsInChildren<Renderer>()) {
					if(shad.GetComponent<ParticleSystem>() == null) {
						shad.material.shader = shaderInst;
					}
				}
			}
		}
	}
}