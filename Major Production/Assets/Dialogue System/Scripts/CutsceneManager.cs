using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    public class CutsceneManager : MonoBehaviour {

        [SerializeField]
        StringAnimatorDict Animators;
        Dictionary<string, Animator> AnimatorDictionary;

        private void Awake()
        {
            AnimatorDictionary = Animators.ToDictionary();
        }

        // Plays all animations indicated by CutsceneEvents in list
        public void DoCutsceneEvents(List<CutsceneEvent> events)
        {
            foreach(CutsceneEvent e in events)
            {
                Animator target = AnimatorDictionary[e.target];
                if(target != null)
                {
                    target.Play(e.animation);
                } else
                {
                    Debug.LogError("Animator \"" + e.target + "\" not found in Cutscene Manager");
                }
            }
        }

        public void ActivateAnimators()
        {
            foreach (KeyValuePair<string, Animator> a in AnimatorDictionary)
            {
                a.Value.enabled = true;
            }
        }

        public void DeactivateAnimators()
        {
            foreach(KeyValuePair<string,Animator> a in AnimatorDictionary)
            {
                a.Value.enabled = false;
            }
        }
    }
}
