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
    public GameObject playerGunGraphic;
    public GameObject[] otherPlayers;
    public GameController gameController;
    public GameObject myPlatform;
    public GameObject myShip;
    public PlayerOreText myOreText;
    public FloatingText myText;
    public GameObject itemFx;
    public Sprite winTexture;
    public AudioSource weaponAudio;
    public AudioSource weaponReloadAudio;
    public AudioSource pickupAudio;

    private bool isKnockedDown = false;
    private float playerKnockdownCd = 0;
    private float playerWeaponCd = 0;
    private float yPos = 0;
    private bool gameOver = false;
    private Rigidbody rb;
    private Vector3 lastMovedDirection;
    bool moveDirectionChanged = false;
    private int ores;
    private bool iWon = false;
    private MeshRenderer gunRenderer;
    private Color gunColor;
    private bool gunColorReset = true;
    private AudioSource movementAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gunRenderer = playerGunGraphic.GetComponent<MeshRenderer>();
        gunColor = gunRenderer.material.color;
        yPos = transform.position.y;
        movementAudio = GetComponent<AudioSource>();
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
            myOreText.SetText(ores);
            pickupAudio.Play();
        }

        if (c.tag == "Platform" && ores > 0 && c.gameObject == myPlatform)
        {
            gameController.RepairShip(this, ores);
            myText.SetText(ores);
            ores = 0;
            Magic.Pooling.PoolManager.Spawn(itemFx, transform.position, Quaternion.identity, "itemFx");
            myOreText.SetText(ores);
            c.gameObject.GetComponent<AudioSource>().Play();
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
        if (iWon) { return; }

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
            if (playerWeaponCd <= 0)
            {
                weaponReloadAudio.Play();
            }
            return;
        }

        if (!gunColorReset)
        {
            gunRenderer.material.color = gunColor;
        }

        if (Input.GetKeyDown(shoot))
        {
            playerWeaponCd = playerWeaponCooldown;
            gunColorReset = false;
            gunRenderer.material.color = new Color(1, 0.8f, 0.0f);
            if (playerWeaponParticles)
            {
                playerWeaponParticles.Play();
                weaponAudio.Play();
                if (weaponReloadAudio.isPlaying) { weaponReloadAudio.Stop(); }
            }

            foreach (GameObject go in otherPlayers)
            {
                Vector3 tgtDirection = (go.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, tgtDirection);
                if (Vector3.Distance(transform.position, go.transform.position) < weaponRange && angle <= weaponAngle)
                {
                    Player playerHandler = go.GetComponent<Player>();
                    if (playerHandler.isKnockedDown) { tgtDirection = Vector3.zero; }
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
            if (movementAudio.mute) { movementAudio.mute = false; }
        }
        else
        {
            if (!movementAudio.mute) { movementAudio.mute = true; }
        }
        rb.AddForce(movementVector * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    public void GameOver(Player winner)
    {
        gameOver = true;

        if (winner == this)
        {
            iWon = true;
            myShip.GetComponent<Spaceship>().TakeOff();
            playerGraphic.SetActive(false);
            playerKnockdownGraphic.SetActive(false);
            GameController.winGraphic = winTexture;
        }
    }
}