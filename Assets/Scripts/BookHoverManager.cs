using UnityEngine;

public class BookHoverManager : MonoBehaviour
{
    public Camera cam; // lasă gol pentru a folosi Camera.main în Awake
    public LayerMask interactableLayer; // selectează layer-ul Interactable
    public float maxDistance = 50f;

    private BookInteractable currentHover;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (BookUI.IsOpen)
        {
            if (currentHover != null)
            {
                currentHover.SetHighlighted(false);
                currentHover = null;
            }
            return;
        }
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, maxDistance, interactableLayer);

        if (hitSomething)
        {
            BookInteractable bi = hit.collider.GetComponentInParent<BookInteractable>();
            if (bi != currentHover)
            {
                if (currentHover != null) currentHover.SetHighlighted(false);
                currentHover = bi;
                if (currentHover != null) currentHover.SetHighlighted(true);
            }

            if (Input.GetMouseButtonDown(0) && currentHover != null)
            {
                currentHover.OnPick();
            }
        }
        else
        {
            if (currentHover != null)
            {
                currentHover.SetHighlighted(false);
                currentHover = null;
            }
        }
    }
}
