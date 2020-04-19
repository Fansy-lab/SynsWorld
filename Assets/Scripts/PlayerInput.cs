using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Transform firePosition;
    public GameObject arrowPrefab;
    public float arrowForce = 20f;
    public GameObject interactPoint;

    public enum lookingAt{ left,right,up,down,none}
    public lookingAt currentlyLookingAt;
    public Vector2 lastLookingDirection;
    public Animator animator;


    Vector2 movement;
    Vector2 shootingDirectionAtTheMomentOfShooting;

    public bool learnedToShoot = false;
    
    public float shootRate = 1f;
    private float lastShot = 0f;
    private bool canShoot = true;
    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        CheckAttack();
    }
    private void Start()
    {
        currentlyLookingAt = lookingAt.down;
        lastLookingDirection= new Vector2(0, -1); 
    }

    private void CheckAttack()
    {
        checkCanShoot();
        if (Input.GetKeyDown(KeyCode.Space) && learnedToShoot && canShoot )
        {
            Shoot();
        }
    }

    public void checkCanShoot()
    {
        if(lastShot<shootRate)
            lastShot += Time.deltaTime;
        if (lastShot > shootRate)
        {
            canShoot = true;
          
        }
        else
        {
            canShoot = false;
        } 
       
    }
    private void Shoot()
    {
        if (currentlyLookingAt == lookingAt.up)
        {
            firePosition.localPosition = new Vector2(0, 0.7f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, 0f);
            
            animator.SetTrigger("attackUp");
        }
        else if (currentlyLookingAt == lookingAt.left)
        {
            firePosition.localPosition = new Vector2(-0.77f, -0.37f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, 90f);
            animator.SetTrigger("attackLeft");

        }
        else if (currentlyLookingAt == lookingAt.right)
        {
            firePosition.localPosition = new Vector2(0.77f, -0.37f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, -90f);

            animator.SetTrigger("attackRight");

        }
        else if (currentlyLookingAt == lookingAt.down)
        {
            firePosition.localPosition = new Vector2(0, -1f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, 180f);

            animator.SetTrigger("attackDown");
        }
        SetArrowDirection();
        Invoke("SpawnArrow",0.3f);
        lastShot = 0;

    }

    private void SetArrowDirection()
    {
        if(currentlyLookingAt == lookingAt.down)
        {
            shootingDirectionAtTheMomentOfShooting = new Vector2(0, -1);
        }
        if (currentlyLookingAt == lookingAt.up)
        {
            shootingDirectionAtTheMomentOfShooting = new Vector2(0, 1);
        }
        if (currentlyLookingAt == lookingAt.left)
        {
            shootingDirectionAtTheMomentOfShooting = new Vector2(-1, 0);
        }
        if (currentlyLookingAt == lookingAt.right)
        {
            shootingDirectionAtTheMomentOfShooting = new Vector2(1, 0);
        }
    }

    private void SpawnArrow()
    {
       GameObject arrow = Instantiate(arrowPrefab, firePosition.position, firePosition.rotation) as GameObject;
        arrow.GetComponent<Projectile>().damageAmmount = SetDamageAmmount();
        Rigidbody2D rbArrow= arrow.GetComponent<Rigidbody2D>();
       
        if(shootingDirectionAtTheMomentOfShooting == new Vector2(0, 0))
        {
            shootingDirectionAtTheMomentOfShooting = lastLookingDirection;
        }
        rbArrow.AddForce(shootingDirectionAtTheMomentOfShooting * arrowForce, ForceMode2D.Impulse);
    }

    private int SetDamageAmmount()
    {
        return UnityEngine.Random.Range(1,3);
    }

    private void CheckMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);

        SetLookingPosition(movement);
    }
    private void SetLookingPosition(Vector2 movement)
    {

        if (movement.x > 0.01)
        {
            interactPoint.transform.localPosition = new Vector2(0.77f, -0.37f);


            animator.SetInteger("lookingAt", 3);
            currentlyLookingAt = lookingAt.right;
            lastLookingDirection = new Vector2(1, 0);
        }
        if (movement.x < 0)
        {
            interactPoint.transform.localPosition = new Vector2(-0.77f, -0.37f);

            animator.SetInteger("lookingAt", 1);
            currentlyLookingAt = lookingAt.left;
            lastLookingDirection = new Vector2(-1, 0);

        }
        if (movement.y > 0.01)
        {
            interactPoint.transform.localPosition = new Vector2(0, 0.7f);

            animator.SetInteger("lookingAt", 2);
            currentlyLookingAt = lookingAt.up;
            lastLookingDirection = new Vector2(0, 1);

        }
        if (movement.y<0)
        {
            interactPoint.transform.localPosition = new Vector2(0, -1f);


            animator.SetInteger("lookingAt", 0);
            currentlyLookingAt = lookingAt.down;
            lastLookingDirection = new Vector2(0, -1);

        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+movement.normalized * moveSpeed *Time.fixedDeltaTime);
    }
}
