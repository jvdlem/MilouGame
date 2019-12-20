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
    [SerializeField] private bool playSound = true;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private bool facingRight = true;
    [SerializeField] private AnimationClip dieAnimation;
    [SerializeField] private AnimationClip shootAnimation;
    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip jumpAnimation;
    [SerializeField] private GameObject projectille;
    [SerializeField] private Transform firepoint;
    [SerializeField] private float yRotate;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private AudioClip walkAudio;
    [SerializeField] private AudioClip landAudio;
    [SerializeField] private AudioClip deathAudio;
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
        if (yRotate >= 360)
        {
            yRotate = 0;
        }
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
            playerAudio.clip = jumpAudio;
            playerAudio.Play();
            onGround = false;
        }
        if (directionalInput.x > 0 && playSound == true&& onGround == true|| directionalInput.x < 0 && playSound == true && onGround == true)
        {
            playSound = false;
            StartCoroutine(walkcycle());
        }
        if (directionalInput.x > 0 && !facingRight || directionalInput.x < 0 && facingRight)
        {
            facingRight = !facingRight;
            yRotate += 180;
            firepoint.transform.rotation = Quaternion.Euler(new Vector3(0, yRotate, 0));
            Vector3 theScale = transform.localScale;
            
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        if (Donejumping)
        {
            playerAnimator.SetBool("Jump", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Shoot());
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
                playerAnimator.SetTrigger("OnGround");
                playerAudio.clip = landAudio;
                playerAudio.Play();
            }
                
        }
        if (collision.gameObject.tag == "trap"&& canMove==true)
        {
            canMove = false;
            //playerAnimator.SetBool("Jump", false);
            playerAnimator.SetTrigger("DieTrigger");
            StartCoroutine(Die());
            
        }
    }
    private IEnumerator Die()
    {
        playerAudio.clip = deathAudio;
        playerAudio.Play();
        yield return new WaitForSeconds(dieAnimation.length);
        DestroyGameobject();
    }
    private IEnumerator Shoot()
    {


        playerAnimator.SetTrigger("shoot");
        yield return new WaitForSeconds(shootAnimation.length/2);
        playerAudio.clip = shootAudio;
        playerAudio.Play();
        Instantiate(projectille,firepoint.transform.position,firepoint.transform.rotation);
        yield return new WaitForSeconds(shootAnimation.length / 2);
        playerAnimator.ResetTrigger("shoot");



    }
    
    private IEnumerator walkcycle()
    {
        playerAudio.clip = walkAudio;
        playerAudio.Play();
        yield return new WaitForSeconds(walkAnimation.length/2);
        playSound = true;
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
