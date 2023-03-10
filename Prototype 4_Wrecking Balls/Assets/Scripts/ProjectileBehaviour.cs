using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

    private Transform target;
    [SerializeField] float speed = 15.0f;
    private bool homing;
    [SerializeField] float rocketStrength = 15.0f;
    private float aliveTimer = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {
            if (collision.gameObject.CompareTag(target.tag))
            {
                Rigidbody targetRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.GetContact(0).normal; //this uses the normal point of contact to determine the direction to push the target in
                targetRb.AddForce(away * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }

    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        Destroy(gameObject, aliveTimer);

    }
}
