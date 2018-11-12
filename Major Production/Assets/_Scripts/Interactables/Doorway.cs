using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
    public class Doorway : Interactable
    {
        [SerializeField]
        string dungeonScene;
        [SerializeField]
        int entryPoint = -1;

        private void Start()
        {
        }

        public override void Hilight()
        {
            
        }

        public override void Interact(InteractionUser user)
        {
            SceneLoader.Instance.LoadScene(dungeonScene, entryPoint);
            // TODO maybe make routine with effects, animation, etc
        }

        public override void Unhilight()
        {
            // TODO
        }
    }
}