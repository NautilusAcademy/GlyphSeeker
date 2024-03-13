using System.Collections.Generic;
using UnityEngine;

public class ControlloPulsantiRunaGialla : MonoBehaviour
{
    public GameObject[] listaPulsanti;
    [SerializeField] List<MonoBehaviour> scriptToActivate;
    void Start()
    {
        ControllaStatoOggetti();
    }

    void Update()
    {
        ControllaStatoOggetti();
    }

    void ControllaStatoOggetti()
    {
        bool tuttiAttivi = true;

        // Controlla se tutti gli oggetti nella lista sono attivi
        foreach (GameObject pulsanti in listaPulsanti)
        {
            if (pulsanti == null || !pulsanti.activeSelf)
            {
                tuttiAttivi = false;
                break; // Se almeno un oggetto non è attivo, esci dal ciclo
            }
        }

        // Attiva gli script dell'oggetto che vuoi attivare con il gruppo di pulsanti
        if (tuttiAttivi)
        {
            Debug.Log("Sono tutti attivi");
            foreach (MonoBehaviour scripts in scriptToActivate)
            {
                scripts.enabled = enabled;
            }
        }
        else
        {
            //Debug.Log("Non sono tutti attivi");
        }
    }
}
