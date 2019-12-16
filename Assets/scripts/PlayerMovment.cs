using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private float speed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private bool onGround;
    [SerializeField] private bool canMove = true;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private bool facingRight = true;
    [SerializeField] private AnimationClip dieAnimation;
    Vector2 directionalInput;
    Vector3 velocity;
    bool Donejumping;
    private float lastPlay;
    private float jumpTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x * speed, playerRB.velocity.y, 0f);
        playerRB.velocity = move;
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerAnimator.SetFloat("Speed", Mathf.Abs(directionalInput.x));
        SetDirectionalInput(directionalInput);

        if (Time.time - lastPlay >= jumpTime)
        {
            lastPlay = Time.time;
            playerAnimator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown("space") && onGround == true && canMove == true)
        {
            jumpTime = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
            playerAnimator.SetBool("Jump", true);
            playerRB.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            onGround = false;
        }
        if (directionalInput.x > 0 && !facingRight || directionalInput.x < 0 && facingRight)
        {
            facingRight = !facingRight;
                      
            Vector3 theScale = transform.localScale;
            
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        if (Donejumping)
        {
            playerAnimator.SetBool("Jump", false);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (onGround == false)
            {
                onGround = true;
                playerAnimator.SetBool("Jump", false);
            }
                
        }
        if (collision.gameObject.tag == "trap"&& canMove==true)
        {
            canMove = false;
            playerAnimator.SetBool("Jump", false);
            playerAnimator.SetBool("Die", true);
            StartCoroutine(Die());
            
        }
    }

    private IEnumerator waitforAnimation()
    {
        yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
        canMove = true;
    }

    private IEnumerator Die()
    {

        yield return new WaitForSeconds(dieAnimation.length);
        playerAnimator.SetBool("Die", false);
        DestroyGameobject();
    }

    private void DestroyGameobject()
    {
        Destroy(this.gameObject);
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
}
