using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float bForce;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
        rb.AddForce(transform.forward * bForce, ForceMode.Impulse);
        Destroy(gameObject, 2.0f);
    }

}
