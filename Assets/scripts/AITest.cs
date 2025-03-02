using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    [SerializeField] private string gasURL;
    [SerializeField] private string prompt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SendDataToGAS());
        }
    }

    private IEnumerator SendDataToGAS()
    {
        Debug.Log("Sending request to: " + gasURL);

        WWWForm form = new WWWForm();
        form.AddField("parameter", prompt);
        UnityWebRequest www = UnityWebRequest.Post(gasURL, form);

        yield return www.SendWebRequest();

        string response = "";

        if (www.result == UnityWebRequest.Result.Success)
        {
            response = www.downloadHandler.text;
            Debug.Log("Response: " + www.downloadHandler.text);
        }
        else
        {
            response = "There was an error!";
            Debug.LogError("Request failed! Error: " + www.error);
        }

        Debug.Log(response);
    }
}
