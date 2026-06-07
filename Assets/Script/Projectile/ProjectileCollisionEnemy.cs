using UnityEngine;

public class ProjectileCollisionEnemy : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle; // Referência para partícula de explosão
    [SerializeField] private float bounderZ; // Variável que dita o valor limite de Z
    
    private void Update()
    {
        OutOfBounder();
    }

    private void OutOfBounder() 
    {
        Vector3 pos = transform.position; // a variável pos pega os valores do transform.position do GameObject

        if (pos.z <= bounderZ) // se pos.z for menor que o valor limite
        {
            Destroy(gameObject); // O GameObject é destruído
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Se o GameObject tiver a tag player
        {
            DisableProjectile(); // Chama método que desativa partícula após sua animação
            Destroy(gameObject); // Destroi esse GameObject
        }

        if (other.CompareTag("Projectile")) // Se o GameObject tiver a tag projectile
        {
            CallParticle(); // Chama método que instancia a partícula
            DisableProjectile(); // Chama método que desativa partícula após sua animação
            Destroy(other.gameObject); // Destroi o outro GameObject
            Destroy(gameObject); // Destroi esse GameObject
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
        if(explosionParticle == null) return;
        
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }
}
