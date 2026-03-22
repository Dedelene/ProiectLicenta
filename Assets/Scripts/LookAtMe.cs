using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    private Animator animator;
    public Transform target;

    [Range(0, 1)] public float weight = 1.0f;
    public float bodyWeight = 0.2f;
    public float headWeight = 0.8f;
    public float eyesWeight = 1.0f;
    public float clampWeight = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator != null && target != null)
        {
            animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyesWeight, clampWeight);
            animator.SetLookAtPosition(target.position);
        }
    }
}
