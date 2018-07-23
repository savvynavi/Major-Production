using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Dialogue { 

    [System.Serializable]
    public class DialogueEntry  {
        public int ID;
        public string Title;
        [SerializeField]
        private int speakerIndex;

        public int SpeakerIndex { get { return speakerIndex; } set { speakerIndex = value; } }
        public string Speaker { get {
                if(speakerIndex >= 0 && speakerIndex < parent.Speakers.Count)
                {
                    return parent.Speakers[speakerIndex];
                } else
                {
                    return "";
                }
            } }

        [TextArea]
        public string Text;
        public List<DialogueEventInstance> OnEnter = new List<DialogueEventInstance>();
        public List<CutsceneEvent> cutsceneEvents = new List<CutsceneEvent>();
        public TransitionList transitions;
        public bool isEnd;
        public Conversation parent;

        [Header("Responses")]
        public List<Response> Responses;

        [HideInInspector]
        public Vector2 position; // Used to place node in editor window

        public DialogueEntry(Conversation parent, int id)
        {
            this.parent = parent;
            this.ID = id;
            transitions = new TransitionList();
        }

        // Returns a name for the entry
        public string Name(bool prependID = false)
        {
            string name;
            bool usedID = false;
            if(!(string.IsNullOrEmpty(Title)))
            {
                name = Title;
            } else if (!string.IsNullOrEmpty(Text))
            {
                name = "\"" + Text + "\"";
            } else
            {
                name = "ID #" + ID.ToString();
                usedID = true;
            }
            if(prependID && !usedID)
            {
                name = "ID #" + ID.ToString() + ": " + name;
            }
            return name;
        }
	}
}
