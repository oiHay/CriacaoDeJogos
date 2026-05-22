using UnityEngine;

public class ProjectileCollisionEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private float bounderZ;
    
    private void Update()
    {
        OutOfBounder();
    }

    private void OutOfBounder()
    {
        Vector3 pos = transform.position; // a variável pos pega os valores do transform.position

        if (pos.z <= bounderZ) // se pos.z for menor que o valor limite
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
