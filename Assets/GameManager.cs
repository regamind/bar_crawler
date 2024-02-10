using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Below code (up until line 23, where we set the _instance reference) is based off of this tutorial (https://vintay.medium.com/creating-a-game-manager-class-in-unity-1a59224eba03)
    // A static private variable to hold the reference to this Game Manager instance
    private static GameManager _instance;

    // A public static Property to allow other classes to get ther reference, but not set
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("ERROR: No GameManager exists in scene.");
            return _instance;
        }
    }

    // Set the _instance reference at the soonest opportunity when the game starts
    private void Awake() => _instance = this;

}
