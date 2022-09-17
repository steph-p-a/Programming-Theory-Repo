using UnityEngine;

public class Target : MonoBehaviour
{
    private float Speed;
    private float LeftBoundary;
    private float RightBoundary;

    public int TargetXP { get; protected set; }
    public int TargetHP { get; protected set; }

    public enum Direction
    {
        None,
        Left,
        Right,
        Last = Right,
    }
    public Direction MoveDirection { get; set; }

    protected virtual void Awake()
    {
        TargetXP = 1;
        TargetHP = 1;
    }

    private void Start()
    {
        Speed = GameManager.Instance.TargetsSpeed;
        LeftBoundary = GameManager.Instance.LeftBoundary;
        RightBoundary = GameManager.Instance.RightBoundary;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (MoveDirection == Direction.Left)
        {
            transform.Translate(-Speed * Time.deltaTime, 0, 0);
            if (transform.position.x < LeftBoundary)
            {
                transform.position = new Vector3(RightBoundary, transform.position.y, transform.position.z);
            }
        }
        else if (MoveDirection == Direction.Right)
        {
            transform.Translate(Speed * Time.deltaTime, 0, 0);
            if (transform.position.x > RightBoundary)
            {
                transform.position = new Vector3(LeftBoundary, transform.position.y, transform.position.z);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Hit();
    }

    protected void Hit()
    {
        LoseHP(1);
        if (TargetHP <= 0)
        {
            GameManager.Instance.AddScore(TargetXP);
            Destroy(gameObject);
        }
    }

    // The inherited classes will overload LoseHP
    public virtual void LoseHP(int hp)
    {
        TargetHP -= hp;
    }
}
