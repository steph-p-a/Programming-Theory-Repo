using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }

    [SerializeField] GameObject[] targetPrefabs;
    [SerializeField] GameObject leftStageLimit;
    [SerializeField] GameObject rightStageLimit;
    [SerializeField] GameObject bottomStageLimit;
    [SerializeField] GameObject topStageLimit;
    [SerializeField] GameObject backdrop;

    private readonly float[] m_targetYPositions = { 1.0f, 2.0f, 3.0f };
    private const float BackdropOffset = 0.08f;
    private float TargetZPosition;

    public float TargetsSpeed { get; private set; } = 1.0f;
    public float LeftBoundary { get; private set; }
    public float RightBoundary { get; private set; }
    public float TopBoundary { get; private set; }
    public float BottomBoundary { get; private set; }

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

        ValidateSerializedFields();
        SetupStageBoundaries();

    }

    // It is not going to work if these are not defined
    private void ValidateSerializedFields()
    {
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

        if (!leftStageLimit)
        {
            Debug.LogError("GameManager does not have a leftStageLimit");
        }

        if (!rightStageLimit)
        {
            Debug.LogError("GameManager does not have a rightStageLimit");
        }

        if (!bottomStageLimit)
        {
            Debug.LogError("GameManager does not have a bottomStageLimit");
        }

        if (!topStageLimit)
        {
            Debug.LogError("GameManager does not have a topStageLimit");
        }

        if (!backdrop)
        {
            Debug.LogError("GameManager does not have a backdrop");
        }
    }

    // Calculate the stage limits based on the surrounding objects
    private void SetupStageBoundaries()
    {
        // The targets will 'teleport' when they reach the center of the side borders.
        if (leftStageLimit)
        {
            LeftBoundary = leftStageLimit.transform.position.x;
        }

        if (rightStageLimit)
        {
            RightBoundary = rightStageLimit.transform.position.x;
        }

        /* For vertical limits, we find the 'edge' of the objects by calculating their
         * center position plus or minus half their size, dependending on direction */
        if (topStageLimit)
        {
            TopBoundary = (topStageLimit.transform.position - (topStageLimit.transform.up * topStageLimit.transform.localScale.y / 2.0f)).y;
        }
        if (bottomStageLimit)
        {
            BottomBoundary = (bottomStageLimit.transform.position + (bottomStageLimit.transform.up * bottomStageLimit.transform.localScale.y / 2.0f)).y;
        }

        // We position the targets just shy off the backdrop
        if (backdrop)
        {
            TargetZPosition = backdrop.transform.position.z - BackdropOffset;
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

    private List<float> CreateEvenSpread(float size, float minDistance, float startingValue = 0.0f)
    {
        List<float> list = new List<float>();

        var count = Mathf.FloorToInt(size / minDistance);
        var distance = size / count;
        for (int i = 0; i < count; i++)
        {
            list.Add(startingValue + (i * distance));
        }

        return list;
    }

    private void CreateTargets()
    {
        int rowIndex = 0;
        List<float> horizontalSpread = CreateEvenSpread(RightBoundary - LeftBoundary, 1.0f, LeftBoundary);
        List<float> verticalSpead = CreateEvenSpread(TopBoundary - BottomBoundary, 1.0f, BottomBoundary + 0.5f);

        // Create a grid of targets
        foreach (float y in verticalSpead)
        {
            foreach (float x in horizontalSpread)
            {
                var targetGameObject = Instantiate(targetPrefabs[0], new Vector3(x, y, TargetZPosition), targetPrefabs[0].transform.rotation);

                var target = targetGameObject.GetComponent<Target>();
                if (target)
                {
                    if ((rowIndex % 2) == 0)
                    {
                        target.MoveDirection = Target.Direction.Left;
                    }
                    else
                    {
                        target.MoveDirection = Target.Direction.Right;
                    }
                }
            }
            rowIndex++;
        }


    }
}
