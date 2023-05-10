using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using MoreMountains.InventoryEngine;
using TMPro;

namespace MoreMountains.TopDownEngine
{
    [Serializable]
    public class DialogueElement
    {
        [Multiline]
        public string DialogueLine;

        public DialogueElement(string s)
        {
            DialogueLine = s;
        }
    }

    [AddComponentMenu("TopDown Engine/GUI/Dialogue Zone")]
    public class DialogueZone : ButtonActivated
    {
        [Header("Activations")]
        [Tooltip("true if can be activated more than once")]
        public bool ActivableMoreThanOnce = true;
        [Range(1, 100)]
        [Tooltip("if the zone is activable more than once, how long should it remain inactive between up times?")]
        public float InactiveTime = 2f;
        [Tooltip("the dialogue lines")]
        public DialogueElement[] Dialogue;

        protected bool _activated = false;
        protected bool _playing = false;
        protected int _currentIndex;
        protected bool _activable = true;
        public static bool IsDialogueActive;
        public InventoryInputManager inventoryInputManager;
        public Sprite ViewItemIcon, InteractItemIcon;

        protected override void OnEnable()
        {
            base.OnEnable();
            _currentIndex = 0;
        }

        public override void TriggerButtonAction()
        {
            if (!CheckNumberOfUses())
            {
                return;
            }
            base.TriggerButtonAction();
            StartDialogue();
        }

        public virtual void StartDialogue()
        {
            if (!_playing)
            {
                _playing = true;
            }
            StartCoroutine(PlayNextDialogue());
        }

        public IEnumerator PlayNextDialogue()
        {
            DialogueZone.IsDialogueActive = true;
            _currentIndex = 0;
            // LevelManager.TryGetInstance().Players[0].GetComponent<CharacterMovement>().MovementForbidden = true;
            inventoryInputManager.OpenInventory();
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().selectionFocusPosition = VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().text.Length;
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().Select();
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().ActivateInputField();
            yield return new WaitUntil(() => VoiceFlowManager.manager.UserEntered);
            VoiceFlowManager.manager.UserEntered = false;
            List<DialogueElement> diags = new List<DialogueElement>();
            string uIn = VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().text;
            ChatManager.manager.AddMsg(null, "You", uIn, true);
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().text = "";
            ChatManager.manager.ClearChatInput();
            yield return StartCoroutine(VoiceFlowManager.manager.GetResponse(uIn));
            if (VoiceFlowManager.resp != null)
            {
                diags.Add(new DialogueElement(VoiceFlowManager.resp));
            }
            ChatManager.manager.AddMsg(null, "Wife", VoiceFlowManager.resp);
            Dialogue = diags.ToArray();
            if (_currentIndex >= Dialogue.Length)
            {
                _currentIndex = 0;
                _activated = true;
                if ((_characterButtonActivation != null))
                {
                    _characterButtonActivation.InButtonActivatedZone = false;
                    _characterButtonActivation.ButtonActivatedZone = null;
                }
                if (ActivableMoreThanOnce)
                {
                    _activable = false;
                    _playing = false;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                yield break;
            }
            _currentIndex++;
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().selectionFocusPosition = VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().text.Length;
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().Select();
            VoiceFlowManager.manager.UserInput.GetComponent<TMP_InputField>().ActivateInputField();
        }

        public void AddViewLog(string _ItemViewLog)
        {
            List<DialogueElement> diags = new List<DialogueElement>();
            string uIn = _ItemViewLog;
            ChatManager.manager.AddMsg(ViewItemIcon, "Log", uIn, true, true);
        }

        public void AddInteractionLog(string _ItemInteractionLog)
        {
            List<DialogueElement> diags = new List<DialogueElement>();
            string uIn = _ItemInteractionLog;
            ChatManager.manager.AddMsg(InteractItemIcon, "Log", uIn, true, true);
        }
    }
}