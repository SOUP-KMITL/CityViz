using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LitJson;

public struct geolocation
{
    public Information lat, lng;
    //public string lat, lng,latdata,lngdata,latname,lngname;
}
public class keypredicted : MonoBehaviour {

    //List<Information> data = new List<Information>();
    string testurl = "http://203.154.58.227:3000/test";
    public geolocation path = new geolocation();
    public Information key;
    public int myclass;
    string json;
    string[] nKey, qKey, tKey, nKey1, qKey1, tKey1;
    // Use this for initialization
    void Start () {
        //this.data = data;
        //data = prep.data;
        //StartCoroutine(findKey());
	}
    public int findClass(Information key,geolocation location)
    {
        Debug.Log("keytype : " + key.myType);
        if(location.lat.path != "" && location.lng.path != "")
        {
            if (key.myType == "nominal")
            {
                return 1;
            }
            else if (key.myType == "number")
            {
                return 2;
            }
            else if (key.myType == "text")
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            if (key.myType == "nominal")
            {
                return 5;
            }
            else if (key.myType == "number")
            {
                return 6;
            }
            else if (key.myType == "text")
            {
                return 7;
            }
            else
            {
                return 0;
            }

        }
    }
    public IEnumerator findKey(string url)
    {
        preparat prep = GameObject.Find("test").GetComponent<preparat>();
        prep.Starts(url);
        yield return new WaitForSecondsRealtime(.7f);
        //key = checkkey(prep.data);
        //path = checklatlng(prep.data, key.path);
        //myclass = findClass(key, path);
        StartCoroutine(checkMachineKey(prep.data));
        //Debug.Log(key.path + " " + path.lat + " " + path.lng);
    }
    int findArray(string keypath)
    {
        try
        {
            string[] key = keypath.Split('.');
            for (int i = key.Length - 1; i >= 0; i--)
            {
                if (key[i] == "i")
                {
                    return i;
                }
            }
        }
        catch
        {
            return 0;
        }
        
        return 0;

    }
    bool checkSamePath(int index,string firstpath,string secondpath)
    {
        if(firstpath == null)
        {
            return true;
        }
        else
        { 
            //string[] first = new string[0];
            string[] first = firstpath.Split('.');
            string[] second = secondpath.Split('.');

            //Debug.Log(firstpath + " " + secondpath);
            if (second.Length < index + 1)
            {
                return false;
            }
            for (int i = index; i >= 0; i--)
            {
                if (first[i] != second[i])
                {
                    //Debug.Log(first[i] + " " + second[i]);
                    return false;
                }
            }
            for (int i = index + 1; i < first.Length; i++)
            {
                if (first[i] == "i")
                {
                    return false;
                }
            }
            for (int i = index + 1; i < second.Length; i++)
            {
                if (second[i] == "i")
                {
                    return false;
                }
            }

            return true;
        }
       
    }
    public geolocation checklatlng(List<Information> data,string keypath)
    {
        geolocation result = new geolocation();
        result.lat.path = "";
        result.lng.path = "";
        int index = findArray(keypath);
        if(keypath == "")
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].myType == "geolocation-long")
                {
                    result.lng = data[i];
                }
                else if (data[i].myType == "geolocation-lat")
                {
                    result.lat = data[i];
                }
                if (result.lat.path != "" && result.lng.path != "")
                {
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].myType == "geolocation-long")
                {
                    //Debug.Log(data[i].myType + " " + data[i].path);
                    if (checkSamePath(index, keypath, data[i].path))
                    {
                        result.lng = data[i];
                    }
                }
                else if (data[i].myType == "geolocation-lat")
                {
                    if (checkSamePath(index, keypath, data[i].path))
                    {
                        result.lat = data[i];
                    }
                }
                if (result.lat.path != "" && result.lng.path != "")
                {
                    break;
                }
            }
        }
        
        return result;
    }
    Information checkkey(List<Information> data)
    {
        qKey = new string[] { "status", "condition", "level", "result", "label", "maxemo", "data" };
        nKey = new string[] { "index", "main", "value", "total", "average", "normal", "data" };
        tKey = new string[] { "description",  "detail", "text", "alert", "message","data", "" };
        qKey1 = new string[] { "", "conditions", "levels", "results", "labels", "", "" };
        nKey1 = new string[] { "", "", "values", "totals", "avg", "", "data" };
        tKey1 = new string[] { "descriptions", "details", "texts", "", "messages", "data", "" };
        for (int i = 0; i < qKey.Length; i++)
        {
            foreach (Information dat in data)
            {
                if (dat.myType == "nominal" && (dat.name.ToLower() == qKey[i]|| dat.name.ToLower() == qKey1[i]))
                {
                    return dat;
                }
            }
            foreach (Information dat in data)
            {
                if (dat.myType == "number" && (dat.name.ToLower() == nKey[i] || dat.name.ToLower() == nKey1[i]))
                {
                    return dat;
                }
            }
            foreach (Information dat in data)
            {
                if (dat.myType == "text" && (dat.name.ToLower() == tKey[i] || dat.name.ToLower() == tKey1[i]))
                {
                    return dat;
                }
            }
        }
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].myType != "geolocation-long" &&
                data[i].myType != "geolocation-lat" &&
                data[i].myType != "time")
            {
                return data[i];
            }
        }
        Information temp = new Information();
        temp.path = "";
        return temp;
    }
    public IEnumerator checkMachineKey(List<Information> data)
    {
        
        yield return StartCoroutine(waitCheckMachineKey(data,testurl,1));//new WaitForSecondsRealtime(0.9f);
        //Debug.Log("k : "+key.path);
        path = checklatlng(data, key.path);
        myclass = findClass(key, path);

    }
    public IEnumerator waitCheckMachineKey(List<Information> data,string url,int Case)
    {
        json = "[";
        int number = 0;
        string path = "";
        data = data.OrderBy(dat => dat.path).ToList();
        for(int i = 0; i < data.Count+1; i++)
        {
            if (i == 0)
            {
                path = data[i].path;
                number++;
            }
            else if (i == data.Count)
            {
                int mt = 0, t = 0;
                if (data[i - 1].myType == "text")
                {
                    mt = 1;
                }
                else if (data[i - 1].myType == "geolocation-long" || data[i - 1].myType == "geolocation-lat")
                {
                    mt = 2;
                }
                else if (data[i - 1].myType == "nominal")
                {
                    mt = 3;
                }
                else if (data[i - 1].myType == "number")
                {
                    mt = 4;
                }
                else if (data[i - 1].myType == "time")
                {
                    mt = 5;
                }
                if (data[i - 1].dataType == "boolean")
                {
                    t = 1;
                }
                else if (data[i - 1].dataType == "float")
                {
                    t = 2;
                }
                else if (data[i - 1].dataType == "int")
                {
                    t = 3;
                }
                else if (data[i - 1].dataType == "string")
                {
                    t = 4;
                }
                json = json + "{\"Name\":\"" + data[i - 1].name +"\",\"path\":\"" + data[i - 1].path + "\",\"Number\":" + number + ",\"myType\":" + mt.ToString() + ",\"Type\":" + t.ToString() + "},";
                number = 1;
            }
            else if (path != data[i].path)
            {
                int mt = 0, t = 0;
                if (data[i - 1].myType == "text")
                {
                    mt = 1;
                }
                else if (data[i - 1].myType == "geolocation-long" || data[i - 1].myType == "geolocation-lat")
                {
                    mt = 2;
                }
                else if (data[i - 1].myType == "nominal")
                {
                    mt = 3;
                }
                else if (data[i - 1].myType == "number")
                {
                    mt = 4;
                }
                else if (data[i - 1].myType == "time")
                {
                    mt = 5;
                }
                if (data[i - 1].dataType == "boolean")
                {
                    t = 1;
                }
                else if (data[i - 1].dataType == "float")
                {
                    t = 2;
                }
                else if (data[i - 1].dataType == "int")
                {
                    t = 3;
                }
                else if (data[i - 1].dataType == "string")
                {
                    t = 4;
                }
                json = json + "{\"Name\":\"" + data[i - 1].name + "\",\"path\":\"" + data[i - 1].path + "\",\"Number\":" + number + ",\"myType\":" + mt.ToString() + ",\"Type\":" + t.ToString() + "},";
                number = 1;
                path = data[i].path;
            }
            else
            {
                number++;
            }
        }
        json = json.Substring(0, json.Length - 1)+"]";
        
        var encoding = new System.Text.UTF8Encoding();
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        byte[] formData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
        
        WWW www = new WWW(url, formData, postHeader);
        yield return www;

        if(Case == 1)
        {
            //test set
            StartCoroutine(WaitForRequest(www, data, json));
        }

    }
    public Information pathToInformation(List<Information> data, string path)
    {
        foreach(Information a in data)
        {
            if(a.path == path)
            {
                return a;
            }
        }
        return new Information();
    }
    private IEnumerator WaitForRequest(WWW www,List<Information> data,string json)
    {
        if (www.error != null)
        {
            Debug.Log("There was an error sending request: " + www.error);
        }
        else
        {
            key.name = "";
            JsonData d = JsonMapper.ToObject(www.text);
            Debug.Log("res : " + www.text);
            for (int i = 0; i < d.Count; i++)
            {
                //Debug.Log("no: " + d[i]["no"].ToString() +"result: |" + d[i]["result"].ToString()+"|");
                if (float.Parse(d[i]["result"].ToString()) == 1)
                {
                    Debug.Log(d[i]["no"].ToString());
                    key = pathToInformation(data, JsonMapper.ToObject(json)[i]["path"].ToString());
                    break;
                }
            }
        }
        if(key.name == "")
        {
            key = checkkey(data);
        }
        yield return key;
    }
    
    string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_data2.csv";
#elif UNITY_ANDROID
            return Application.persistentDataPath+"Saved_data2.csv";
#elif UNITY_IPHONE
            return Application.persistentDataPath+"/"+"Saved_data2.csv";
#else
            return Application.dataPath +"/"+"Saved_data2.csv";
#endif
    }
}
