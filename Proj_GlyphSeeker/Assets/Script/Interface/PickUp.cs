using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool canPickUp;
    [SerializeField]
    public float safeDistance;


    #region EXTRA - Gizmo

    private void OnDrawGizmosSelected()
    {
        //Disegna una sfera grande quanto la safeDistance
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, safeDistance);
    }

    #endregion
}
