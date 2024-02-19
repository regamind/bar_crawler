using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Player player;

    private void Awake()
    {
        //player = GetComponent<Player>();

        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<Player>();
        var index = playerInput.playerIndex;
        player = players.FirstOrDefault(p => p.GetPlayerIndex() == index);
    }

    public void OnMove(CallbackContext context)
    {
        //player.SetInputVector(context.ReadValue<Vector2>());

        if (player != null)
            player.SetInputVector(context.ReadValue<Vector2>());
    }
}
