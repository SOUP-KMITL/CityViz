using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;

public class deleteurl : MonoBehaviour
{

    private List<string[]> data = new List<string[]>();
    
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
    // Use this for initialization
    void Start()
    {
       
    }
    public void delete(string name)
    {
        // Creating First row of titles manually.
        string filePath = getPath();
        string text = "";
        string[,] texts = LoadCsv(filePath);
        string[] lines = File.ReadAllLines(filePath);
        Debug.Log("delete " + name);
        GameObject go = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name);
        Destroy(go);
        for (int i = 0; i < texts.GetLength(0); i++)
        {
            if (texts[i, 0] != name)
            {
                text = text + lines[i]+'\n';
            }
        }

        StreamWriter outStream = File.CreateText(filePath);
        outStream.Write(text);
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
        return Application.dataPath + "/" + "Saved_data2.csv";
#endif
    }
}
