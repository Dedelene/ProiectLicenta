using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public string key;
    public Transform numberMesh;
    public KeypadInteractable keypadController;

    private Vector3 originalPosButton;
    private Vector3 originalPosNumber;

    bool isPressed = false;

    void Start()
    {
        originalPosButton = transform.localPosition;
        if (numberMesh != null)
            originalPosNumber = numberMesh.localPosition;

        if (keypadController == null)
            keypadController = GetComponentInParent<KeypadInteractable>();
    }

    public void Press()
    {
        if (isPressed) return;

        isPressed = true;

        Vector3 offset = -transform.forward * 0.04f;

        transform.localPosition = originalPosButton + offset;
        if (numberMesh != null)
            numberMesh.localPosition = originalPosNumber + offset;

        if (keypadController != null)
            keypadController.OnKeyPress(key);

        Invoke(nameof(Release), 0.35f);

    }

    void Release()
    {
        transform.localPosition = originalPosButton;
        if (numberMesh != null)
            numberMesh.localPosition = originalPosNumber;

        isPressed = false;
    }
}

