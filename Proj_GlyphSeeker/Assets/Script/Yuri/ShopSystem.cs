using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] GameObject shopMenuUI, playerHUD, shopButton, advice;

    [SerializeField] List<MonoBehaviour> scriptToBlock;



    void Start()
    {
        gameIsPaused = false;
        Resume();
    }

    void Update()
    {

        if (GameManager.inst.inputManager.Player.Pause.WasPressedThisFrame())
        {
            if (gameIsPaused)
            {
                Resume();
            }
        }
    }


    public void Resume()
    {
        playerHUD.SetActive(true);
        shopMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        EnableAllScripts(true);
    }

    void OpenShop()
    {
        shopMenuUI.SetActive(true);
        playerHUD.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameIsPaused = true;
        SelectFromShop();
        EnableAllScripts(false);
    }

    public void EnableAllScripts(bool enabled)
    {
        foreach (MonoBehaviour scr in scriptToBlock)
        {
            scr.enabled = enabled;
        }
    }

    public void SelectFromShop()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(shopButton);

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            advice.gameObject.SetActive(true);
            if(GameManager.inst.inputManager.Player.Interaction.WasPressedThisFrame())
            {
                OpenShop();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            advice.gameObject.SetActive(false);
        }
    }

    public void PowerUp1()
    {
        Debug.LogError("Hai eseguito un potenziamento");
    }
}