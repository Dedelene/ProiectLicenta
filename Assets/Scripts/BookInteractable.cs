using System.Linq;
using UnityEngine;

public class BookInteractable : MonoBehaviour
{
    public GameObject outlineObject;
    public GameObject bookUI;

    [Header("Book Content")]
    public BookData data;

    void Awake()
    {
        bookUI.SetActive(true);
        if (outlineObject) outlineObject.SetActive(false);
    }

    public void SetHighlighted(bool on)
    {
        if (outlineObject) outlineObject.SetActive(on);
    }

    public void OnPick()
    {
    
        if (data == null)
        {
            Debug.LogWarning($"BookInteractable '{name}' has no BookData assigned.");
            return;
        }

        if (data.pages == null || data.pages.Length == 0)
        {
            Debug.LogWarning($"Book '{name}' has no pages assigned.");
            return;
        }

        if (BookUI.Instance == null)
        {
            Debug.LogWarning("BookUI.Instance is null. Make sure a GameObject with BookUI script is present in the scene.");
            return;
        }

        BookUI.Instance.OpenBook(data.pages);
    }

}

