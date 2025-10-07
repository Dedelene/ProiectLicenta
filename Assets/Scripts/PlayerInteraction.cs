using UnityEngine;
public class PlayerInteraction : MonoBehaviour {
    public Camera cam;
    public float interactDistance = 1f;
    public LayerMask inspectableMask;
    public LayerMask interactableMask;
    void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayInteract = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(rayInteract, out RaycastHit hitInteract, interactDistance, interactableMask))
            { 
                var drawer = hitInteract.collider.GetComponent<DrawerController>();
                if (drawer != null)
                { 
                    drawer.ToggleDrawer(); 
                    return;
                } 
                var keypad = hitInteract.collider.GetComponent<KeypadInteractable>();
                if (keypad != null) 
                { keypad.EnterKeypad(); 
                   return; 
                } 
            }
            Ray rayInspect = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(rayInspect, out RaycastHit hitInspect, interactDistance, inspectableMask))
            { 
                var inspectable = hitInspect.collider.GetComponent<InspectableItem>();
                if (inspectable != null) 
                { 
                    inspectable.SendMessage("EnterInspect");
                    return;
                }
            }
            Debug.DrawRay(cam.transform.position, cam.transform.forward * interactDistance, Color.red);

        }
    }
}
