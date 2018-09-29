using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class show_Key : MonoBehaviour {
    float keyBoxH = 53f;
    float top;
    float panelwidth = 748f;
    int count;
    RectTransform panel;

	// Use this for initialization
	void Start () {
        count = 0;
        panel = this.GetComponent<RectTransform>();
        //Debug.Log(panel.name);
        /*
        createNewGroup("test1", 4);
        addKey("test1", "pathtest","test");
        addKey("test1", "pathtest", "test");
        addKey("test1", "pathtest", "test");
        addKey("test1", "pathtest", "test");
        setLatLong("test1", "latnam", "lngnam", "latpath", "lngpath");
        createNewGroup("test2", 2);
        addKey("test2", "pathtest", "test");
        addKey("test2", "pathtest", "test");
        setLatLong("test2", "test2", "test2", "test2", "test2");
        
        createNewGroup("test3", 3);
        */
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void createNewGroup(string nameOfGroup,int numberOfKey) {
        if (count == 0) { count = 1; }
        float x = 0;
        GameObject temp;
        GameObject temp2;
        RectTransform container2;
        temp2 = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2"); // edit file nameeee
        container2 = temp2.GetComponent<RectTransform>();
        temp = new GameObject();
        temp.transform.SetParent(panel);
        temp.name = nameOfGroup;
        
        temp.AddComponent<RectTransform>();
        RectTransform tempRect = temp.GetComponent<RectTransform>();
        temp.transform.localScale = new Vector3(1f, 1f, 1f);
        if (numberOfKey == 1) {
            numberOfKey = 2;
        }
        tempRect.sizeDelta = new Vector2(panelwidth, keyBoxH * numberOfKey);
        temp.transform.localPosition = new Vector3(0f, 0f, 0f);
        GameObject tempDetail;
        tempDetail = Instantiate(Resources.Load("defaultGroup", typeof(GameObject))) as GameObject;
        tempDetail.transform.SetParent(temp.transform);
        tempDetail.transform.localScale = new Vector3(1f, 1f, 1f);
        RectTransform tempRect2 = tempDetail.GetComponent<RectTransform>();
        tempRect2.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, tempRect2.rect.width);
        tempRect2.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, tempRect2.rect.height);
        temp.transform.SetParent(container2.transform);
        temp.transform.localScale = new Vector3(1f, 1f, 1f);
        tempDetail.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void addKey(string nameOfGroup, string keyName,string keyPath) {

        GameObject temp = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + nameOfGroup + "/defaultGroup(Clone)/key");
        GameObject tempKey = Instantiate(Resources.Load("key_button", typeof(GameObject))) as GameObject;
        tempKey.name = keyPath;
        tempKey.transform.SetParent(temp.transform);
        tempKey.transform.localScale = new Vector3(1f, 1f, 1f);
        Text[] allText = tempKey.GetComponentsInChildren<Text>();
        allText[0].text = keyName;
        allText[2].text = keyPath;

    }
   public  void setLatLong(string nameOfGroup, string latName, string lngName, string latPath, string lngPath)
    {

        GameObject temp = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + nameOfGroup + "/defaultGroup(Clone)/latlngNull");
        Text[] allText = temp.GetComponentsInChildren<Text>();
        if (latName != "") { 
        allText[0].text = latName;
        allText[1].text = lngName;
        allText[2].text = "Path:"+ latPath;
        allText[3].text = "Path: " +lngPath;
        }
        else { 
            allText[0].text = "none";
            allText[1].text = "none";
            allText[2].text = "";
            allText[3].text = "";
        }
    }
}
