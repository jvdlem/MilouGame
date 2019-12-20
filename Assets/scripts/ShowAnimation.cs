using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Start()
    {
        animator.SetBool("Playanimation", true);
    }
}
