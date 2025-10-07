using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public string key;
    public Transform numberMesh;

    private Vector3 originalPosButton;
    private Vector3 originalPosNumber;

    void Start()
    {
        originalPosButton = transform.localPosition;
        if (numberMesh != null)
            originalPosNumber = numberMesh.localPosition;
    }

    public void Press()
    {
        Vector3 offset = -transform.forward * 0.04f;

        transform.localPosition = originalPosButton + offset;
        if (numberMesh != null)
            numberMesh.localPosition = originalPosNumber + offset;

        Invoke(nameof(Release), 0.35f);

        FindObjectOfType<KeypadInteractable>().OnKeyPress(key);
    }

    void Release()
    {
        transform.localPosition = originalPosButton;
        if (numberMesh != null)
            numberMesh.localPosition = originalPosNumber;
    }
}

