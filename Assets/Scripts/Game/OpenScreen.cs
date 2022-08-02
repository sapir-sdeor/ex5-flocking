using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OpenScreen : MonoBehaviour
{
   
    [SerializeField] InputActionReference action;
    
    protected void OnEnable() {
        action.action.Enable();
        action.action.started += LoadGame;
        action.action.performed += LoadGame;
        action.action.canceled += LoadGame;

    }

    protected void OnDisable() {
        action.action.started -= LoadGame;
        action.action.performed -= LoadGame;
        action.action.canceled -= LoadGame;
    }
    

    private void LoadGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Flocking");
    }
}
