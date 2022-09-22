using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Score { get; private set; }
    public float TimeLeft { get; private set; }
    public bool IsGameRunning { get; private set; }

    [SerializeField] GameObject[] targetPrefabs;
    [SerializeField] float[] targetProbs;
    [SerializeField] GameObject leftStageLimit;
    [SerializeField] GameObject rightStageLimit;
    [SerializeField] GameObject bottomStageLimit;
    [SerializeField] GameObject topStageLimit;
    [SerializeField] GameObject backdrop;
    [SerializeField] TextMeshProUGUI scoreField;
    [SerializeField] TextMeshProUGUI timeField;
    [SerializeField] TextMeshProUGUI gameOver;
    [SerializeField] TextMeshProUGUI finalScore;


    private const float BackdropOffset = 0.08f;
    private const float GameTime = 60.0f;
    private const float GameOverDelay = 3.0f;
    private float startTime;

    private int m_Stage;
    public int m_TargetCount;

    public float TargetsSpeed { get; private set; }
    public float LeftBoundary { get; private set; }
    public float RightBoundary { get; private set; }
    public float TopBoundary { get; private set; }
    public float BottomBoundary { get; private set; }
    public float TargetZPosition { get; private set; }

    public void PlayClip(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

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
        if (targetProbs.Length != targetPrefabs.Length)
        {
            Debug.LogError("GameManager targetProbs and targetPrefabs length do not match");
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
        if (!scoreField)
        {
            Debug.LogError("GameManager does not have a scoreField");
        }
        if (!timeField)
        {
            Debug.LogError("GameManager does not have a timeField");
        }
        if (!gameOver)
        {
            Debug.LogError("GameManager does not have a gameOver");
        }
        if (!gameOver)
        {
            Debug.LogError("GameManager does not have a finalScore");
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

    private void DisplayScore()
    {
        scoreField.text = Score.ToString();
    }

    private void DisplayTime()
    {
        timeField.text = Mathf.CeilToInt(TimeLeft).ToString();
    }

    void Start()
    {
        gameOver.gameObject.SetActive(false);

        m_TargetCount = 0;

        m_Stage = 0;
        TargetsSpeed = 0;

        Score = 0;
        DisplayScore();

        TimeLeft = GameTime;
        DisplayTime();

        NextLevel();
        startTime = Time.time;
        IsGameRunning = true;
    }

    private void FixedUpdate()
    {
        if (IsGameRunning)
        {
            TimeLeft = startTime + GameTime - Time.time;
            DisplayTime();
            if (TimeLeft <= 0.0f)
            {
                GameOver();
            }
            if (m_TargetCount == 0)
            {
                NextLevel();
            }
        }
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(GameOverDelay);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        IsGameRunning = false;
        gameOver.gameObject.SetActive(true);
        finalScore.text = Score.ToString();
        StartCoroutine(nameof(ReturnToMenu));
    }

    private void NextLevel()
    {
        m_Stage++;
        TargetsSpeed = m_Stage;
        CreateTargets(m_Stage);
    }
    public void AddScore(int score)
    {
        if (IsGameRunning)
        {
            Score += score;
            m_TargetCount--;
            DisplayScore();
        }
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

    GameObject ChooseTarget(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return targetPrefabs[i];
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return targetPrefabs[probs.Length - 1];
    }

    private void CreateTargets(float distance)
    {
        int rowIndex = 0;
        List<float> horizontalSpread = CreateEvenSpread(RightBoundary - LeftBoundary, distance, LeftBoundary);
        List<float> verticalSpead = CreateEvenSpread(TopBoundary - BottomBoundary, 1.0f, BottomBoundary + 0.5f);

        // Create a grid of targets
        foreach (float y in verticalSpead)
        {
            foreach (float x in horizontalSpread)
            {
                var targetGameObject = Instantiate(ChooseTarget(targetProbs), new Vector3(x, y, TargetZPosition), targetPrefabs[0].transform.rotation);
                m_TargetCount++;

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
