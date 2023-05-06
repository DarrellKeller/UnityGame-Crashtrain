using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    public chatItem ChatItemPref;
    public Transform chatItemTr;
    public List<chatItem> msgs;
    public static ChatManager manager;
    public TMPro.TextMeshProUGUI inputText;
    private void Awake()
    {
        manager = this;
    }
    // Start is called before the first frame update
    ScrollRect scrollRect;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void AddMsg(Sprite s, string nm, string msg, bool isme = false,bool _log=false){
        //ScrollRect scrollRect = chatItemTr.GetComponentInParent<ScrollRect>();
       // scrollRect.verticalNormalizedPosition = 0;
        chatItem chat = Instantiate<chatItem>(ChatItemPref,chatItemTr);
        chat.Set(s,nm,msg,isme,_log);
        msgs.Add(chat);
        //ScrollRect scrollRect = chatItemTr.GetComponentInParent<ScrollRect>();
        //scrollRect.verticalNormalizedPosition = 0;
        UpdateScrollBar();
    }
    public void UpdateScrollBar() 
    {
        ScrollRect scrollRect = chatItemTr.GetComponentInParent<ScrollRect>();
        Canvas.ForceUpdateCanvases();
       // scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollRect.verticalNormalizedPosition,0f,1f);
       
        scrollRect.verticalNormalizedPosition = 0;
    }
    public void ClearChatInput() 
    {
        inputText.text = "";
        
    }
}
