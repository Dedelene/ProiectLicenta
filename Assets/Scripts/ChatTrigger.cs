using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatTrigger : MonoBehaviour
{
    public GameObject chatPlayer;
    public GameObject chatAI;
    public new CameraMovement camera;
    public GameObject crosshair;
    private GeminiChatManager chatManager;
    void Start()
    {
        chatManager = FindAnyObjectByType<GeminiChatManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatPlayer.SetActive(true);
            chatAI.SetActive(true);
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
            GeminiChatManager manager = FindAnyObjectByType<GeminiChatManager>();

            if (chatManager != null)
            {
                chatManager.StartFirstConversation();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chatPlayer.SetActive(false);
            chatAI.SetActive(false);
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
