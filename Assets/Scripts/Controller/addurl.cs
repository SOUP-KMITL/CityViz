using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using System;

public struct inputdata
{
    public string name,url,keypath,latpath,lngpath,myclass,status;
}
public class addurl : MonoBehaviour
{

    inputdata temp = new inputdata();
    public InputField url;
    public InputField name;
    public GameObject addpanel;
    public GameObject showkeypanel;
    public GameObject configpanel;
    public GameObject visualizepanel;
    public GameObject namepanel;
    public GameObject successpanel;

    // Use this for initialization
    void Start()
    {
        //showconfig();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void addbutton()
    {
        addpanel.SetActive(true);
        temp = new inputdata();
    }
    public void clearadd()
    {
        url.text = "";
    }
    public void nextadd()
    {
        temp.url = url.text;
        clearadd();
        closeadd();
        showkey();
    }
    public void showkey()
    {
        showkeypanel.SetActive(true);
        keypredicted kp = GameObject.Find("test").GetComponent<keypredicted>();
        StartCoroutine(kp.findKey(temp.url));
        StartCoroutine(showkeywait(kp));
    }
    IEnumerator showkeywait(keypredicted kp)
    {
        yield return new WaitForSecondsRealtime(1.8f);

        GameObject panel;
        Text txt;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/KeypathDetails");
        txt = panel.GetComponent<Text>();
        txt.text = kp.key.path;
        temp.keypath = kp.key.path;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/locationpathDetails");
        txt = panel.GetComponent<Text>();
        txt.text = kp.path.lat.path + " , " + kp.path.lng.path;
        temp.latpath = kp.path.lat.path;
        temp.lngpath = kp.path.lng.path;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/exampleDetails");
        txt = panel.GetComponent<Text>();
        txt.text = kp.key.value;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/locationsDetails");
        txt = panel.GetComponent<Text>();
        txt.text = kp.path.lat.value + " , " + kp.path.lng.value;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/KeyName");
        txt = panel.GetComponent<Text>();
        txt.text = kp.key.name;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/LocationName");
        txt = panel.GetComponent<Text>();
        txt.text = kp.path.lat.name + " , " + kp.path.lng.name;
        temp.myclass = kp.myclass.ToString();

    }
    public void showselectedkey(Information key,geolocation location)
    {
        showkeypanel.SetActive(true);
        keypredicted kp = GameObject.Find("test").GetComponent<keypredicted>();
        GameObject panel;
        Text txt;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/KeypathDetails");
        txt = panel.GetComponent<Text>();
        txt.text = key.path;
        temp.keypath = key.path;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/locationpathDetails");
        txt = panel.GetComponent<Text>();
        txt.text = location.lat.path + " , " + location.lng.path;
        temp.latpath = location.lat.path;
        temp.lngpath = location.lng.path;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/exampleDetails");
        txt = panel.GetComponent<Text>();
        txt.text = key.value;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/locationsDetails");
        txt = panel.GetComponent<Text>();
        txt.text = location.lat.value + " , " + location.lng.value;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/KeyName");
        txt = panel.GetComponent<Text>();
        txt.text = key.name;
        panel = GameObject.Find("16_9_showKey/showKey_panel/EdibleText/LocationName");
        txt = panel.GetComponent<Text>();
        txt.text = location.lat.name + " , " + location.lng.name;
        temp.myclass = kp.findClass(key, location).ToString();

    }
    public void nextkey()
    {
        closekey();
        showviz();
    }
    public void selectkey()
    {
        closekey();
        showconfig();
    }
    public void showconfig()
    {

        configpanel.SetActive(true);
        keypredicted kp = GameObject.Find("test").GetComponent<keypredicted>();
        StartCoroutine(kp.findKey(temp.url));
        StartCoroutine(showconfigwait(kp));
    }
    IEnumerator showconfigwait(keypredicted kp)
    {
        listkey listkey = GameObject.Find("test").GetComponent<listkey>();
        show_Key shkey = GameObject.Find("test").GetComponent<show_Key>();
        //List<geolocation> location = new List<geolocation>();
        StartCoroutine(listkey.matching(temp.url));
        yield return new WaitForSecondsRealtime(1.2f);
        int check = 0;
        string path = "99";
        for (int i = 0; i < listkey.pair.Count+1; i++)
        {
            if(i == listkey.pair.Count)
            {
                if(i == 0)
                {
                    shkey.createNewGroup(kp.path.lat.path, i - check + 1);
                    shkey.setLatLong(kp.path.lat.path, kp.path.lat.name, kp.path.lng.name, kp.path.lat.path, kp.path.lng.path);
                    shkey.addKey(kp.path.lat.path, "none", "none");
                    Information ikey = new Information();
                    geolocation location = new geolocation();
                    location.lat = kp.path.lat;
                    location.lng = kp.path.lng;
                    Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + kp.path.lat.path + "/defaultGroup(Clone)/key/none").GetComponent<Button>();
                    button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });
                }
                else
                {

                    shkey.createNewGroup(path, i - check + 1);
                    shkey.setLatLong(path, listkey.pair[i - 1].lat.name, listkey.pair[i - 1].lng.name, listkey.pair[i - 1].lat.path, listkey.pair[i - 1].lng.path);
                    shkey.addKey(path, "none", "none");
                    Information ikey = new Information();
                    geolocation location = new geolocation();
                    location.lat = listkey.pair[i - 1].lat;
                    location.lng = listkey.pair[i - 1].lng;
                    Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + path + "/defaultGroup(Clone)/key/none").GetComponent<Button>();
                    button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });

                }
            }
            else if(path != listkey.pair[i].lat.path)
            {
                if(path != "99")
                {
                    if(path == "")
                    {
                        shkey.createNewGroup("none", i - check+1);
                       // Debug.Log(i - check);
                        shkey.setLatLong("none", "", "", "","");
                        shkey.addKey("none", "none", "none");
                        Information ikey = new Information();
                        geolocation location = new geolocation();
                        Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/none/defaultGroup(Clone)/key/none").GetComponent<Button>();
                        button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });

                    }
                    else
                    {
                        shkey.createNewGroup(path, i - check+1);
                       // Debug.Log(i - check);
                        shkey.setLatLong(path, listkey.pair[i].lat.name ,listkey.pair[i].lng.name ,listkey.pair[i].lat.path, listkey.pair[i].lng.path);
                        shkey.addKey(path, "none", "none");
                        Information ikey = new Information();
                        geolocation location = new geolocation();
                        location.lat = listkey.pair[i].lat;
                        location.lng = listkey.pair[i].lng;
                        Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + path + "/defaultGroup(Clone)/key/none").GetComponent<Button>();
                        button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });
                    }
                    path = listkey.pair[i].lat.path;
                    check = i;
                }
                else
                {
                    path = listkey.pair[i].lat.path;
                    check = i;
                }
            }
        }
        //Debug.Log(listkey.pair.Count);
        for (int i = 0; i < listkey.pair.Count; i++)
        {
            //Debug.Log(i+" "+listkey.pair[i].lat.path);
            if (listkey.pair[i].lat.path != "")
            {
                shkey.addKey(listkey.pair[i].lat.path, listkey.pair[i].key.name, listkey.pair[i].key.path);
                geolocation location = new geolocation();
                location.lat = listkey.pair[i].lat;
                location.lng = listkey.pair[i].lng;
                Information ikey = listkey.pair[i].key;
                Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/" + listkey.pair[i].lat.path + "/defaultGroup(Clone)/key/" + listkey.pair[i].key.path).GetComponent<Button>();
                button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });

            }
            else
            {
                shkey.addKey("none", listkey.pair[i].key.name, listkey.pair[i].key.path);
                geolocation location = new geolocation();
                location.lat = listkey.pair[i].lat;
                location.lng = listkey.pair[i].lng;
                Information ikey = listkey.pair[i].key;
                Button button = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2/none/defaultGroup(Clone)/key/" + listkey.pair[i].key.path).GetComponent<Button>();
                button.onClick.AddListener(() => { closeconfig(); showselectedkey(ikey, location); });

            }
        }
    }
    public void showviz()
    {
        visualizepanel.SetActive(true);
        GameObject panel = GameObject.Find("16_9_Visualization/visualizationResult/Pic");
        RawImage img = panel.GetComponent<RawImage>();
        GameObject panel1 = GameObject.Find("16_9_Visualization/visualizationResult/TypeText");
        Text txt = panel1.GetComponent<Text>();

        if (temp.myclass == "1" ||
            temp.myclass == "2" ||
            temp.myclass == "3")
        {
            txt.text = "Map and Sidebar";
            img.texture = Resources.Load<Texture>("ui_components/visualPic/sideBarAndMap");

        }
        else if (temp.myclass == "5" ||
            temp.myclass == "6" ||
            temp.myclass == "7")
        {
            txt.text = "Sidebar Only";
            img.texture = Resources.Load<Texture>("ui_components/visualPic/sidebaronly");

        }
        else if (temp.myclass == "4")
        {
            txt.text = "Map Only";
            img.texture = Resources.Load<Texture>("ui_components/visualPic/maponly");

        }
        else
        {
            txt.text = "Else case";
            //img.texture = Resources.Load<Texture>("ui_components/visualPic/maponly");

        }
    }
    public void nextviz()
    {
        closevis();
        showname();
    }
    public void showname()
    {
        namepanel.SetActive(true);
    }
    public void clearname()
    {
        name.text = "";
    }
    public void nextname()
    {
        temp.name = name.text;
        name.text = "";
        writefile wf = GameObject.Find("test").GetComponent<writefile>();
        wf.Save(temp);
        closename();
        showsuccess();
    }
    public void showsuccess()
    {
        successpanel.SetActive(true);
    }
    public void nextsuccess()
    {
        closesuccess();
    }
    public void closeadd()
    {
        addpanel.SetActive(false);
    }
    public void closekey()
    {
        showkeypanel.SetActive(false);
    }
    public void closeconfig()
    {
        Transform container2 = GameObject.Find("16_9_sub/16_9_configure/maskPanel/container2").GetComponent<Transform>();
        foreach (Transform child in container2)
        {
            Destroy(child.gameObject);
        }
        configpanel.SetActive(false);
    }
    public void closevis()
    {
        visualizepanel.SetActive(false);
    }
    public void closename()
    {
        namepanel.SetActive(false);
    }
    public void closesuccess()
    {
        successpanel.SetActive(false);
    }
}
