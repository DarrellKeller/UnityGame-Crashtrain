using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class NPC : MonoBehaviour
{

    public string NpcId;
    public string ViewLog;
    public string InteractLog;

    // Start is called before the first frame update
    void Start()
    {
        FetchDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FetchDialogue(){
        
    }

    public void StartDialogue(){
        gameObject.GetComponentInChildren<DialogueZone>().StartDialogue();
    }

}
