using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Chase : MonoBehaviour

{

    public GameObject Player;
    public float speed;

    private Animator anim;
    private float distance;
    private Rigidbody2D rb;
    private bool isChasing;

    private Enemy_Patrol patrol;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        patrol = GetComponent<Enemy_Patrol>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Follow()
    {
        distance = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 direction = (Player.transform.position - transform.position);

        if (distance > 1f) {

            isChasing = true;
            
            rb.MovePosition(
            Vector2.MoveTowards(
            rb.position,
            Player.transform.position,
            speed * Time.fixedDeltaTime
        )
            );
            anim.Play("Walk");

        }


        Vector3 scale = transform.localScale;

        if (Player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        else if (Player.transform.position.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
  
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        Follow();



    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
