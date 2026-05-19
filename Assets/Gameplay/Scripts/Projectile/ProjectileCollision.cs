using System;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float bounderX;
    
    private void Update()
    {
        OutOfBounder();
    }

    private void OutOfBounder()
    {
        Vector3 pos = transform.position; // a variável pos pega os valores do transform.position

        if (pos.z >= bounderX || pos.z <= -bounderX) // se pos.z for menor/maior/igual os valores limites
        {
            Destroy(this.gameObject);
        }
        
        pos.z = Mathf.Clamp(pos.z, -bounderX, bounderX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            CallParticle();
            DisableProjectile();
            Destroy(gameObject);
        }

        if (other.CompareTag("Projectile"))
        {
            CallParticle();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void DisableProjectile()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    private void CallParticle()
    {
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }
}
