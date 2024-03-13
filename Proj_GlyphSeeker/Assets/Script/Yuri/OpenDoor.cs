using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float height = 2f;
    [SerializeField] private float speed = 5f;
    private Vector3 startPosition;
    private bool playerInTrigger = false;

    private void Start()
    {
        startPosition = door.transform.position;
    }
    void Update()
    {
        if (playerInTrigger)
        {
            Alza();
        }
        else
        {
            Abbassa();
        }
    }

    void Alza()
    {
        Vector3 newPosition = new Vector3(door.transform.position.x, height, door.transform.position.z);
        door.transform.position = Vector3.Lerp(door.transform.position, newPosition, speed * Time.deltaTime);
    }

    void Abbassa()
    {
        door.transform.position = Vector3.Lerp(door.transform.position, startPosition, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}

