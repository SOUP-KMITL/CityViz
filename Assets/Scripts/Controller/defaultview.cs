using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaultview : MonoBehaviour {
    
    public void setdefaultview()
    {
        WrldMap wrld = GameObject.Find("WrldMap").GetComponent<WrldMap>();
        wrld.OnDestroy();
        wrld.SetupApi();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
