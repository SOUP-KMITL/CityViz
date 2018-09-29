using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

struct InfoData
{
    public string url, name, latpath, lngpath, keypath, myclass;
}
public class visualization : MonoBehaviour
{
    
    InfoData csv = new InfoData();
    int currentTime = 1;
    List<GameObject> Models = new List<GameObject>();
    List<GameObject> Parent = new List<GameObject>();
    List<float> average = new List<float>();
    List<string> nominal = new List<string>();
    string[,] LoadCsv(string filename)
    {
        // Get the file's text.
        string whole_file = File.ReadAllText(filename);

        // Split into lines.
        whole_file = whole_file.Replace('\n', '\r');
        string[] lines = whole_file.Split(new char[] { '\r' },
            StringSplitOptions.RemoveEmptyEntries);

        // See how many rows and columns there are.
        int num_rows = lines.Length;
        int num_cols = lines[0].Split(',').Length;

        // Allocate the data array.
        string[,] values = new string[num_rows, num_cols];

        // Load the array.
        for (int r = 0; r < num_rows; r++)
        {
            string[] line_r = lines[r].Split(',');
            for (int c = 0; c < num_cols; c++)
            {
                values[r, c] = line_r[c];
            }
        }

        // Return the values.
        return values;
    }
    void getCSV()
    {
        string filePath = getPath();
        string[,] data = LoadCsv(filePath);
        
        if(GameObject.Find("16_9_main/sideBar/Panel/container")!=null)
        { 
            createAndMove crt = GameObject.Find("16_9_main/sideBar/Panel/container").GetComponent<createAndMove>();
            for (int i = 0; i < Parent.Count; i++)
            {
                //Debug.Log("box " + Parent[i].name);
                crt.deleteBox(Parent[i].name);
            }
        }
        try
        {
            for (int i = 0; i < Parent.Count; i++)
            {
                Destroy(Parent[i]);
            }
            Parent.Clear();
            //Debug.Log("clear model");
            Models.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        for (int i = 0; i < data.GetLength(0); i++)
        {
            
            csv.name = data[i, 0];
            csv.url = data[i, 1];
            csv.keypath = data[i, 2];
            csv.latpath = data[i, 3];
            csv.lngpath = data[i, 4];
            csv.myclass = data[i, 5];
            if(data[i,6] != "disable")
            {
                Parent.Add(new GameObject(csv.name));
                StartCoroutine(getData(csv));
            }
        }
    }
    int checkSamePath(InfoData csv)
    {
        int minLength = 99;
        string[] latList = csv.latpath.Split('.');
        string[] lngList = csv.lngpath.Split('.');
        string[] keyList = csv.keypath.Split('.');
        if(csv.myclass == "4")
        {
            //no key
            if (latList.Length <= minLength)
            {
                minLength = latList.Length;
            }
            if (lngList.Length <= minLength)
            {
                minLength = lngList.Length;
            }
            for (int i = 0; i < minLength; i++)
            {
                if (!(latList[i] == lngList[i]))
                {
                    minLength = i;
                    break;
                }
            }
        }
        else if (csv.myclass == "5" || csv.myclass == "6" || csv.myclass == "7")
        {
            //no lat lng
            if (keyList.Length <= minLength)
            {
                minLength = keyList.Length;
            }
        }
        else if (csv.myclass == "3" || csv.myclass == "2" || csv.myclass == "1")
        {
            //key lat lng
            if (latList.Length <= minLength)
            {
                minLength = latList.Length;
            }
            if (lngList.Length <= minLength)
            {
                minLength = lngList.Length;
            }
            if (keyList.Length <= minLength)
            {
                minLength = keyList.Length;
            }
            for (int i = 0; i < minLength; i++)
            {
                if (!(latList[i] == lngList[i] && lngList[i] == keyList[i]))
                {
                    minLength = i;
                    break;
                }
            }
        }

        //Debug.Log(minLength);
        return minLength;
    }
    string newPath(string[] path,int index)
    {
        string result = "";
        for(int i = index; i < path.Length; i++)
        {
            //Debug.Log(i + " : " +result);
            if (result == "")
            {
                result = path[i];
            }
            else
            {
                result = result + "." + path[i];
            }
        }
        //Debug.Log(result);
        return result;
    }
    void loopPath(JsonData json,InfoData csv, string path, int len)
    {
        for(int i = 0; i < json.Count; i++)
        {
            defaultPath(json[i], csv,path, len);
        }
    }
    string setData(JsonData json, string dataPath,int minLen)
    {
        JsonData result = json;
        //Debug.Log(dataPath);
        string[] list = dataPath.Split('.');
        for(int i = minLen; i < list.Length; i++)
        {
            if (list[i] == "0")
            {
                result = result[0];
            }
            else if (list[i] == "1")
            {
                result = result[1];
            }
            else
            {
                result = result[list[i]];
            }
        }
        return result.ToString();
    }
    void defaultPath(JsonData json,InfoData csv,string path,int len)
    {
        JsonData result = json;
        string[] list = path.Split('.');
        bool check = true;

        //Debug.Log(len);

        for (int i = 0; i < len; i++)
        {
            //Debug.Log(list[i]);
            if (list[i] == "i")
            {
                i++;
                loopPath(result, csv, newPath(list, i), len - i);
                check = false;
                break;
            }
            else if(list[i] != "")
            {
                result = result[list[i]];
                check = true;
            }
        }
        if (check)
        {
            display(result, csv);
        }
        //Debug.Log(result.ToString());
    }
    void display(JsonData result, InfoData csv)
    {
        //Debug.Log("value : " + value + ", lat : " + lat + ", lng :" + lng);
        if (csv.myclass == "1")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));
            string lat = setData(result, csv.latpath, checkSamePath(csv));
            string lng = setData(result, csv.lngpath, checkSamePath(csv));

