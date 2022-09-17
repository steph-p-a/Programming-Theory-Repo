using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    [SerializeField] GameObject[] targetPrefabs;
    private readonly float[] m_targetXPositions = { -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f };
    private readonly float[] m_targetYPositions = { 1.0f, 2.0f, 3.0f };
    private const float TargetZPosition = 4.92f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Score = 0;
        DontDestroyOnLoad(gameObject);

        if (targetPrefabs.Length == 0)
        {
            Debug.LogError("GameManager does not have any targetPrefabs");
        }
        // Validate that all target prefabs have the Target component
        for (int i = 0; i < targetPrefabs.Length; i++)
        {
            if (targetPrefabs[i].GetComponent<Target>() == null)
            {
                Debug.LogError("TargetPrefab: " + targetPrefabs[i].name + " does not have the Target class");
            }
        }
    }

    void Start()
    {
        CreateTargets();
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    private void CreateTargets()
    {
        int row = 0;
        // Create a grid of individual targets
        foreach (float y in m_targetYPositions)
        {
            foreach (float x in m_targetXPositions)
            {
                var targetGameObject = Instantiate(targetPrefabs[0], new Vector3(x, y, TargetZPosition), targetPrefabs[0].transform.rotation);
                var target = targetGameObject.GetComponent<Target>();

                if (target)
                {
                    if ((row % 2) == 0)
                    {
                        target.MoveDirection = Target.Direction.Left;
                    }
                    else
                    {
                        target.MoveDirection = Target.Direction.Right;
                    }
                }
            }
            row++;
        }
    }
}
