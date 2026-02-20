using UnityEngine;
using UnityEngine.UI;

public class InspectableItem : MonoBehaviour
{
    public GameObject inspectCanvas;   
    public Image inspectImage;         
    public Sprite itemSprite;
    public MonoBehaviour cameraController;
    public MonoBehaviour movementController;

    public bool isInspecting = false;

    void Start()
    {
        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);
    }
    private void Update()
    {
        if (isInspecting && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)))
            ExitInspect();
    }
    public void EnterInspect()
    {
        isInspecting = true;
        if (cameraController) cameraController.enabled = false;
        if (movementController) movementController.enabled = false;

        if (inspectCanvas != null)
        {
            inspectCanvas.SetActive(true);
            inspectImage.sprite = itemSprite;
            inspectImage.preserveAspect = true;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitInspect()
    {
        isInspecting = false;
        if (cameraController) cameraController.enabled = true;
        if (movementController) movementController.enabled = true;

        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}


