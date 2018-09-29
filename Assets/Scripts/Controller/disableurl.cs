using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class disableurl : MonoBehaviour
{

    private List<string[]> data = new List<string[]>();
    public string name;
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
    public void disable(string name)
    {
        // Creating First row of titles manually.
        string filePath = getPath();
        string text = "";
        string[,] texts = LoadCsv(filePath);
        string[] lines = File.ReadAllLines(filePath);
        try
        {
            GameObject go = GameObject.Find(name);
            Destroy(go);
        }
        catch
        {

        }
        GameObject eye = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name + "/eye");
        Image eyeimg = eye.GetComponent<Image>();
        eyeimg.sprite = Resources.Load<Sprite>("ui_components/button/eye2");

        for (int i = 0; i < texts.GetLength(0); i++)
        {
            if (texts[i, 0] == name)
            {
                string temp = "";
                for (int j = 0; j < texts.GetLength(1); j++)
                {
                    if (j == 6)
                    {
                        temp = temp + ",disable";
                    }
                    else if (j == 0)
                    {
                        temp = texts[i, j];
                    }
                    else
                    {
                        temp = temp + ',' + texts[i, j];
                    }
                }
                text = text + temp + '\n';
            }
            else
            {
                text = text + lines[i]+'\n';
            }
        }

        StreamWriter outStream = File.CreateText(filePath);
        outStream.Write(text);
        outStream.Close();
    }
    public void enable(string name)
    {
        // Creating First row of titles manually.
        string filePath = getPath();
        string text = "";
        string[,] texts = LoadCsv(filePath);
        string[] lines = File.ReadAllLines(filePath);
    
        GameObject eye = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name + "/eye");
        Image eyeimg = eye.GetComponent<Image>();
        eyeimg.sprite = Resources.Load<Sprite>("ui_components/button/eye1");

        for (int i = 0; i < texts.GetLength(0); i++)
        {
            if (texts[i, 0] == name)
            {
                string temp = "";
                for (int j = 0; j < texts.GetLength(1); j++)
                {
                    if (j == 6)
                    {
                        temp = temp + ",enable";
                    }
                    else if (j == 0)
                    {
                        temp = texts[i, j];
                    }
                    else
                    {
                        temp = temp + ',' + texts[i, j];
                    }
                }
                text = text + temp + '\n';
            }
            else
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
