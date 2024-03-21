using UnityEngine;

public class PiattaformaSemovente : PiattaformaBase
{
    [SerializeField] private float velocitaMovimento = 5f;
    [SerializeField] private float intervalloInversioneDirezione = 2f;

    private float timerInversioneDirezione;

    void Start()
    {
        timerInversioneDirezione = intervalloInversioneDirezione;
    }

    void Update()
    {
        MuoviPiattaforma();

        timerInversioneDirezione -= Time.deltaTime;

        if (timerInversioneDirezione <= 0f)
        {
            InvertiDirezione();
            timerInversioneDirezione = intervalloInversioneDirezione;
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
    }
}
