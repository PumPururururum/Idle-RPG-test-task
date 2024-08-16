using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    private Animator animator;
    private PlayerManager player;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.flyArrow )
        {
            spriteRenderer.enabled = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-1.5f, transform.position.y, 0), 8 * Time.deltaTime);
            if(transform.position != new Vector3(-1.5f, transform.position.y, 0))
            {

                animator.SetBool("fly",true);
            }
        }
        if (!player.flyArrow )
        {
            spriteRenderer.enabled = false;
            transform.position = new Vector3(-5.73f, transform.position.y, 0);
        }
    }

    public void EndAnimation()
    {
        animator.SetBool("fly", false);
    }
}
