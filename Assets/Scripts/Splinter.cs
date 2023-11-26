using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splinter : MonoBehaviour
{
    public float speed = 0f;
    public float TimeToDestroy = 5f;
    private float destroyTime;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        destroyTime = Time.time + TimeToDestroy;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ITankComponent tankComponent = collision.gameObject.GetComponent<ITankComponent>();
        if (tankComponent != null)
        {
            tankComponent.BreakDown();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
