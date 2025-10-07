using UnityEngine;

public class KeypadInteractable : MonoBehaviour
{
    public Camera mainCam;
    public Camera keypadCam;
    public GameObject crosshair;
    public GameObject door;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buzzClip;

    private bool isUsingKeypad = false;
    private string inputCode = "";
    private string correctCode = "11213";

    void Update()
    {
        if (isUsingKeypad && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitKeypad();
        }
        if (!isUsingKeypad && Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            int mask = LayerMask.GetMask("Interactable");

            if (Physics.Raycast(ray, out RaycastHit hit, 3f, mask))
            {
                if (hit.collider.gameObject.name == "panel_code")
                {
                    EnterKeypad();
                }
            }
        }
        if (isUsingKeypad && Input.GetMouseButtonDown(0))
        {
            Ray ray = keypadCam.ScreenPointToRay(Input.mousePosition);
            int mask = LayerMask.GetMask("KeypadButtons");

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
            {
                KeypadButton btn = hit.collider.GetComponent<KeypadButton>();
                if (btn != null) btn.Press();
            }
        }
    }

    public void EnterKeypad()
    {
        isUsingKeypad = true;

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
        if (key == "Escape")
        {
            inputCode = "";
            ExitKeypad();
            return;
        }
        if(key == "Enter")
        {
            if(inputCode == correctCode)
            {
                inputCode = "";
                if (door != null)
                {
                    DoorController controller = door.GetComponent<DoorController>();
                    if (controller != null)
                    {
                        controller.OpenDoor();
                    }
                    ExitKeypad();
                }
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
        inputCode += key;
    }

}

