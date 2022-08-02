using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class WinDisplay : MonoBehaviour
{
    [SerializeField] private RawImage blueImage;
    [SerializeField] private RawImage redImage;
    [SerializeField] InputActionReference action;
    void Start()
    {
        switch (Score.groupWinner)
        {
            case 0:
                redImage.gameObject.SetActive(true);
                break;
            case 1:
                blueImage.gameObject.SetActive(true);
                break;
        }
    }

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
