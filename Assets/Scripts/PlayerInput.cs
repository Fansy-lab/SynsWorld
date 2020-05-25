using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public float moveSpeed = 5f;
    public GameObject arrowPrefab;
    public float arrowForce = 20f;
    public enum lookingAt{ left,right,up,down,none}
    public lookingAt currentlyLookingAt;
    public float shootRate;
    public float dashRate;
    public float dashDistance;
    public bool learnedToShoot = false;


    [SerializeField] GameObject interactPoint;
    [SerializeField] Transform firePosition;
    [SerializeField] DashBarScript dashSlider;
    [SerializeField] GameObject dashEffect;

    float lastShot = 0f;
    float lastDashed = 0f;
    bool canShoot = true;
    bool canDash = true;
    Rigidbody2D rb;
    Animator animator;
    Vector2 movement;
    Vector2 shootingDirectionAtTheMomentOfShooting;
    Vector2 lastLookingDirection;


    private void Start()
    {
        dashSlider.SetMaxValue(Convert.ToInt32(dashRate));
        dashSlider.SetDashBar(dashRate);


        lastDashed = dashRate + 1;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentlyLookingAt = lookingAt.down;
        lastLookingDirection = new Vector2(0, -1);
    }
    // Update is called once per frame
    void Update()
    {
        CheckCanDash();
        CheckCanShoot();

        CheckKeys();
        CheckMovement();
        CheckAttack();


    }

    private void CheckKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GM.Instance.ToggleQuests();

        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GM.Instance.ToggleInventoryPanel();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {

            Dash();
        }
    }



    private void Dash()
    {
        bool canDashToLocation = CanDashToLocation(new Vector3(lastLookingDirection.x, lastLookingDirection.y, 0),dashDistance);
        if (canDashToLocation)
        {
            Instantiate(dashEffect, transform.position, Quaternion.identity);

            transform.position += new Vector3(lastLookingDirection.x, lastLookingDirection.y, 0) * dashDistance;
            lastDashed = 0;
            SoundEffectsManager.instance.PlayDashSound();
        }
    }
    private bool CanDashToLocation(Vector3 dir,float distance)
    {
        RaycastHit2D[] cllisions = Physics2D.RaycastAll(transform.position, dir, distance);


        if (cllisions.Length==1 )//always hits the player
        {
            return true;
        }
        if (cllisions.Length > 2)//hits the player,the interactPoint and something else
        {
            return false;
        }
        if (cllisions.Length == 2)//hits the player and something else, might be the interactPoint
        {
            InteractPoint interactPoint =null;
            foreach (var col in cllisions)
            {
                interactPoint= col.collider.GetComponent<InteractPoint>();
            }
            if(interactPoint == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        return false;
    }




    private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && learnedToShoot && canShoot )
        {
            Shoot();
        }
    }

    public void CheckCanDash()
    {
        if (lastDashed < dashRate)
        {
            lastDashed += Time.deltaTime;
            dashSlider.SetDashBar(lastDashed);

        }
        if (lastDashed > dashRate)
        {
            canDash = true;

        }
        else
        {
            canDash = false;
        }

    }
    public void CheckCanShoot()
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

            animator.SetTrigger("shooting");
            animator.SetFloat("ShootDir", 1);
        }
        else if (currentlyLookingAt == lookingAt.left)
        {
            firePosition.localPosition = new Vector2(-0.77f, -0.1f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, 90f);

            animator.SetTrigger("shooting");
            animator.SetFloat("ShootDir", 2);

        }
        else if (currentlyLookingAt == lookingAt.right)
        {
            firePosition.localPosition = new Vector2(0.77f, -0.1f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, -90f);


            animator.SetTrigger("shooting");
            animator.SetFloat("ShootDir", 3);

        }
        else if (currentlyLookingAt == lookingAt.down)
        {
            firePosition.localPosition = new Vector2(0, -1f);
            firePosition.localRotation = Quaternion.Euler(0f, 0f, 180f);

            animator.SetTrigger("shooting");
            animator.SetFloat("ShootDir", 0);
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
        SoundEffectsManager.instance.PlayReleaseArrowSound();
    }

    private int SetDamageAmmount()
    {
        int attack = GetComponent<PlayerStats>().DPS;
        InventoryItem weapon = InventoryManager.instance.CheckIfSlotIsTakenAndReturnItemIfOcupied(InventoryItem.Slot.weapon);

        int minDamage = 0;
        int maxDamage = 0;
        if (weapon)
        {
           minDamage =  weapon.equipableWeaponryStats.AttackMinDamage;
           maxDamage = weapon.equipableWeaponryStats.AttackMaxDamage;

        }
        return UnityEngine.Random.Range(1+ minDamage, 1+ maxDamage);
    }

    private void CheckMovement()
    {
        Vector3 Movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

        animator.SetFloat("Horizontal", Movement.x);
        animator.SetFloat("Vertical", Movement.y);
        animator.SetFloat("Magnitute", Movement.magnitude);

        transform.position = transform.position + Movement.normalized * Time.deltaTime * moveSpeed;
        SetLookingPosition(Movement);
    }
    private void SetLookingPosition(Vector2 movement)
    {

        if (movement.x > 0.01)
        {
            interactPoint.transform.localPosition = new Vector2(0.77f, -0.37f);


            animator.SetFloat("LookDir", 3f);
            currentlyLookingAt = lookingAt.right;
            lastLookingDirection = Vector2.right;

        }
        if (movement.x < 0)
        {
            interactPoint.transform.localPosition = new Vector2(-0.77f, -0.37f);

            animator.SetFloat("LookDir", 2f);
            currentlyLookingAt = lookingAt.left;
            lastLookingDirection =  Vector2.left;

        }
        if (movement.y > 0.01)
        {
            interactPoint.transform.localPosition = new Vector2(0, 0.7f);

            animator.SetFloat("LookDir", 1f);
            currentlyLookingAt = lookingAt.up;
            lastLookingDirection = Vector2.up;

        }
        if (movement.y<0)
        {
            interactPoint.transform.localPosition = new Vector2(0, -1f);


            animator.SetFloat("LookDir", 0f);
            currentlyLookingAt = lookingAt.down;
            lastLookingDirection = Vector2.down;

        }

    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position+movement.normalized * moveSpeed *Time.fixedDeltaTime);
    }
}
