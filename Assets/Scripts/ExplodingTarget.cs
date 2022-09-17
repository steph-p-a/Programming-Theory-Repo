using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ExplodingTarget : Target
{
    [SerializeField] private Material altMaterial;
    [SerializeField] private GameObject explosionPrefab;

    protected override void Awake()
    {
        base.Awake();
        // ExplodingTargets are worth more, and they have 2 HP!
        TargetXP = 3;
        TargetHP = 2;
    }

    void Start()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("ExplodingTarget does not have an explosion prefab");
        }
        if (altMaterial == null)
        {
            Debug.LogError("ExplodingTarget does not have altMaterial");
        }
    }

    void Explode()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

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
