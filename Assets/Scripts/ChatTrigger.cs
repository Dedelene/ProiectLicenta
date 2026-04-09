using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public GameObject chatUI;
    public new CameraMovement camera;
    public GameObject crosshair;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if(camera != null)
            {
                camera.enabled = false;
            }
            if(crosshair != null)
            {
                crosshair.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (camera != null)
        {
            camera.enabled = true;
        }
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }
}
