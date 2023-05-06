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
    string Bearer = "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";
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

    // public GetResponse(string userData){
    //     StartCoroutine(userData);
    // }

    public IEnumerator GetResponse(string payload) {
       // JSONObject o = new JSONObject();
       // JSONObject o1 = new JSONObject();
       // JSONObject o2 = new JSONObject();
       // o1.Add("type","text");
       // o2.Add("payload",payload);
       // o.Add("action",JsonUtility.ToJson(o1.ToString())+ JsonUtility.ToJson(o2.ToString()));

       // rawData rawData = new rawData() { type = "text",payload = payload };
       // ActionBotChat action = new ActionBotChat() { action = rawData };
       // string _data = Newtonsoft.Json.JsonConvert.SerializeObject(action).ToString();
       // Debug.LogError(Newtonsoft.Json.JsonConvert.SerializeObject(action));
       // byte[] byteArray = Encoding.UTF8.GetBytes(_data);
       // UnityWebRequest req = UnityWebRequest.Post("https://general-runtime.voiceflow.com/state/user/" + user + "/interact",_data);
       // req.SetRequestHeader("Authorization",ApiKey);
       // req.method = "POST";
       // req.SetRequestHeader("Content-Type","application/json");
       // req.SetRequestHeader("versionID",VersionId);
       // yield return req.SendWebRequest();
       // Debug.Log(req.downloadHandler.text);
       // JSONNode data = (JSONNode)JSON.Parse(req.downloadHandler.text);
       // Debug.Log(req.downloadHandler.text);
       //// Debug.Log(data[0]);
       // //Debug.Log(data[0]["payload"]["message"]);
       // VoiceFlowManager.resp = data[0]["payload"]["message"];
       // if(resp == null|| resp == "") { resp = "..."; }
       // yield return null;
        //

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
            //www.SetRequestHeader("Content-Type","application/json");
            www.SetRequestHeader("versionID",VersionId);
            yield return www.SendWebRequest();
            List<ChatBotResponse> chatBotResponses= JsonConvert.DeserializeObject<List<ChatBotResponse>>(www.downloadHandler.text);
            //Debug.LogError("REPLY : "+ www.downloadHandler.text);

            resp = chatBotResponses[1].payload.message;
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




