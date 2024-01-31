using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public InputManager inputManager;
    public static GameManager inst;

    private void Awake()
    {
        inputManager = new InputManager();
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        inputManager.Enable();
        if (inst != null && inst != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            inst = this;
        }
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }
}