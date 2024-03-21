using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectibleScript : MonoBehaviour
{
    [SerializeField] SaveSO_Script save_SO;
    [Space(10)]
    [SerializeField] string id;
    [SerializeField] bool isCollected = false;

    [Header("Dedback")]
    [SerializeField] Transform collectibleModel;
    [SerializeField] float maxSinHeight = 0.2f,
                           animSpeed = 0.65f;
    Vector3 startModelPos;



    private void Awake()
    {
        startModelPos = collectibleModel.position;


        //Controlla se e' gia' stato raccolto
        CheckCollected();
    }

    void Update()
    {
            //Calcola il valore dell'onda Seno
        float sinWaveValue = Mathf.Sin(Time.realtimeSinceStartup * animSpeed) * maxSinHeight * 0.5f;

        //Muove il modello con un movimento ad onda seno
        collectibleModel.position = startModelPos + Vector3.up * sinWaveValue;
    }


    private void OnTriggerEnter(Collider other)
    {
        //IPlayer playerCheck = other.GetComponent<IPlayer>();

        if (other.CompareTag("Player"))//playerCheck != null)
        {
            isCollected = true;

            //Lo aggiunge alla lista
            save_SO.ChangeUnlockedCollectibleValue(id, isCollected);

            //Toglie il collezionabile dalla scena
            CheckCollected();
        }
    }

    void CheckCollected()
    {
        //(Dis)Attiva il collezionabile se e' stato raccolto o no
        gameObject.SetActive(!isCollected);
    }


    public void LoadCollectible(bool newIsCollected)
    {
        //Cambia il collez. raccolto
        //e lo attiva o disattiva di conseguenza
        isCollected = newIsCollected;
            
        CheckCollected();
    }

    public string GetID() => id;



    #region EXTRA - Cambiare l'inspector

    private void OnValidate()
    {
        //Limita i valori per essere sempre positivi
        maxSinHeight = Mathf.Clamp(maxSinHeight, 0, maxSinHeight);
        animSpeed = Mathf.Clamp(animSpeed, 0, animSpeed);
    }

    #endregion


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying
                            ? startModelPos
                            : transform.position,
                dim = new Vector3(0.5f, 0, 0.5f);

        Gizmos.color = Color.gray * 0.65f;
        Gizmos.DrawCube(center + Vector3.up * maxSinHeight * 0.5f,
                        dim);
        Gizmos.DrawCube(center - Vector3.up * maxSinHeight * 0.5f,
                        dim);
    }

    #endregion


    #region EXTRA - Reset

    private void Reset()
    {
        GenerateID();
    }

    void GenerateID()
    {
        //Genera un nuovo id per il collezionabile
        id = System.Guid.NewGuid() + "";

        /* 
         * Questo è il video in cui spiega il GUID
         * (da 18:10 a 18:52)
         * https://youtu.be/aUi9aijvpgs?t=1090s
         */
    }

    #endregion
}
