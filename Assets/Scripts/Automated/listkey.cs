using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct match
{
    public Information key, lat, lng;
}
public class listkey : MonoBehaviour
{

    List<Information> key = new List<Information>();
    public List<match> pair = new List<match>();
    match temp = new match();
    // Use this for initialization
    void Start () {
        //StartCoroutine(matching(url6));
        
	}

    public IEnumerator matching(string url)
    {
        key = new List<Information>();
        pair = new List<match>();
        //Debug.Log("findlatlng");
        preparat preparat = GameObject.Find("test").GetComponent<preparat>();
        preparat.Starts(url);
        keypredicted keypredicted = GameObject.Find("test").GetComponent<keypredicted>();
        yield return new WaitForSecondsRealtime(0.7f);
        Information keypath = new Information();
        bool same;
        for (int i = 0; i < preparat.data.Count; i++)
        {
            if (preparat.data[i].myType != "geolocation-lat" && preparat.data[i].myType != "geolocation-long")
            {
                same = false;
                keypath = preparat.data[i];
                for (int j = 0; j < key.Count; j++)
                {
                    if (key[j].path == preparat.data[i].path)
                    {
                        same = true;
                        break;
                        //key.Add(preparat.data[i]);
                    }
                }
                //Debug.Log(preparat.data[i].path);
                if (!same)
                {
                    key.Add(keypath);
                }
            }
        }
        for (int i = 0; i < key.Count; i++)
        {
            //Debug.Log(key[i].path);
            geolocation location = keypredicted.checklatlng(preparat.data, key[i].path);
            temp.lat = location.lat;
            temp.lng = location.lng;
            temp.key = key[i];
            pair.Add(temp);
        }
        pair = pair.OrderBy(sel => sel.lat.path).ToList();
        
    }
    // Update is called once per frame
	void Update () {
		
	}
}
