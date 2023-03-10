using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Gameobject var
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public GameObject extraPowerupIndicator;
    public GameObject projectilePrefab; //used to assign rocket prefab
    private GameManager gameManager;

    public PowerUpType currentPowerUp = PowerUpType.None; //this is calling the power up type from the powerup script and setting it to none

    private GameObject tmpRocket; //used to spawn rocket prefab
   


    //Rigidbody var
    private Rigidbody playerRb;
    private Rigidbody enemyRb;


    //Variables
    public float speed;
    private float outofboundsX = 15.5f;
    private float outofboundsZ = 13.5f;
    private float forwardInput;
    public bool hasPowerup = false;
    public bool hasExtraPowerup = false;
    private float powerupStrength = 15.0f;
    private Vector3 powerupIndicatorOffset;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gameManager = GameManager._gameManager;


    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        //Sets power up position to player position

        powerupIndicatorOffset = new Vector3(0, 0.5f, 0);
        powerupIndicator.transform.position = transform.position - powerupIndicatorOffset;
        extraPowerupIndicator.transform.position = transform.position - powerupIndicatorOffset;

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        //This block of code defines parameters for game over

        if (transform.position.x > outofboundsX || transform.position.x < -outofboundsX)
        {

            Destroy(gameObject);
            gameManager.GameOver();

        }
        else if (transform.position.z > outofboundsZ || transform.position.z < -outofboundsZ)
        {

            Destroy(gameObject);
            gameManager.GameOver();

        }
    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            hasExtraPowerup = false;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType; //specificaly gets the powerup script and its enum variable powerUpType
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);


            if (StartCoroutine(PowerupCountdownRoutine()) != null)
            {
                StopCoroutine(PowerupCountdownRoutine());
            }

        }

        if (other.CompareTag("ExtraPowerup"))
        {
            hasExtraPowerup = true;
            hasPowerup = false;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType; //specificaly gets the powerup script and its enum variable powerUpType
            extraPowerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);

            if (StartCoroutine(PowerupCountdownRoutine()) != null)
            {
                StopCoroutine(PowerupCountdownRoutine());
            }

        }

    }

    //Sets count down timer independant of update method for the power up
    IEnumerator PowerupCountdownRoutine()
    {
        if (currentPowerUp == PowerUpType.Pushback)
        {
            yield return new WaitForSeconds(7);
            hasPowerup = false;
            currentPowerUp = PowerUpType.None;
            powerupIndicator.gameObject.SetActive(false);

        }
        else if (currentPowerUp == PowerUpType.Rockets)
        {
            yield return new WaitForSeconds(7);
            hasExtraPowerup = false;
            currentPowerUp = PowerUpType.None;
            extraPowerupIndicator.gameObject.SetActive(false);
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //applies a propelling force to enemy if player collides while powerup is active
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(projectilePrefab, transform.position + Vector3.forward, Quaternion.identity);
            tmpRocket.GetComponent<ProjectileBehaviour>().Fire(enemy.transform);
        }
    }
}



