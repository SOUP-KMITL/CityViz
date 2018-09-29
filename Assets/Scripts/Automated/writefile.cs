using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;


public class writefile : MonoBehaviour {

    private List<string[]> data = new List<string[]>();
    string trainurl = "http://203.154.58.227:3000/train";
    // Use this for initialization
    void Start () {
		
	}
    public IEnumerator SaveToTrainFile(string keypath)
    {
        preparat prep = GameObject.Find("test").GetComponent<preparat>();
        keypredicted kp = GameObject.Find("test").GetComponent<keypredicted>();
        List<Information> key = new List<Information>();
        key.Add(kp.pathToInformation(prep.data,keypath));
        yield return StartCoroutine(kp.waitCheckMachineKey(key, trainurl,2));
    }
    public void Save(inputdata temp)
    {
        StartCoroutine(SaveToTrainFile(temp.keypath));
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[7];
        rowDataTemp[0] = temp.name;
        rowDataTemp[1] = temp.url;
        rowDataTemp[2] = temp.keypath;
        rowDataTemp[3] = temp.latpath;
        rowDataTemp[4] = temp.lngpath;
        rowDataTemp[5] = temp.myclass;
        rowDataTemp[6] = "enable";
        data.Add(rowDataTemp);

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }

        string filePath = getPath();
        string text = "";
        try
        {
            text = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            Debug.Log("Don't have CSV file");
            text = "";
        }

        StreamWriter outStream = File.CreateText(filePath);
        outStream.Write(text + sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
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
