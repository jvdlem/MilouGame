using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectille : MonoBehaviour
{
    [SerializeField] private Rigidbody2D projectilleRB;
    [SerializeField] private Transform projectille;
    [SerializeField] private int speed;
    [SerializeField] private Animator bulletAnimator;
    [SerializeField] private AnimationClip explodeAnimation;
    [SerializeField] private AudioSource bulletAudio;
    [SerializeField] private AudioClip explodeAudio;
    // Start is called before the first frame update
    void Start()
    {
        projectilleRB.AddForce(transform.right * speed, ForceMode2D.Force);
        StartCoroutine(DeathTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground"|| collision.gameObject.tag == "trap")
        {
            StartCoroutine(Die());

        }
        
    }

    private IEnumerator Die()
    {
        bulletAudio.clip = explodeAudio;
        bulletAudio.Play();
        bulletAnimator.SetTrigger("Explode");
        yield return new WaitForSeconds(explodeAnimation.length);
        Destroy(this.gameObject);
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10);
        StartCoroutine(Die());
    }


}
