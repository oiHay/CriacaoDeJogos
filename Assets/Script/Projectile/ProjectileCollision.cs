using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle; // Referência para partícula de explosão
    [SerializeField] private float bounderZ; // Variável que dita o valor limite de Z

    private float _projectileDamage; 

    public void Initialize(float damage)
    {
        _projectileDamage = damage;
    }
    
    private void Update()
    {
        OutOfBounder();
    }

    private void OutOfBounder()
    {
        Vector3 pos = transform.position; // a variável pos pega os valores do transform.position

        if (pos.z >= bounderZ || pos.z <= -bounderZ) // se pos.z for menor/maior/igual os valores limites
        {
            Destroy(gameObject); // Destrói esse GameObject
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Se o GameObject tiver a tag enemy
        {
            EnemyBehaviour behaviour = other.GetComponent<EnemyBehaviour>(); // Cria referência ao componente EnemyBehaviour 
            
            if (behaviour != null)
                behaviour.TakeDamage(_projectileDamage); // Para poder invocar a action TakeDamage, que garante que o enemy receba dano
            
            CallParticle(); // Chama método que instancia a partícula
            DisableProjectile(); // Chama método que desativa partícula após sua animação
            Destroy(gameObject); // Destroi esse GameObject
        }

        if (other.CompareTag("Projectile"))
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
        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
    }
}