            //show icon quality
            Models.Add(showQuality(value, lat, lng,csv));
            cvQuality(value, csv);
            
            Models[Models.Count - 1].name = "Model[" + (Models.Count - 1) + "]";
            Models[Models.Count - 1].transform.parent = GameObject.Find("/"+csv.name).transform;

            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/Text").GetComponent<TextMesh>().text = value;
            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/Name/name").GetComponent<TextMesh>().text = csv.name;


        }
        else if (csv.myclass == "2")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));
            string lat = setData(result, csv.latpath, checkSamePath(csv));
            string lng = setData(result, csv.lngpath, checkSamePath(csv));

            //number on 3dtext
            cvNumber(value, csv);

            Models.Add(showNumber(value, lat, lng,csv));
            Models[Models.Count - 1].name = "Model[" + (Models.Count - 1) + "]";
            Models[Models.Count - 1].transform.parent = GameObject.Find("/"+csv.name).transform;


            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/Text").GetComponent<TextMesh>().text = value;
            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/name").GetComponent<TextMesh>().text = "-"+csv.name+"-";

        }
        else if (csv.myclass == "3")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));
            string lat = setData(result, csv.latpath, checkSamePath(csv));
            string lng = setData(result, csv.lngpath, checkSamePath(csv));

            //text on 3dtext

            cvText(value, csv);

            Models.Add(showText(value, lat, lng));
            Models[Models.Count - 1].name = "Model[" + (Models.Count - 1) + "]";
            Models[Models.Count - 1].transform.parent = GameObject.Find("/"+csv.name).transform;

            string temp = value;
            for(int index = 30; index < value.Length; index += 30)
            {
                temp = temp.Insert(index, "\n");
            //Debug.Log(temp);
            }
            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/Text").GetComponent<TextMesh>().text = temp;
            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/name").GetComponent<TextMesh>().text = "-" + csv.name + "-";
        }
        else if (csv.myclass == "4")
        {
            string lat = setData(result, csv.latpath, checkSamePath(csv));
            string lng = setData(result, csv.lngpath, checkSamePath(csv));

            //show point in map 
            //no key
            Models.Add(showPointer(lat, lng));
            Models[Models.Count - 1].name = "Model[" + (Models.Count - 1) + "]";
            Models[Models.Count - 1].transform.parent = GameObject.Find(csv.name).transform;

            GameObject.Find(csv.name + "/Model[" + (Models.Count - 1) + "]" + "/Name/name").GetComponent<TextMesh>().text = csv.name;
        }
        else if (csv.myclass == "5")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));

            //show cv quality
            cvQuality(value, csv);
            
        }
        else if (csv.myclass == "6")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));

            //cv number
            cvNumber(value, csv);
            
        }
        else if (csv.myclass == "7")
        {
            string value = setData(result, csv.keypath, checkSamePath(csv));

            cvText(value, csv);
        }
    }
    IEnumerator getData(InfoData csv)
    {
        WWW www = new WWW(csv.url);
        yield return www;
        JsonData data = JsonMapper.ToObject(www.text);
        //Debug.Log(www.text);
        average.Clear();
        nominal.Clear();
        if (csv.keypath != "")
        {
            defaultPath(data, csv, csv.keypath, checkSamePath(csv));
        }
        else if (csv.latpath != "")
        {
            defaultPath(data, csv, csv.latpath, checkSamePath(csv));
        }
    }
	void Update () {
        if ((int)Time.time % 15 == 0 && (int)Time.time != currentTime)
        {
            currentTime = (int)Time.time;
            getCSV();
        }

        for (int i = 0; i < Models.Count; i++)
        {
            try
            {
                Models[i].transform.LookAt(Camera.main.transform);
                Models[i].transform.rotation = Quaternion.Euler(0, Models[i].transform.localEulerAngles.y - 180, Models[i].transform.localEulerAngles.z);

            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }
    GameObject showPointer(string latstr, string lngstr)
    {
        float lat = float.Parse(latstr);
        float lng = float.Parse(lngstr);
        float z = (float)(111278.1275 * lat + 71.0495 * lng - 1536784.7526);
        float x = (float)(108069.6489 * lng - 10864722.6152);
        return Instantiate(Resources.Load("automatedicon/" + "pin"), new Vector3(x, 170, z), Quaternion.identity) as GameObject;
    }
    GameObject showNumber(string value, string latstr, string lngstr,InfoData csv)
    {
        float lat = float.Parse(latstr);
        float lng = float.Parse(lngstr);
        float z = (float)(111278.1275 * lat + 71.0495 * lng - 1536784.7526);
        float x = (float)(108069.6489 * lng - 10864722.6152);

        return Instantiate(Resources.Load("automatedicon/" + "number"), new Vector3(x, 150, z), Quaternion.identity) as GameObject;
        /*
        GameObject temp = Instantiate(Resources.Load("number"), new Vector3(x, 150, z), Quaternion.identity) as GameObject;
        temp.name = "Model[" + Models.Count + "]";
        temp.transform.parent = GameObject.Find(csv.name).transform;

        GameObject text = GameObject.Find(csv.name + "/Model[" + Models.Count + "]" + "/Text");
        TextMesh tt = text.GetComponent<TextMesh>();
        tt.text = value;

        return temp;*/
    }
    GameObject showText(string value, string latstr, string lngstr)
    {
        float lat = float.Parse(latstr);
        float lng = float.Parse(lngstr);
        float z = (float)(111278.1275 * lat + 71.0495 * lng - 1536784.7526);
        float x = (float)(108069.6489 * lng - 10864722.6152);
        return Instantiate(Resources.Load("automatedicon/" + "text"), new Vector3(x, 150, z), Quaternion.identity) as GameObject;
    }
    GameObject showQuality(string value, string latstr, string lngstr,InfoData csv)
    {
        float lat = float.Parse(latstr);
        float lng = float.Parse(lngstr);
        float z = (float)(111278.1275 * lat + 71.0495 * lng - 1536784.7526);
        float x = (float)(108069.6489 * lng - 10864722.6152);
        string key = "null";
        if (value.ToLower() == "low" || value.ToLower() == "lower")
        {
            key = "low";
        }
        else if (value.ToLower() == "med" || value.ToLower() == "medium" || value.ToLower() == "normal" || value.ToLower() == "moderate")
        {
            key = "medium";
        }
        else if (value.ToLower() == "high" || value.ToLower() == "higher" || value.ToLower() == "Dangerouse")
        {
            key = "high";
        }
        if (value.ToLower() == "good" || value.ToLower() == "true" || value.ToLower() == "safe")
        {
            key = "good";
        }
        else if (value.ToLower() == "bad" || value.ToLower() == "false" )
        {
            key = "bad";
        }
        else if (value.ToLower() == "fair" )
        {
            key = "fair";
        }
        return Instantiate(Resources.Load("automatedicon/"+key), new Vector3(x, 200, z), Quaternion.identity) as GameObject;
        /*
        GameObject temp = Instantiate(Resources.Load(key), new Vector3(x, 200, z), Quaternion.identity) as GameObject;
        temp.name = "Model[" + Models.Count + "]";
        temp.transform.parent = GameObject.Find(csv.name).transform;

        GameObject text = GameObject.Find(csv.name + "/Model[" + Models.Count+ "]" + "/Text");
        TextMesh tt = text.GetComponent<TextMesh>();
        tt.text = value;

        return temp;*/
    }
    void cvQuality(string value, InfoData csv)
    {
        string str = "";
        string nom = "";
        int check = 0;
        nominal.Add(value.ToLower());
        nominal = nominal.OrderBy(val => val).ToList();
        for(int i = 0; i < nominal.Count; i++)
        {
            if(nom != nominal[i])
            {
                if(i-check > 0)
                {
                    str = str + nominal[i - 1] + " : " + (i - check) +"\t";
                }
                nom = nominal[i];
                check = i;
            }
            //str = str + nominal[i];
        }
        str = str + nominal[nominal.Count-1] + " : " + (nominal.Count - check);
        //Debug.Log(str);
        if (GameObject.Find("16_9_main/sideBar/Panel/container") != null)
        {
            createAndMove crt = GameObject.Find("16_9_main/sideBar/Panel/container").GetComponent<createAndMove>();
            if (GameObject.Find("16_9_main/sideBar/Panel/container/" + csv.name) == null)
            {
                crt.createNewBox(csv.name, str);
            }
            else
            {
                crt.deleteBox(csv.name);
                crt.createNewBox(csv.name, str);
            }
        }
    }
    void cvNumber(string value, InfoData csv)
    {
        string str = "";
        float val = float.Parse(value);
        average.Add(val);
        float min = val;
        float max = val;
        if (GameObject.Find("16_9_main/sideBar/Panel/container") != null)
        {
            createAndMove crt = GameObject.Find("16_9_main/sideBar/Panel/container").GetComponent<createAndMove>();
            if (GameObject.Find("16_9_main/sideBar/Panel/container/" + csv.name) == null)
            {
                crt.createNewBoxSet(csv.name, val.ToString(), min.ToString(), max.ToString());
            }
            else
            {
                str = GameObject.Find("16_9_main/sideBar/Panel/container/" + csv.name + "/minVal").GetComponent<Text>().text;
                if (float.Parse(str) > val)
                {
                    min = val;
                }
                else
                {
                    min = float.Parse(str);
                }
                str = GameObject.Find("16_9_main/sideBar/Panel/container/" + csv.name + "/maxVal").GetComponent<Text>().text;
                if (float.Parse(str) < val)
                {
                    max = val;
                }
                else
                {
                    max = float.Parse(str);
                }

                crt.deleteBox(csv.name);
                crt.createNewBoxSet(csv.name, average.Average().ToString(), min.ToString(), max.ToString());
            }
        }
    }
    void cvText(string value, InfoData csv)
    {
        if (GameObject.Find("16_9_main/sideBar/Panel/container") != null)
        {
            createAndMove crt = GameObject.Find("16_9_main/sideBar/Panel/container").GetComponent<createAndMove>();
            crt.createNewBox(csv.name, value);
        }
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
