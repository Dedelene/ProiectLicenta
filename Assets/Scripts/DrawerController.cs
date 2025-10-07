using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    public Vector3 openOffSet = new Vector3(0, 0, 0.5f);
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

    public Transform player;

    private System.Collections.IEnumerator MoveDrawer()
    {
        isMoving = true;

        Vector3 target = isOpen ? closedPos : openPos;
        Vector3 start = transform.localPosition;

        float t = 0f;

        float dist = Vector3.Distance(player.position, transform.position);
        bool attachPlayer = dist < 3f;

        if (attachPlayer) player.SetParent(transform);

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.localPosition = target;
        isOpen = !isOpen;
        isMoving = false;

        if (attachPlayer) player.SetParent(null);
    }
}
