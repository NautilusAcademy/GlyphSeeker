using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piattaforma : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField]
    private Transform secondPosition;
    Vector3 posToMove;

    [SerializeField]
    private float speed;

    private void Start()
    {
        startPosition = transform.position;
        posToMove = startPosition;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, posToMove, Time.deltaTime * speed);   
    }

    public void ChangePosition()
    {
        if (startPosition == posToMove)
            posToMove = secondPosition.position;
        else
            posToMove = startPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IPlayer>() != null)
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPlayer>() != null)
        {
            other.transform.parent = null;
        }
    }
}
