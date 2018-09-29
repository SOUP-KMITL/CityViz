using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using LitJson;

public struct Information
{
    public string path, name, value, dataType, myType;
}

public class preparat : MonoBehaviour
{
    
    string text;
    int currentIndex = 0;
    Information temp = new Information();
    public List<Information> data = new List<Information>();

    // Use this for initialization
    public void Starts(string url)
    {
        data.Clear();
        StartCoroutine(getData(url));
    }

    string findMyType(string dataType, string name, string value)
    {
        string myType = "none";
        if (dataType == "string")
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                myType = "time";
            }
            else if (name.ToLower() == "longitude" ||
               name.ToLower() == "long" ||
               name.ToLower() == "lng" ||
               name.ToLower() == "lon" ||
               name.ToLower() == "y")
            {
                myType = "geolocation-long";
            }
            else if (name.ToLower() == "latitude" ||
               name.ToLower() == "lat" ||
               name.ToLower() == "x")
            {
                myType = "geolocation-lat";
            }
            else if (value.ToLower() == "low" ||
                value.ToLower() == "lower" ||
                value.ToLower() == "med" ||
                value.ToLower() == "medium" ||
                value.ToLower() == "high" ||
                value.ToLower() == "higher" ||
                value.ToLower() == "good" ||
                value.ToLower() == "bad" ||
                value.ToLower() == "true" ||
                value.ToLower() == "false" ||
                value.ToLower() == "moderate" ||
                value.ToLower() == "normal" ||
                value.ToLower() == "safe" ||
                value.ToLower() == "fair")
            {
                myType = "nominal";
            }
            else if (value.ToLower() == "\"null\"")
            {
                myType = "null";
            }
            else
            {
                myType = "text";
            }
        }
        if (dataType == "boolean")
        {
            myType = "nominal";
        }
        if (dataType == "float" || dataType == "int")
        {
            if (name.ToLower() == "longitude" ||
                name.ToLower() == "long" ||
                name.ToLower() == "lng" ||
                name.ToLower() == "lon" ||
                name.ToLower() == "y")
            {
                myType = "geolocation-long";
            }
            else if (name.ToLower() == "latitude" ||
               name.ToLower() == "lat" ||
               name.ToLower() == "x")
            {
                myType = "geolocation-lat";
            }
            else
            {
                myType = "number";
            }
        }
        return myType;
    }
    string myStringName()
    {
        currentIndex++;
        int startIndex = currentIndex;
        for (; currentIndex < text.Length; currentIndex++)
        {
            if (text[currentIndex] == '"')
            {
                if (text[currentIndex - 1] != '\\')
                {
                    temp.name = text.Substring(startIndex, currentIndex - startIndex);
                    currentIndex++;
                    break;
                }
            }
        }
        return temp.name;
    }

    void myObject(string path)
    {
        currentIndex++;
        for (; currentIndex < text.Length; currentIndex++)
        {
            string tpath = path;
            if (text[currentIndex] == '"')
            {
                if (tpath == "")
                {
                    tpath = myStringName();
                }
                else
                {
                    tpath = tpath + "." + myStringName();
                }
            }
            if (text[currentIndex] == ':')
            {
                currentIndex++;
                myValue(tpath);
            }
            if (text[currentIndex] == '}')
            {
                currentIndex++;
                break;
            }
        }
    }

    void myString(string path)
    {
        currentIndex++;
        int startIndex = currentIndex;
        for (; currentIndex < text.Length; currentIndex++)
        {
            if (text[currentIndex] == '"')
            {
                if (text[currentIndex - 1] != '\\')
                {
                    temp.value = text.Substring(startIndex, currentIndex - startIndex);
                    temp.path = path;
                    temp.dataType = "string";
                    temp.myType = findMyType(temp.dataType, temp.name, temp.value);
                    data.Add(temp);
                    currentIndex++;
                    break;
                }
            }
        }
    }

    void myNumber(string path)
    {
        int startIndex = currentIndex;
        for (; currentIndex < text.Length; currentIndex++)
        {
            if (text[currentIndex] == '}' || text[currentIndex] == ',' || text[currentIndex] == ']')
            {
                temp.value = text.Substring(startIndex, currentIndex - startIndex);
                temp.path = path;
                if (temp.value.Contains("."))
                {
                    temp.dataType = "float";
                    string[] mypath = temp.path.Split('.');
                    if(temp.name == "coord" || temp.name == "coordinate" || temp.name == "coordinates" || temp.name == "geolocation")
                    {
                        if (mypath[mypath.Length - 1] == "0")
                        {
                            temp.myType = "geolocation-lat";
                        }
                        else if (mypath[mypath.Length - 1] == "1")
                        {
                            temp.myType = "geolocation-long";
                        }
                    }
                    else
                    {
                        temp.myType = findMyType(temp.dataType, temp.name, temp.value);
                    }

                }
                else
                {
                    temp.dataType = "int";
                    temp.myType = findMyType(temp.dataType, temp.name, temp.value);

                }
                data.Add(temp);
                break;
            }
        }
    }

    void myValue(string path)
    {
        int startIndex = currentIndex;
        if (startIndex + 6 < text.Length && text.Substring(startIndex, 6) == "\"null\"")
        {
            currentIndex = currentIndex + 6;
            temp.value = text.Substring(startIndex, currentIndex - startIndex);
            temp.path = path;
            temp.dataType = "null";
            temp.myType = findMyType(temp.dataType, temp.name, temp.value);
            data.Add(temp);
        }
        else if (startIndex + 5 < text.Length && text.Substring(startIndex, 5) == "false")
        {
            currentIndex = currentIndex + 5;
            temp.value = text.Substring(startIndex, currentIndex - startIndex);
            temp.path = path;
            temp.dataType = "boolean";
            temp.myType = findMyType(temp.dataType, temp.name, temp.value);
            data.Add(temp);
        }
        else if (startIndex + 4 < text.Length && text.Substring(startIndex, 4) == "true")
        {
            currentIndex = currentIndex + 4;
            temp.value = text.Substring(startIndex, currentIndex - startIndex);
            temp.path = path;
            temp.dataType = "boolean";
            temp.myType = findMyType(temp.dataType, temp.name, temp.value);
            data.Add(temp);
        }
        else if (startIndex + 4 < text.Length && text.Substring(startIndex, 4) == "null")
        {
            currentIndex = currentIndex + 4;
            temp.value = text.Substring(startIndex, currentIndex - startIndex);
            temp.path = path;
            temp.dataType = "null";
            temp.myType = findMyType(temp.dataType, temp.name, temp.value);
            data.Add(temp);
        }
        else if (text[currentIndex] == '"')
        {
            myString(path);
        }
        else if (text[currentIndex] == '-' ||
            text[currentIndex] == '0' ||
            text[currentIndex] == '1' ||
            text[currentIndex] == '2' ||
            text[currentIndex] == '3' ||
            text[currentIndex] == '4' ||
            text[currentIndex] == '5' ||
            text[currentIndex] == '6' ||
            text[currentIndex] == '7' ||
            text[currentIndex] == '8' ||
            text[currentIndex] == '9')
        {
            myNumber(path);
        }
        else if (text[currentIndex] == '{')
        {
            myObject(path);
        }
        else if (text[currentIndex] == '[')
        {
            myArray(path);
        }
    }

    void myArray(string path)
    {
        currentIndex++;
        //int startIndex = currentIndex;
        int index = 0;
        for (; currentIndex < text.Length; currentIndex++)
        {
            if (text[currentIndex] == ']')
            {
                //Debug.Log(text.Substring(startIndex, i - startIndex));
                currentIndex++;
                break;
            }
            else
            {
                if(temp.name == "coord" || temp.name == "coordinate" || temp.name == "coordinates" || temp.name == "geolocation")
                {
                    myValue(path + "." + index);
                    index++;
                }
                else
                {
                    myValue(path + ".i");

                }
            }
            if (text[currentIndex] == ']')
            {
                //Debug.Log(text.Substring(startIndex, i - startIndex));
                currentIndex++;
                break;
            }
        }
    }

    void findStructure(string t)
    {
        text = t;
        for (; currentIndex < text.Length; currentIndex++)
        {
            if (text[currentIndex] == '{')
            {
                myObject("");
            }
            else if (text[currentIndex] == '[')
            {
                myArray("");
            }
        }
    }

    string deleteSpace(string data)
    {
        int checkString = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (checkString == 0 && data[i] == '"')
            {
                checkString = 1;
                continue;
            }
            if (checkString == 1 && data[i] == '"')
            {
                checkString = 0;
                continue;
            }
            if (checkString == 0 && (data[i] == ' ' || data[i] == '\n' || data[i] == '\t'))
            {
                data = data.Remove(i, 1);
                i--;
            }
        }
        return data;
    }
    IEnumerator getData(string uri)
    {
        WWW www = new WWW(uri);
        yield return www;
        string json = deleteSpace(www.text);
        //Debug.Log("Deleate Space :\n" + json);
        currentIndex = 0;
        findStructure(json);
        //keypredicted.Starts();
        string str = "";
        for (int i = 0; i < data.Count; i++)
        {
            str = str + data[i].path + " : " + data[i].dataType + " : " + data[i].name + " : " + data[i].myType + " : " + data[i].value + "\n";
        }
        //Debug.Log(str);

    }

	// Update is called once per frame
	void Update () {
		
	}
}
