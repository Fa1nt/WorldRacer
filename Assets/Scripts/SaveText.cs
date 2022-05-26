using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveText : MonoBehaviour
{
    public InputField textInput;
    public static string data;

    void Start()
    {
        if (data != null)
            textInput.text = data;
    }

    public void SaveData(string newData)
    {
        data = newData;
    }
}
