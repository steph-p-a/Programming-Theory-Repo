using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

// INHERITANCE : ExplodingTarget is a variant of the regular Target.
// After 2 impacts it destroys everything in a 1.5 unit radius.
public class ExplodingTarget : Target
{
    [SerializeField] private Material altMaterial;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionParticles;

    protected override void Awake()
    {
        base.Awake();
        // ExplodingTargets are worth more, and they have 2 HP!
        TargetXP = 3;
        TargetHP = 2;
    }

    protected override void Start()
    {
        base.Start();

        if (explosionPrefab == null)
        {
            Debug.LogError("ExplodingTarget does not have an explosion prefab");
        }
        if (altMaterial == null)
        {
            Debug.LogError("ExplodingTarget does not have altMaterial");
        }
        if (explosionParticles == null)
        {
            Debug.LogError("ExplodingTarget does not have explosionParticles");
        }
    }

    void Explode()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }
        if (explosionParticles)
        {
            Instantiate(explosionParticles, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    // INHERITANCE: this method overrides the protected method is the base class Target
    // POLYMORPHISM: see the Target class for how it uses the LoseHP method without having to know the derived class
    public override void LoseHP(int hp)
    {
        base.LoseHP(hp);
        if (TargetHP <= 0)
        {
            Explode();
        }
        else
        {
            // On first hit, paint the Exploding target with the altMaterial
            var mr = GetComponent<MeshRenderer>();
            if (mr && altMaterial)
            {
                mr.material = altMaterial;
            }
        }

    }
}
