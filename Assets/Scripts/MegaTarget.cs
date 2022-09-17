using UnityEngine;

public class MegaTarget : Target
{
    [SerializeField] private GameObject rocketPrefab;

    void Start()
    {
        // MegaTargets are worth more
        m_targetXP = 3;
    }

    // Send a rocket from the 'position' in the 'direction' 
    private void LauchRocket(Vector3 position, Vector3 direction)
    {
        Instantiate(rocketPrefab, position, rocketPrefab.transform.rotation).transform.Rotate(direction);
    }

    // When a MegaTarget loses all its HP, it sends rockets in four directions
    public override void LoseHP(int hp)
    {
        base.LoseHP(hp);
        if (m_targetHP <= 0)
        {
            var left = new Vector3(transform.position.x - 0.10f, transform.position.y, transform.position.z);
            LauchRocket(left, new Vector3(0, 0, 270));
            var up = new Vector3(transform.position.x, transform.position.y + 0.10f, transform.position.z);
            LauchRocket(up, new Vector3(0, 0, 0));
            var right = new Vector3(transform.position.x + 0.10f, transform.position.y, transform.position.z);
            LauchRocket(right, new Vector3(0, 0, 90));
            var down = new Vector3(transform.position.x, transform.position.y - 0.10f, transform.position.z);
            LauchRocket(down, new Vector3(0, 0, 180));
        }
    }
}
