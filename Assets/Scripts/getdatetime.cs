using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class getdatetime : MonoBehaviour {
    string stime;
    string date;
    GameObject timeText;
    GameObject dateText;
    Text textTemp;
    Text textTemp1;
    // Use this for initialization
    void Start () {
        date = " ";
        stime = " ";
        timeText =  GameObject.Find("/16_9_main/sideBar/dateTime/timeText");
        dateText = GameObject.Find("/16_9_main/sideBar/dateTime/dateText");
        
    }

    // Update is called once per frame
    void Update () {
        stime = System.DateTime.Now.ToShortTimeString();
        date = System.DateTime.Now.ToShortDateString();
        textTemp = timeText.GetComponent<Text>();
        textTemp1 = dateText.GetComponent<Text>();
        textTemp.text = stime;
        textTemp1.text = date;
        //Debug.Log(stime);
        //Debug.Log(date);
    }
}
