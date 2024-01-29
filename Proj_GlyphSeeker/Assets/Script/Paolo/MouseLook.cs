using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // vogliamo controllare la velocità del nostro mouse

    public Transform playerBody; // ci serve un riferimento per far ruotare il nostro personaggio

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // non vogliamo vedere il cursore quando ruotiamo

    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY; // se  incrementato, il movimento risulterebbe al contrario
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // vogliamo evitare di ruotare eccessivamente fino a dietro le nostrespalle
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }
}
