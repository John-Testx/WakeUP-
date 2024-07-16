using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    float countdown;
    Rigidbody rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    public bool hasBeenThrown;
    [SerializeField] int damage;
    public float blastRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;
    bool hasExploded = false;
    int grenadesExploded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenThrown)
        {
            Countdown();
        }
    }

    

    void Countdown()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) { return; }

        grenadesExploded = 0;
        hasExploded = true;

        // Show effects
        GameObject explosion = Instantiate(explosionEffect,transform.position,transform.rotation);
        ParticleSystem ps = explosionEffect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            Destroy(explosion, ps.main.duration);
        }
        else
        {
            // Fallback: destroy after a fixed duration if no ParticleSystem found
            Destroy(explosion, 2f);
        }

        audioSource.PlayOneShot(audioClip);

        // Get nearby objects
        Collider [] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        List<CharacterStats> stats = new List<CharacterStats>();

        foreach (Collider collider in colliders) 
        {
            
            if (collider.TryGetComponent<Grenade>(out Grenade grenade))
            {
                if (!grenade.hasExploded)
                {
                    grenadesExploded++;
                    grenade.hasExploded = true;
                    GameObject explosion2 = Instantiate(explosionEffect, collider.transform.position, collider.transform.rotation);
                    Destroy(explosion2, ps.main.duration);
                    Destroy(collider.gameObject);
                }
            }

            if (collider.TryGetComponent(out CharacterStats character)) { if (!stats.Contains(character)) { stats.Add(character); } }

            if (collider.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }

        grenadesExploded++;
        ApplyDamage(stats);
        StartCoroutine(DestroyAfterSound());
    }

    void ApplyDamage(List<CharacterStats> array)
    {
        Debug.Log(grenadesExploded);

        foreach (var i in array)
        {
            if (grenadesExploded == 1) { i.TakeDamage(damage); }
            else {   i.TakeDamage(damage * grenadesExploded / 2);    }
        }
    }

    IEnumerator DestroyAfterSound()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(audioClip.length);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (hasBeenThrown) { Gizmos.DrawSphere(transform.position, blastRadius); }
    }

}
