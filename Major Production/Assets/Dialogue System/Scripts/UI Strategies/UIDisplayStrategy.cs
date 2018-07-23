using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [RequireComponent(typeof(DialogueUI))]
    public class UIDisplayStrategy : MonoBehaviour
    {
        [SerializeField]
        protected DialogueUI uiManager;

        // Called when a new DialogueEntry is entered
        public virtual void DisplayDialogueEntry(DialogueEntry entry)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called for each response in a DialogueEntry when it is entered
        /// </summary>
        /// <param name="response">The response to display</param>
        /// <param name="ID">ID used to indicate selected response</param>
        /// <param name="possible">True if the response can be selected</param>
        public virtual void DisplayResponse(Response response, int ID, bool possible)
        {
            throw new System.NotImplementedException();
        }

        // Called after all responses for a DialogueEntry have been sent
        public virtual void FinishedDisplayResponses()
        {

        }

        // Clear responses displayed
        public virtual void ClearResponses()
        {
            throw new System.NotImplementedException();
        }

        // Called when a conversation begins
        public virtual void OnConversationStart()
        {

        }

        // Called when ending a conversation
        public virtual void OnConversationEnd()
        {

        }
    }
}