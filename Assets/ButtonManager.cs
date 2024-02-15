using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void NextScene()
    {
        Debug.Log("nextScene called");
        SceneManager.LoadScene(0);
        
    }
}
