using UnityEngine;

// INHERITANCE : MegaTarget is a variant of the regular Target. On impact, it sends rockets in four directions
public class MegaTarget : Target
{
    [SerializeField] private GameObject rocketPrefab;

    protected override void Awake()
    {
        base.Awake();
        // MegaTargets are worth more
        TargetXP = 3;
    }

    // Send a rocket from the 'position' in the 'direction' 
    private void LauchRocket(Vector3 position, Vector3 direction)
    {
        Instantiate(rocketPrefab, position, rocketPrefab.transform.rotation).transform.Rotate(direction);
    }

    Vector3 XOffset(Vector3 position, float offset)
    {
        return new Vector3(position.x + offset, position.y, position.z);
    }

    Vector3 YOffset(Vector3 position, float offset)
    {
        return new Vector3(position.x, position.y + offset, position.z);
    }

    // INHERITANCE: this method overrides the protected method is the base class Target
    // POLYMORPHISM: see the Target class for how it uses the LoseHP method without having to know the derived class
    // When a MegaTarget loses all its HP, it sends rockets in four directions
    public override void LoseHP(int hp)
    {
        base.LoseHP(hp);
        if (TargetHP <= 0)
        {
            // Lauch the rocket just a little off so they don't collide with one another
            const float offset = 0.15f;
            LauchRocket(YOffset(transform.position, offset), new Vector3(0, 0, 0));    // Up
            LauchRocket(XOffset(transform.position, -offset), new Vector3(0, 0, 90));   // Left
            LauchRocket(YOffset(transform.position, -offset), new Vector3(0, 0, 180)); // Down    
            LauchRocket(XOffset(transform.position, offset), new Vector3(0, 0, 270)); // Right
        }
    }
}
