using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public struct CutsceneEvent 
    {
        public string target;       // Key of Animator in CutsceneManager
        public string animation;    // Name of animation state to enter
    }
}