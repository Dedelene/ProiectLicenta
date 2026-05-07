using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform playerBody;
    public float xRotation = 0f;

    [Header("Head Bobbing")]
    public float bobSpeed = 12f;
    public float bobAmount = 0.05f;

    private float defaultPosY = 0f;
    private float timer = 0f;

    int frameToSkip = 5;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = transform.localEulerAngles.x;
        if (xRotation > 180) xRotation -= 360;

        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (frameToSkip > 0)
        {
            frameToSkip--;
            return;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        HandleHeadBob();
    }

    void HandleHeadBob()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                defaultPosY + Mathf.Sin(timer) * bobAmount,
                transform.localPosition.z
            );
        }
        else
        {
            timer = 0f;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * bobSpeed),
                transform.localPosition.z
            );
        }
    }
}
