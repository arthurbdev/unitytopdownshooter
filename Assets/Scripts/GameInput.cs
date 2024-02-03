using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
  private PlayerInputActions playerInputActions;
  public static Action OnPauseGame;

  private void OnEnable()
  {
    playerInputActions = new PlayerInputActions();
    playerInputActions.Player.Enable();
    playerInputActions.UI.Enable();
    playerInputActions.UI.Pause.performed += PauseAction;
    GameController.OnGameEnd += playerInputActions.Disable;
    OnPauseGame += TogglePlayerInput;
  }


  private void OnDisable()
  {
    playerInputActions.Player.Disable();
    playerInputActions.UI.Disable();
    playerInputActions.UI.Pause.performed -= PauseAction;
    GameController.OnGameEnd -= playerInputActions.Disable;
    OnPauseGame -= TogglePlayerInput;
  }

  private void PauseAction(InputAction.CallbackContext ctx)
  {
    OnPauseGame?.Invoke();
  }

  private void TogglePlayerInput()
  {
    if (playerInputActions.Player.enabled) playerInputActions.Player.Disable();
    else playerInputActions.Player.Enable();
  }


  public Vector2 GetMovementVectorNormalized()
  {
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

    inputVector = inputVector.normalized;

    return inputVector;
  }


  public Vector2 GetMousePosition()
  {
    return playerInputActions.Player.Mouse.ReadValue<Vector2>();
  }

  public bool IsMouseClicked()
  {
    return playerInputActions.Player.Shoot.IsPressed();
  }
}
