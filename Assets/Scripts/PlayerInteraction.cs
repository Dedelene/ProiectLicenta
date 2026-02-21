using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 2.5f;
    public LayerMask interactionLayers;

    private BookInteractable currentBook;

    void Update()
    {
        Ray ray = new (cam.transform.position, cam.transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactionLayers);

        if (!BookUI.IsOpen && hitSomething)
        {
            if (hit.collider.TryGetComponent<BookInteractable>(out var book))
            {
                if (currentBook != book)
                {
                    if (currentBook != null) currentBook.SetHighlighted(false);
                    currentBook = book;
                    currentBook.SetHighlighted(true);
                }
            }
            else
            {
                ResetBookHighlight();
            }
        }
        else
        {
            ResetBookHighlight();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (BookUI.IsOpen) return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (hitSomething)
            {
                ExecuteInteraction(hit);
            }
        }
    }

    void ResetBookHighlight()
    {
        if (currentBook != null)
        {
            currentBook.SetHighlighted(false);
            currentBook = null;
        }
    }

    void ExecuteInteraction(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<DrawerController>(out var drawer)) drawer.ToggleDrawer();
        else if (hit.collider.TryGetComponent<KeypadInteractable>(out var keypad)) keypad.EnterKeypad();
        else if (hit.collider.TryGetComponent<InspectableItem>(out var inspectable)) inspectable.EnterInspect();
        else if (hit.collider.TryGetComponent<DoorClickController>(out var door)) door.ToggleDoor();
        else if (hit.collider.TryGetComponent<CupboardDoor>(out var cupboard)) cupboard.ToggleDoor();
        else if (hit.collider.TryGetComponent<LargeInspect>(out var large)) large.EnterInspect();
        else if (hit.collider.TryGetComponent<BookInteractable>(out var book)) book.OnPick();
        else if (hit.collider.TryGetComponent<ChestInteraction>(out var chest)) chest.EnterLockPad();
    }
}