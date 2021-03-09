using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class FollowerController : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.runtimeAnimatorController = animatorController;
    }

    public void FlipSprite(bool isFlipped) {
        spriteRenderer.flipX = isFlipped;
    }

    public void SetWalking(bool isWalking) {
        animator.SetBool("IsWalking", isWalking);
    }
}
