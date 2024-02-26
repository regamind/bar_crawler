using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryScene : MonoBehaviour
{

    public TextMeshProUGUI victorious;


    private void Awake()
    {
        victorious.text = StateNameTracker.victoriousPlayer;
    }


    
}
