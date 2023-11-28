using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiExplosion : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 5f;

    AudioSource explosionSound;

    private void Start()
    {
        //explosionSound = GameObject.Find("BlastAudio").GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;


        Instantiate(explosionEffect, transform.position, transform.rotation);

        //explosionSound.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        Destroy(gameObject);

        Debug.Log("ball destroyed");
    }
}
