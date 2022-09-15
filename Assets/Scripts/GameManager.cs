using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject[] targetPrefabs;

    private readonly float[] targetXPositions = { -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f };
    private readonly float[] targetYPositions = { 1.0f, 2.0f, 3.0f };
    private const float targetZPosition = 4.92f;

    public int Score { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = gameObject.GetComponent<GameManager>();
        Score = 0;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        int row = 0;
        foreach (float y in targetYPositions)
        {
            foreach (float x in targetXPositions)
            {
                var target = Instantiate(targetPrefabs[0], new Vector3(x, y, targetZPosition), targetPrefabs[0].transform.rotation).GetComponent<Target>();

                if ((row % 2) == 0)
                {
                    target.MoveDirection = Target.Direction.Left;
                }
                else
                {
                    target.MoveDirection = Target.Direction.Right;
                }
            }
            row++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddScore(int score)
    {
       Score += score;
    }
}
