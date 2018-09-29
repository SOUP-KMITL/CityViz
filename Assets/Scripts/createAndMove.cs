using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class createAndMove : MonoBehaviour
{
    RectTransform contain;
    Text[] tempComt;
    Text TempText;
    Text TempVal;
    Text TempMax;
    Text TempMin;
    Text TempAvg;
    float wid;
    float hi;
    float lastChild;
    float boxH = 185f;
    int first = 0;
    int test = 0;
    GameObject childd;
    GameObject tempBox;
    //GameObject[] tempFind;

    // Use this for initialization
    GameObject[] box = new GameObject[7];
    JsonData data;
    IEnumerator tData(string uri)
    {
        WWW www = new WWW(uri);
        yield return www;
        data = JsonMapper.ToObject(www.text);
    }
    void Start()
    {

        contain = GetComponent<RectTransform>();
        /*
        //createNewBox("NCT 127", "TOUCH");
        createNewBox("From Admin", "1");
        //createNewBox("NCT 127", "TOUCH BABY");
        createNewBox("From Admin", "2");
        createNewBox("From Admin", "3");
        createNewBox("From Admin", "4");
        createNewBox("From Admin", "5");
        createNewBox("From Admin", "6");
        createNewBox("From Admin", "peek a boo");
        createNewBox("From Admin", "7");
        createNewBox("From Admin", "8");
        createNewBox("NCT 127", "baby don't stop");
        createNewBox("From Admin", "9");
        createNewBox("From Admin", "10");
        // createNewBoxSet("NCT 127", "25.4","26","28");
        //createNewBox("NCT 127", "12345");
        // createNewBox("NCT 127", "678910");
        //createNewBox("Test3", "12345");
        //createNewBox("Test4", "12345");
        //deleteBox("NCT 127");
        //Debug.Log(contain.childCount);
        // deleteBox("From Admin");

        //createNewBox("NCT U", "Don't you know I'm a boss");*/
    }
    void Update()
    {


    }
    /*
    void Start () {


        contain = GetComponent<RectTransform>();
        //extendContainer();
        //moveContainer();
        //moveContent();
        //createNewBox("bell","13");
        // extendContainer();
        // moveContainer();
        // moveContent();
        //createNewBox("bell", "13");



    }*/

    // Update is called once per frame


    public GameObject createNewBox(string name, string val)
    {
        if (contain.childCount == 0) {
            first = 0;
        }
        if (first != 0)
        {
            contain = GetComponent<RectTransform>();
            
                lastChild = contain.transform.GetChild(contain.childCount - 1).localPosition.y;
                //print(contain.transform.GetChild(contain.childCount - 1).name);
                /*
                if (GameObject.Find("16_9/sideBar/Panel/container/" + name) != null)
                {
                    tempFind = GameObject.Find("16_9/sideBar/Panel/container/" + name);
                    tempComt = tempFind.GetComponentsInChildren<Text>();
                    TempText = tempComt[0];
                    TempVal = tempComt[1];
                    TempText.text = name;
                    TempVal.text = val;
                }
                else {*/
                tempBox = Instantiate(Resources.Load("setText", typeof(GameObject))) as GameObject;
                tempBox.transform.SetParent(contain);
                tempBox.transform.localPosition = new Vector3(0f, lastChild - boxH, 0f);
                tempBox.transform.localScale = new Vector3(1f, 1f, 1f);
                tempComt = tempBox.GetComponentsInChildren<Text>();
                TempText = tempComt[0];
                TempVal = tempComt[1];
                TempText.text = name;
                TempVal.text = val;
                tempBox.name = name;
                tempBox.tag = "notiBox";
            
            //}
        }
        else
        {
            first = 1;
            contain = GetComponent<RectTransform>();
            //lastChild = contain.transform.GetChild(contain.childCount - 1).localPosition.y;
            //print(contain.transform.GetChild(contain.childCount - 1).name);
            tempBox = Instantiate(Resources.Load("setText", typeof(GameObject))) as GameObject;
            tempBox.transform.SetParent(contain);
            tempBox.transform.localPosition = new Vector3(0f, 365f, 0f);
            tempBox.transform.localScale = new Vector3(1f, 1f, 1f);
            tempComt = tempBox.GetComponentsInChildren<Text>();
            TempText = tempComt[0];
            TempVal = tempComt[1];
            TempText.text = name;
            TempVal.text = val;
            tempBox.name = name;
            tempBox.tag = "notiBox";

        }
        return tempBox;

    }
    public GameObject createNewBoxSet(string name, string avg, string min, string max)
    {
        if(contain.childCount == 0)
        {
            first = 0;
        }
        if (first != 0)
        {
            contain = GetComponent<RectTransform>();
            
            lastChild = contain.transform.GetChild(contain.childCount - 1).localPosition.y;
            //print(contain.transform.GetChild(contain.childCount - 1).name);
            /*if (GameObject.Find("16_9/sideBar/Panel/container/" + name) != null)
            {
                tempFind = GameObject.Find("16_9/sideBar/Panel/container/" + name);
                tempComt = tempFind.GetComponentsInChildren<Text>();
                TempText = tempComt[0];
                TempMax = tempComt[4];
                TempMin = tempComt[5];
                TempAvg = tempComt[6];
                TempText.text = name;
                TempMax.text = max;
                TempMin.text = min;
                TempAvg.text = avg;
                
            }
            else
            {*/
            tempBox = Instantiate(Resources.Load("setAvg", typeof(GameObject))) as GameObject;
            tempBox.transform.SetParent(contain);
            tempBox.transform.localPosition = new Vector3(0f, lastChild - boxH, 0f);
            tempBox.transform.localScale = new Vector3(1f, 1f, 1f);
            tempComt = tempBox.GetComponentsInChildren<Text>();
            TempText = tempComt[0];
            TempMax = tempComt[4];
            TempMin = tempComt[5];
            TempAvg = tempComt[6];
            TempText.text = name;
            TempMax.text = max;
            TempMin.text = min;
            TempAvg.text = avg;
            tempBox.name = name;
            tempBox.tag = "notiBox";
            //}
        }
        else
        {
            first = 1;
            contain = GetComponent<RectTransform>();
            //lastChild = contain.transform.GetChild(contain.childCount - 1).localPosition.y;
            //print(contain.transform.GetChild(contain.childCount - 1).name);
            tempBox = Instantiate(Resources.Load("setAvg", typeof(GameObject))) as GameObject;
            tempBox.transform.SetParent(contain);
            tempBox.transform.localPosition = new Vector3(0f, 365f, 0f);
            tempBox.transform.localScale = new Vector3(1f, 1f, 1f);
            tempComt = tempBox.GetComponentsInChildren<Text>();
            TempText = tempComt[0];
            TempMax = tempComt[4];
            TempMin = tempComt[5];
            TempAvg = tempComt[6];
            TempText.text = name;
            TempMax.text = max;
            TempMin.text = min;
            TempAvg.text = avg;
            tempBox.name = name;


        }
        return tempBox;
    }

    public void deleteBox(string nameapi)
    {

        //Debug.Log("in delete loop");
        List<Transform> tempArr = new List<Transform>();
        List<int> indexT = new List<int>();
        List<float> posArr = new List<float>();
        contain = GetComponent<RectTransform>();
        for (int i = 0; i < contain.childCount; i++)
        {
            //save all loc
            //Debug.Log(contain.transform.GetChild(i).name);
            // Debug.Log(contain.childCount);
            contain = GetComponent<RectTransform>();
            tempArr.Add(contain.transform.GetChild(i));
            posArr.Add(contain.transform.GetChild(i).localPosition.y);
            if (contain.transform.GetChild(i).name == nameapi)
            {
                indexT.Add(i);
            }

        }
        if (indexT.Count != 0)
        {
            for (int i = 0; i < indexT.Count; i++)
            {
                contain = GetComponent<RectTransform>();
                DestroyImmediate(tempArr[indexT[i]].gameObject);
                //Debug.Log(contain.childCount);

                for (int j = indexT[i]; j < contain.childCount; j++)
                {

                    contain.transform.GetChild(j).transform.localPosition = new Vector3(0f, posArr[j], 0f);

                }

            }
        }
        //contain = GetComponent<RectTransform>();


    }
}
