using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemHighlightStates{
    Hidden,
    Normal,
    TooFartoInteract,
    View,
    Interact,
}

public class HighlightObject : MonoBehaviour
{

    public ItemHighlightStates currentState;
    public Image arrow, mouse, view;
    public CanvasGroup group;
    public Sprite arrow_disable, arrow_normal, arrow_highlight;
    public Sprite  view_normal, view_highlight;
    public Sprite mouse_normal, mouse_arrow, mouse_view;
    public Interactable root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetState();
    }

    public void SetState(){
        switch (currentState)
        {
            case ItemHighlightStates.Hidden: group.alpha = 0; break;
            case ItemHighlightStates.Normal:
            group.alpha = 1f;
            arrow.sprite = arrow_normal;
            view.sprite = view_normal;
            mouse.sprite = mouse_normal;
            break;
            case ItemHighlightStates.TooFartoInteract:
            group.alpha = 1f;
            arrow.sprite = arrow_disable;
            view.sprite = view_normal;
            mouse.sprite = mouse_normal;
            break;
            case ItemHighlightStates.View:
            group.alpha = 1f;
            arrow.sprite =  root.IsFartointeract ? arrow_disable :arrow_normal;
            view.sprite = view_highlight;
            mouse.sprite = mouse_view;
            break;
            case ItemHighlightStates.Interact:
            group.alpha = 1f;
            arrow.sprite = arrow_highlight;
            view.sprite = view_normal;
            mouse.sprite = mouse_arrow;
            break;
        }
    }
}
