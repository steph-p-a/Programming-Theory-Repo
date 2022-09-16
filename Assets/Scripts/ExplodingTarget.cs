using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ExplodingTarget : Target
{
    [SerializeField] private Material altMaterial;
    [SerializeField] private GameObject explosionPrefab;
    void Start()
    {
        // ExplodingTargets are worth more, and they have 2 HP!
        m_targetXP = 3;
        m_targetHP = 2;

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
        if (m_targetHP <= 0)
        {
            Explode();
        }
        else
        {
            // On first hit, show the Exploding target with the altMaterial
            var mr = GetComponent<MeshRenderer>();
            if (mr && altMaterial)
            {
                mr.material = altMaterial;
            }
        }

    }
}
