using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.InventoryEngine;

public class Interactable : MonoBehaviour
{
    // The prefab for the prompt
    public GameObject PromptPref;
    // The offset for the prompt
    public Vector3 PromptOffset;

    // The highlight object
    HighlightObject highlight;
    // The minimum distance to interact
    public float minDistToInteract = 2f;
    // The player transform
    public static Transform player;
    // Whether the player is inside the interactable object
    bool IsInside;
    // The item picker associated with the interactable object
    public ItemPicker item;
    // The NPC associated with the interactable object
    public NPC npc;

    // Set up the highlight object
    private void Awake()
    {
        SetHighlight();
    }

    // Instantiate the highlight object and set its position and state
    void SetHighlight()
    {
        highlight = Instantiate(PromptPref, transform).GetComponent<HighlightObject>();
        highlight.transform.localPosition = PromptOffset;
        highlight.currentState = ItemHighlightStates.Hidden;
        highlight.root = this;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is inside the interactable object
        if (IsInside)
        {
            // If the left mouse button is pressed and the player is close enough to interact, set the highlight state to Interact and call OnInteract
            if (Input.GetMouseButton(0) && (transform.position - player.position).magnitude < minDistToInteract && highlight.currentState != ItemHighlightStates.Interact)
            {
                highlight.currentState = ItemHighlightStates.Interact;
                OnInteract();
            }
            // If the right mouse button is pressed, set the highlight state to View and call OnView
            else if (Input.GetMouseButton(1) && highlight.currentState != ItemHighlightStates.Interact)
            {
                highlight.currentState = ItemHighlightStates.View;
                OnView();
            }
            // If the left or right mouse button is released, set the highlight state to Normal
            //else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) highlight.currentState = ItemHighlightStates.Normal;
            // If the player is too far away to interact, set the highlight state to TooFartoInteract
            else if ((transform.position - player.position).magnitude > minDistToInteract)
            {
                highlight.currentState = ItemHighlightStates.TooFartoInteract;
            }
            // Otherwise, set the highlight state to Normal
            else
            {
                highlight.currentState = ItemHighlightStates.Normal;
            }
            // Update the logs
            UpdateLogs();
        }
    }

    // Update the logs when the mouse button is released
    private void UpdateLogs()
    {
        if (Input.GetMouseButtonUp(0) && (transform.position - player.position).magnitude < minDistToInteract && highlight.currentState != ItemHighlightStates.Interact)
        {
            highlight.currentState = ItemHighlightStates.Interact;
            InteractionLog();
        }
        else if (Input.GetMouseButtonUp(1) && highlight.currentState != ItemHighlightStates.Interact)
        {
            highlight.currentState = ItemHighlightStates.View;
            ViewLog();
        }
    }

    // Whether the player is too far away to interact
    public bool IsFartointeract
    {
        get { return (transform.position - player.position).magnitude > minDistToInteract; }
    }

    // When the mouse enters the interactable object, set IsInside to true and set the highlight state to Normal or TooFartoInteract
    void OnMouseEnter()
    {
        IsInside = true;
        if ((transform.position - player.position).magnitude > minDistToInteract)
        {
            highlight.currentState = ItemHighlightStates.TooFartoInteract;
        }
        else
        {
            highlight.currentState = ItemHighlightStates.Normal;
        }
    }

    // When the mouse exits the interactable object, set IsInside to false and set the highlight state to Hidden
    private void OnMouseExit()
    {
        IsInside = false;
        highlight.currentState = ItemHighlightStates.Hidden;
    }

    // When the player interacts with the object, pick up the item or start the NPC dialogue and open the inventory
    public void OnInteract()
    {
        if (item != null)
        {
            item.Pick();
            FindObjectOfType<InventoryInputManager>().OpenInventory();
        }
        if (npc != null)
        {
            npc.StartDialogue();
            FindObjectOfType<InventoryInputManager>().OpenInventory();
        }
    }

    // Add the interaction log to the dialogue zone
    private void InteractionLog()
    {
        if (item)
        {
            FindObjectOfType<DialogueZone>().AddInteractionLog(item.Item.InteractionLogDetail);
        }
        if (npc != null)
        {
            FindObjectOfType<DialogueZone>().AddInteractionLog(npc.InteractLog);
        }
    }

    // When the player views the object, do nothing
    public void OnView()
    {
        //
    }

    // Add the view log to the dialogue zone
    private void ViewLog()
    {
        if (item)
        {
            FindObjectOfType<DialogueZone>().AddViewLog(item.Item.ViewLogDetail);
        }
        if (npc)
        {
            FindObjectOfType<DialogueZone>().AddViewLog(npc.ViewLog);
        }
    }
}