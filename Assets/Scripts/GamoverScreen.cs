using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamoverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public Text pointsText;
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
