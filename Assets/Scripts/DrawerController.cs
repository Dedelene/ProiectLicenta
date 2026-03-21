using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawerController : MonoBehaviour
{
    public Vector3 openOffSet = new (0, 0, 0.5f);
    public float speed = 2f;

    private Vector3 openPos;
    private Vector3 closedPos;
    private bool isOpen = false;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = transform.localPosition;
        openPos = closedPos + openOffSet;

    }
    public void ToggleDrawer()
    {
        if (!isMoving)
            StartCoroutine(MoveDrawer());
    }

    private IEnumerator MoveDrawer()
    {
        isMoving = true;

        Vector3 target = isOpen ? closedPos : openPos;
        Vector3 start = transform.localPosition;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.localPosition = target;
        isOpen = !isOpen;
        isMoving = false;
    }
}