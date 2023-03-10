using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { None, Hard, ExtraHard }

public class Enemy : MonoBehaviour
{

    private GameObject player;
    private Rigidbody enemyRb;
    [SerializeField] EnemyType currentEnemy = EnemyType.None;

    public float speed;
    private float forceMultiplier = 2.0f;
    public int points;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
    }

    // Update is called once per frame
    void Update()
    {

        //look direction is returning a vector that represents the distance between the playerTransform and enemy. This tells the enemy where to move
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        if(currentEnemy == EnemyType.ExtraHard)
        {
            enemyRb.AddForce(lookDirection * forceMultiplier * speed);
        }
        else if (currentEnemy == EnemyType.Hard)
        {
            enemyRb.AddForce(lookDirection * speed);
        }
                
        if (transform.position.y < -3)
        {
            Destroy(gameObject);
            
        }

    }

}
