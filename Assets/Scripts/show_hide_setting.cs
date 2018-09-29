using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class show_hide_setting : MonoBehaviour {

    // Use this for initialization
    public  GameObject settingPanel;
    public GameObject DetailsPanel;
    public  int statusSetting = 0;
    public int statusDetails = 0;
    public void showHideSetting()
    {
        //Debug.Log("setting button ");
        if (statusSetting == 0 && statusDetails == 0)
        {

            settingPanel.gameObject.SetActive(true);
            statusSetting = 1;
        }
        else if (statusSetting == 0 && statusDetails == 1)
        {
            
            DetailsPanel.gameObject.SetActive(false);
            statusDetails = 0;
            settingPanel.gameObject.SetActive(true);
            statusSetting = 1;
            //Debug.Log("close details before open setting  ");
        }
        else {
            settingPanel.gameObject.SetActive(false);
            statusSetting = 0;
        }
    }
    public void showHideDetails()
    {
        //Debug.Log("details button ");
  
        if (statusSetting == 0 && statusDetails == 0)
        {
             DetailsPanel.gameObject.SetActive(true);
            statusDetails = 1;
        }
        else if (statusSetting == 1 && statusDetails == 0)
        {
           
            settingPanel.gameObject.SetActive(false);
            statusSetting = 0;
            DetailsPanel.gameObject.SetActive(true);
            statusDetails = 1;
            //Debug.Log("close settings before open details  ");
        }
        else
        {
            DetailsPanel.gameObject.SetActive(false);
            statusDetails = 0;
        }
    }

}
