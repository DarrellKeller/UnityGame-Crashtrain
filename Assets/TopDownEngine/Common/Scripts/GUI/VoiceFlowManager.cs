using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System.Text;
using MoreMountains.InventoryEngine;
using MoreMountains.TopDownEngine;
using TMPro;
using System;
using Newtonsoft.Json;

public class VoiceFlowManager : MonoBehaviour
{

    public static VoiceFlowManager manager;

    private void Awake()
    {
        manager = this;
    }

    string VersionId = "6325656c64484143a984ae27";
    string ApiKey = "VF.DM.6328c179b6503d000741dd2f.iMz8Edbuxati1JMS";
    string user = "user_123";

    public static string resp = "...";
    public GameObject UserInput;
    public InventoryInputManager disp;

    bool userEntered;
    public bool UserEntered {
        get { return userEntered; }
        set { userEntered = value; }
    }

    private void Update()
    {
        if(DialogueZone.IsDialogueActive) {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                UserEntered = true;
                ChatManager.manager.ClearChatInput();
            }
        }
    }


    public IEnumerator GetResponse(string payload) {
        Debug.Log("Sending request to Voiceflow API with payload: " + payload);

        rawData raw = new rawData() { type = "text",payload = payload };
            ActionBotChat b = new ActionBotChat { action=raw };
            string postData = JsonConvert.SerializeObject(b);

        byte[] bytes=null;
        try 
        {
            bytes =Encoding.UTF8.GetBytes(postData);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
       

            using(UnityWebRequest www = UnityWebRequest.Put("https://general-runtime.voiceflow.com/state/user/" + user + "/interact",bytes))
            {
                www.SetRequestHeader("Content-Type","application/json");
            www.SetRequestHeader("Authorization",ApiKey);
            www.method = "POST";
            www.SetRequestHeader("versionID",VersionId);
            yield return www.SendWebRequest();
            List<ChatBotResponse> chatBotResponses= JsonConvert.DeserializeObject<List<ChatBotResponse>>(www.downloadHandler.text);


            resp = chatBotResponses.Find(r => r.type == "text")?.payload.message ?? "";
            Debug.Log("Received response from Voiceflow API: " + resp);
            }

       
        //
    }

    private byte[] GetBytes(string postData)
    {
        throw new NotImplementedException();
    }
}
public class rawData
{
    public string type = "text";
    public string payload = "";
}
public class ActionBotChat 
{
    public rawData action;
}

///Response classes
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class Child
{
    public string text { get; set; }
}

public class Content
{
    public List<Child> children { get; set; }
}

public class Payload
{
    public Slate slate { get; set; }
    public string message { get; set; }
}

public class ChatBotResponse
{
    public string type { get; set; }
    public Payload payload { get; set; }
}

public class Slate
{
    public string id { get; set; }
    public List<Content> content { get; set; }
}




