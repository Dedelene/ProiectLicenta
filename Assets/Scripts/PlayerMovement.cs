using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    CharacterController controller;
    public float gravity = -9.8f;
    public TMP_InputField inputChat;

    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {   
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputChat && inputChat.isFocused)
        {
            return;
        }

        isGrounded = controller.isGrounded;

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (controller == null || !controller.enabled) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 1)
            move.Normalize();

        controller.Move(speed * Time.deltaTime * move);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
