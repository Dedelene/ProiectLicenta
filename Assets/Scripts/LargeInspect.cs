using UnityEngine;
using UnityEngine.UI;

public class ILargeInspect : MonoBehaviour
{
    public GameObject inspectCanvas;   
    public Image inspectImage;         
    public Sprite itemSprite;
    public MonoBehaviour cameraController;

    private bool isInspecting = false;

    void Start()
    {
        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        if (!isInspecting)
            EnterInspect();
    }

    void EnterInspect()
    {
        isInspecting = true;
        if (cameraController) cameraController.enabled = false;

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

        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

