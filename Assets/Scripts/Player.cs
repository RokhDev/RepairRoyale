using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode moveForward;
    public KeyCode moveBackwards;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode shoot;
    public float movementSpeed;
    public float rotationSpeed;
    public ParticleSystem playerWeaponParticles;
    public float playerWeaponCooldown;
    public float weaponRange;
    public float weaponAngle;
    public float weaponForce;
    public float playerKnockdownDuration;
    public GameObject playerGraphic;
    public GameObject playerKnockdownGraphic;
    public GameObject[] otherPlayers;
    public GameController gameController;
    public GameObject myPlatform;
    public GameObject myShip;
    public GameObject itemFx;

    private bool isKnockedDown = false;
    private float playerKnockdownCd = 0;
    private float playerWeaponCd = 0;
    private float yPos = 0;
    private bool gameOver = false;
    private Rigidbody rb;
    private Vector3 lastMovedDirection;
    bool moveDirectionChanged = false;
    private int ores;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        yPos = transform.position.y;
    }
    
    private void Update()
    {
        if (gameOver) { return; }

        if (isKnockedDown)
        {
            PlayerKnockdown();
            return;
        }

        PlayerShooting();
    }

    private void FixedUpdate()
    {
        if (gameOver) { return; }

        if (isKnockedDown) { return; }

        PlayerMovement();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Ore" && !isKnockedDown)
        {
            ores++;
            Magic.Pooling.PoolManager.Spawn(itemFx, c.transform.position, Quaternion.identity, "itemFx");
            Magic.Pooling.PoolManager.DeSpawn(c.gameObject, "ores");
        }
    }

    private void PlayerKnockdown()
    {
        if (playerKnockdownCd > 0)
        {
            playerKnockdownCd -= Time.deltaTime;
            return;
        }

        isKnockedDown = false;
        playerKnockdownGraphic.SetActive(false);
        playerGraphic.SetActive(true);
    }

    public void Knockdown()
    {
        isKnockedDown = true;
        playerKnockdownCd = playerKnockdownDuration;
        playerKnockdownGraphic.SetActive(true);
        playerGraphic.SetActive(false);
    }

    private void PlayerShooting()
    {
        if (playerWeaponCd > 0)
        {
            playerWeaponCd -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(shoot))
        {
            playerWeaponCd = playerWeaponCooldown;
            if (playerWeaponParticles)
            {
                playerWeaponParticles.Play();
            }

            foreach (GameObject go in otherPlayers)
            {
                Vector3 tgtDirection = (go.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, tgtDirection);
                if (Vector3.Distance(transform.position, go.transform.position) < weaponRange && angle <= weaponAngle)
                {
                    Rigidbody tgtRb = go.GetComponent<Rigidbody>();
                    tgtRb.AddForce(tgtDirection * weaponForce, ForceMode.VelocityChange);
                }
            }
        }
    }

    private void PlayerMovement()
    {
        Vector3 movementVector = Vector3.zero;
        if (Input.GetKey(moveForward))
        {
            movementVector += Vector3.right;
            moveDirectionChanged = true;
        }
        if (Input.GetKey(moveBackwards))
        {
            movementVector += Vector3.left;
            moveDirectionChanged = true;
        }
        if (Input.GetKey(moveRight))
        {
            movementVector += Vector3.back;
            moveDirectionChanged = true;
        }
        if (Input.GetKey(moveLeft))
        {
            movementVector += Vector3.forward;
            moveDirectionChanged = true;
        }

        movementVector.Normalize();
        if (moveDirectionChanged)
        {
            lastMovedDirection = movementVector;
            moveDirectionChanged = false;
            Quaternion lookDir = Quaternion.LookRotation(lastMovedDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDir, rotationSpeed * Time.fixedDeltaTime);
        }
        rb.AddForce(movementVector * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}