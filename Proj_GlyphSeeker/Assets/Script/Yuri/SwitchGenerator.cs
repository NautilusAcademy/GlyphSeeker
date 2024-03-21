using System.Collections;
using UnityEngine;

public class SwitchGenerator : MonoBehaviour, IChargeable
{
    [System.Serializable]
    public class OggettoDaSpostare
    {
        public GameObject piattaforma;
        public Transform puntoA;
        public Transform puntoB;
    }

    [SerializeField]
    private float durataMovimento = 3f;
    [SerializeField]
    private OggettoDaSpostare[] daSpostare;

    private bool suPuntoIniziale = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        foreach (var ogg in daSpostare)
        {
            Gizmos.DrawLine(ogg.puntoA.position, ogg.puntoB.position);
        }
    }

    public void Charge()
    {
        StartCoroutine(MoveObjectsCoroutine());
    }

    private IEnumerator MoveObjectsCoroutine()
    {
        foreach (var ogg in daSpostare)
        {
            Vector3 posIniziale = ogg.piattaforma.transform.position;
            Vector3 target = suPuntoIniziale ? ogg.puntoB.position : ogg.puntoA.position;
            float startTime = Time.time;

            while (Time.time - startTime < durataMovimento)
            {
                float t = (Time.time - startTime) / durataMovimento;
                ogg.piattaforma.transform.position = Vector3.Lerp(posIniziale, target, t);
                yield return null;
            }

            ogg.piattaforma.transform.position = target;
        }

        suPuntoIniziale = !suPuntoIniziale;
    }
}
