using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;

public class listname : MonoBehaviour
{

    private List<string[]> data = new List<string[]>();
    int currentTime = 1;
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
    void Start () {
		//list();
	}
	void list()
    {
        string filePath = getPath();
        string[,] texts = LoadCsv(filePath);
        for (int i = 0; i < texts.GetLength(0); i++)
        {
            GameObject go = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + texts[i,0]);
            Destroy(go);
        }
        for (int i = 0; i < texts.GetLength(0); i++)
        {
            string name = texts[i, 0];
            string status = texts[i, 6];
            StartCoroutine(createcv(i, name,status));
        }

    }
    IEnumerator createcv(int i,string name,string status)
    {
        deleteurl deleteurl = new deleteurl();
        disableurl disableurl = new disableurl();
        GameObject go = Instantiate(Resources.Load("Prefab/detail")) as GameObject;
        RawImage img = go.GetComponent<RawImage>();
        //img.rectTransform.position = new Vector2(0, 0);
        go.name = name;
        go.transform.parent = GameObject.Find("16_9_main/settingBar/Panel/container4setting").transform;
        img.rectTransform.localScale = new Vector3(1, 1, 1);
        img.rectTransform.localPosition = new Vector2(0.00042248f, 395 - ((i) * 88));
        //img.rectTransform.localPosition = new Vector2(0.00042248f, 395 - ((i+7) * 88));

        yield return new WaitForSecondsRealtime(0.0005f);

        GameObject txt = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name + "/Text");
        Text tt = txt.GetComponent<Text>();
        tt.text = name;
        GameObject delete = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name + "/delete");
        Button del = delete.GetComponent<Button>();
        //Debug.Log(name);
        del.onClick.AddListener(() => { deleteurl.delete(name); });
        GameObject eye = GameObject.Find("16_9_main/settingBar/Panel/container4setting/" + name + "/eye");
        Button eyebutton = eye.GetComponent<Button>();
        //Debug.Log(name + " " + status);
        if (status == "enable")
        {
            eyebutton.onClick.AddListener(() => { disableurl.disable(name); });
        }
        if (status == "disable")
        {
            Image eyeimg = eye.GetComponent<Image>();
            eyeimg.sprite = Resources.Load<Sprite>("ui_components/button/eye2");
            eyebutton.onClick.AddListener(() => { disableurl.enable(name); });
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if ((int)Time.time % 1 == 0 && (int)Time.time != currentTime)
        {
            currentTime = (int)Time.time;
            //Debug.Log("time " + Time.time);
            list();
            //Debug.Log(Models.Count);
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
