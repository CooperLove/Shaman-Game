using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Projectile_Shoot_Test projectile = null;
    [SerializeField] private List<ParticleSystem> particles = new List<ParticleSystem>();
    [SerializeField] private float particlesDestroyTime = 0f;

    [SerializeField] private float velocity = 0.0f;
    [SerializeField] private Projectile_Shoot_Test.SpaceDirection direction = Projectile_Shoot_Test.SpaceDirection.right;
    [SerializeField] private Space space = Space.World;

    // private bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Projectile_Shoot_Test>();
        particles = GetComponentsInChildren<ParticleSystem>().ToList();
    }

    private void Update() {
        var dir = direction == Projectile_Shoot_Test.SpaceDirection.up ? Vector3.up : Vector3.right;
        transform.Translate(dir * (velocity * Time.deltaTime), space);
    }


    private void PauseParticles (){

        Destroy(transform.GetChild(0).gameObject);

        foreach (var particle in particles)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = false;
            particle.transform.parent = transform.parent;
            Destroy(particle.gameObject, particlesDestroyTime);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!other.CompareTag("Enemy") || hit) 
    //         return;
        
    //     var player = Player.Instance;
    //     var pi = Player.Instance.playerInfo;

    //     var enemy = other.transform.GetComponent<Enemy>();
    //     enemy?.TakeDamage(pi.CalculatePhysicalDamage());

    //     hit = true;
    //     PauseParticles ();
    // }
}
