using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainGamePrefab;
    public GameObject mainGameObject;
    void Awake()
    {
        Time.timeScale = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked() {
        Text startButtonText = GameObject.Find("StartText").GetComponent<Text>();
        if (startButtonText.text != "Try Again!") {
            startButtonText.text = "Try Again!";
        }
        foreach (Transform eachChild in transform) {
            if (eachChild.name != "Score") {
                Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(false);
            }
        }
        Time.timeScale = 1.0f;
        mainGameObject = (GameObject) Instantiate(mainGamePrefab);
        mainGameObject.SetActive(true);
        Camera camera = Camera.main;
        camera.clearFlags = CameraClearFlags.Nothing;
        GameObject cameraObject = GameObject.Find("Main Camera");
        cameraObject.GetComponent<camera_Controller>().enabled = true;
    }
}
