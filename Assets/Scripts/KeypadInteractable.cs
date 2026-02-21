using UnityEngine;

public class KeypadInteractable : MonoBehaviour
{
    public Camera mainCam;
    public Camera keypadCam;
    public GameObject crosshair;
    public DoorController door;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buzzClip;

    private bool isUsingKeypad = false;
    private string inputCode = "";
    private readonly string correctCode = "11213";
    readonly int maxLength = 5;

    void Update()
    {
        if (!isUsingKeypad) return;

        if (Input.GetMouseButtonDown(1))
        {
            ExitKeypad();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = keypadCam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit, 100f)){
                if(hit.collider.TryGetComponent<KeypadButton>(out var btn))
                {
                    btn.Press();
                }
            }
        }
    }

    public void EnterKeypad()
    {
        isUsingKeypad = true;
        inputCode = "";

        mainCam.gameObject.SetActive(false);
        keypadCam.gameObject.SetActive(true);
        if (crosshair != null) crosshair.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }

    public void ExitKeypad()
    {
        isUsingKeypad = false;

        keypadCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        if (crosshair != null) crosshair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnKeyPress(string key)
    {
        if(key == "Escape")
        {
            inputCode = "";
            ExitKeypad();
            return;
        }

        if (key == "Enter")
        {
            CheckCode();
            return;
        }

        if(inputCode.Length < maxLength)
        {
            inputCode += key;
        }

    }

    void CheckCode()
    {

        if (inputCode == correctCode)
        {
            if (door != null)
            {
                door.OpenDoor();
            }
            ExitKeypad();
        }
        else
        {
            inputCode = "";
            if (audioSource != null && buzzClip != null)
            {
                audioSource.PlayOneShot(buzzClip);
            }
        }
        return;
    }

}

