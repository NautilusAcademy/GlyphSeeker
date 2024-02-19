using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float height = 2f;
    [SerializeField] private float speed = 5f;

    private bool playerInTrigger = false;

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
        Vector3 nuovaPosizione = new Vector3(door.transform.position.x, height, door.transform.position.z);
        door.transform.position = Vector3.Lerp(door.transform.position, nuovaPosizione, speed * Time.deltaTime);
    }

    void Abbassa()
    {
        Vector3 nuovaPosizione = new Vector3(door.transform.position.x, 0f, door.transform.position.z);
        door.transform.position = Vector3.Lerp(door.transform.position, nuovaPosizione, speed * Time.deltaTime);
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

