using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName ="Dialogue/Conversation")]
    public class Conversation : ScriptableObject
    {
        [SerializeField][HideInInspector]
        int nextID = 0;     // Next unused ID

        public int NextID { get { return nextID; } }

        [SerializeField]
        private int startingID; // ID of dialogue entry to start the conversation

        public List<DialogueEntry> Entries = new List<DialogueEntry>();

        // Keys for Speakers in this conversation, used by DialogueManager to select actor
        public List<string> Speakers = new List<string>();

        // Returns dialogue entry identified by startingID
        public DialogueEntry Start { get { return FindEntry(startingID); } }

        public DialogueEntry FindEntry(int id)
        {
            DialogueEntry entry = Entries.Find(e => e.ID == id);
            return entry;
        }

        public DialogueEntry FindEntry(string title)
        {
            DialogueEntry entry = Entries.Find(e => e.Title == title);
            return entry;
        }

        // Adds DialogueEntry to conversation with unique ID
        public DialogueEntry AddEntry()
        {
            DialogueEntry newEntry = new DialogueEntry(this, nextID);
            if (Entries == null)
            {
                Entries = new List<DialogueEntry>();
            }
            ++nextID;
            Entries.Add(newEntry);
            return newEntry;
        }

        public void RemoveEntry(DialogueEntry entry)
        {
            Entries.Remove(entry);
        }


    }
}
