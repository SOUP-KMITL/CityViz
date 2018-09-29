using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class test : MonoBehaviour {

    string url = "http://203.154.58.227:3000/sms";
    //string url = "http://161.246.35.220:9000/user/keyword/";
    string json = @"{'user_id': '5aa0d8ff19b23728fc801545'}";
    // Use this for initialization
    void Start () {
        //POST();
        StartCoroutine(teste());
		
	}
	
	// Update is called once per frame
	void Update () {
	}
    public IEnumerator teste()
    {
        preparat prep = GameObject.Find("test").GetComponent<preparat>();
        prep.Starts("http://127.0.0.1:8080/density.json");
        yield return new WaitForSecondsRealtime(.7f);
        //keypredicted key = GameObject.Find("test").GetComponent<keypredicted>();
        //json = key.checkMachineKey(prep.data);
        POST();
    }
    public WWW POST()
    {
        WWW www;
        var encoding = new System.Text.UTF8Encoding();
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        // convert json string to byte
        //json = json.Replace("'", "\"");
        byte[] formData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
        Debug.Log(json);

        www = new WWW(url, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            JsonData d = JsonMapper.ToObject(data.text);
            //Debug.Log("WWW Request: " + data.text);
            for(int i = 0; i < d.Count; i++)
            {
                //Debug.Log("no: " + d[i]["no"].ToString() +"result: |" + d[i]["result"].ToString()+"|");
                if (d[i]["result"].ToString() == "1")
                {
                    Debug.Log("no: " + d[i]["no"].ToString());
                }
            }
        }
    }
}
