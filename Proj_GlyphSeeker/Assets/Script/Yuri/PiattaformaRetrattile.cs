using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaRetrattile : PiattaformaBase
{
    [SerializeField] private float velocitaMovimento = 5f;
    [SerializeField] private float pausa = 3f;
    [SerializeField] private float intervalloInversioneDirezione = 2f;
    bool stop;
    private float timerAttesa;
    private float timerInversioneDirezione;

    void Start()
    {
        timerAttesa = pausa;
        timerInversioneDirezione = intervalloInversioneDirezione;
    }

    void Update()
    {
        if (!stop)
        {
            MuoviPiattaforma();

            timerInversioneDirezione -= Time.deltaTime;
        }
        

        if (timerInversioneDirezione <= 0f)
        {
            stop = true;
            timerAttesa -= Time.deltaTime;
            if (timerAttesa <= 0f)
            {
                stop = false;
                InvertiDirezione();
                timerInversioneDirezione = intervalloInversioneDirezione;
            }
            
        }
    }

    void MuoviPiattaforma()
    {
        float movimento = velocitaMovimento * Time.deltaTime;
        transform.Translate(new Vector3(movimento, 0f, 0f));
    }

    void InvertiDirezione()
    {
        velocitaMovimento = -velocitaMovimento;
        timerAttesa = pausa;
    }
}
