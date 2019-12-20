using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{

    Player player;
    private bool facingRight;
    //public Animator animator;

    void Start()
    {
        player = GetComponent<Player>();
        facingRight = true;
    }
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);
        //animator.SetFloat("Speed", Mathf.Abs(directionalInput.x));

        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.OnJumpInputDown();

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                player.OnJumpInputUp();
            }
            if (directionalInput.x > 0 && !facingRight || directionalInput.x < 0 && facingRight)
            {
                facingRight = !facingRight;

                Vector3 theScale = transform.localScale;

                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
    }
}