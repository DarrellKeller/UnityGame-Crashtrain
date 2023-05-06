using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class chatItem : MonoBehaviour
{

    public Image Meim;
    public Image Otherim;
    public TextMeshProUGUI MyName;
    public TextMeshProUGUI MY_Image_Text_PlaceHolder_Latter;
    public TextMeshProUGUI MyMessage;
    public TextMeshProUGUI OtherName;
    public TextMeshProUGUI Other_Image_Text_PlaceHolder_Latter;
    public TextMeshProUGUI OtherMessage;

    public GameObject MeObj, OtherObj;
    public bool IsMe;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Sprite prof_im, string nm, string msg, bool IsMe = false,bool log=false){
        if(IsMe){
            Meim.sprite = prof_im;
            MyName.text = nm;
            MyMessage.text = msg;
            MY_Image_Text_PlaceHolder_Latter.text = nm.ToCharArray()[0].ToString();
        }else{
            Otherim.sprite = prof_im;
            OtherName.text = nm;
            OtherMessage.text = msg;
            Other_Image_Text_PlaceHolder_Latter.text = nm.ToCharArray()[0].ToString();
        }
        if(prof_im == null) 
        {
            
                if(IsMe)
                {
                    Meim.enabled = false;
                    MY_Image_Text_PlaceHolder_Latter.enabled = true;
                MY_Image_Text_PlaceHolder_Latter.color = Color.yellow;
                }
                else
                {
                    Otherim.enabled = false;
                    Other_Image_Text_PlaceHolder_Latter.enabled = true;
                    Other_Image_Text_PlaceHolder_Latter.color = Color.cyan;
            }
            
        }
        if(log)
        {
            Meim.enabled = true;
            Otherim.enabled = true;
            MY_Image_Text_PlaceHolder_Latter.enabled = false;
            Other_Image_Text_PlaceHolder_Latter.enabled = false;
        }
        MeObj.SetActive(IsMe);
        OtherObj.SetActive(!IsMe);
    }
}
