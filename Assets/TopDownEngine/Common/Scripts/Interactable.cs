using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.InventoryEngine;

public class Interactable : MonoBehaviour
{

    public GameObject PromptPref;
    public Vector3 PromptOffset;

    HighlightObject highlight;
    public float minDistToInteract = 2f;
    public static Transform player;
    bool IsInside;
    public ItemPicker item;
    public NPC npc;

    private void Awake()
    {
        SetHighlight();
    }

    void SetHighlight(){
        highlight = Instantiate(PromptPref,transform).GetComponent<HighlightObject>();
        highlight.transform.localPosition = PromptOffset;
        highlight.currentState = ItemHighlightStates.Hidden;
        highlight.root = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsInside)
        {
            if(Input.GetMouseButton(0) && (transform.position - player.position).magnitude < minDistToInteract && highlight.currentState != ItemHighlightStates.Interact) { highlight.currentState = ItemHighlightStates.Interact; OnInteract(); }
            else if(Input.GetMouseButton(1) && highlight.currentState != ItemHighlightStates.Interact) { highlight.currentState = ItemHighlightStates.View; OnView(); }
            //else if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) highlight.currentState = ItemHighlightStates.Normal;
            else if((transform.position - player.position).magnitude > minDistToInteract) highlight.currentState = ItemHighlightStates.TooFartoInteract;
            else highlight.currentState = ItemHighlightStates.Normal;
            UpdateLogs();

        }

    }

    private void UpdateLogs()
    {
        if(Input.GetMouseButtonUp(0) && (transform.position - player.position).magnitude < minDistToInteract && highlight.currentState != ItemHighlightStates.Interact) { highlight.currentState = ItemHighlightStates.Interact; InteractionLog(); }
        else if(Input.GetMouseButtonUp(1) && highlight.currentState != ItemHighlightStates.Interact) { highlight.currentState = ItemHighlightStates.View; ViewLog(); }
    }

    public bool IsFartointeract{
        get{return (transform.position - player.position).magnitude > minDistToInteract;}
    }

    
    void OnMouseEnter()
    {
        IsInside = true;
        if((transform.position - player.position).magnitude > minDistToInteract){
            highlight.currentState = ItemHighlightStates.TooFartoInteract;  
        }else{
            highlight.currentState = ItemHighlightStates.Normal;
        }
    }
    private void OnMouseExit()
    {
        IsInside = false;
        highlight.currentState = ItemHighlightStates.Hidden;
    }


    public void OnInteract()
    {
        if(item != null)
        {
            item.Pick();
            FindObjectOfType<InventoryInputManager>().OpenInventory();
        }
        if(npc != null)
        {
            npc.StartDialogue();
            FindObjectOfType<InventoryInputManager>().OpenInventory();
        }
    }

    private void InteractionLog()
    {
        if(item)
        {
            FindObjectOfType<DialogueZone>().AddInteractionLog(item
              .Item.InteractionLogDetail);
        }
        if(npc != null)
        {

            FindObjectOfType<DialogueZone>().AddInteractionLog(npc.InteractLog);
        }
    }

    public void OnView()
    {
      //
    }

    private void ViewLog()
    {
        if(item)
        {
            FindObjectOfType<DialogueZone>().AddViewLog(item
             .Item.ViewLogDetail);
        }
        if(npc)
        {
            FindObjectOfType<DialogueZone>().AddViewLog(npc
                .ViewLog);
        }
    }


}
