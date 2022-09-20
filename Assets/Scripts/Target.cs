using UnityEngine;

public class Target : MonoBehaviour
{
    private float LeftBoundary;
    private float RightBoundary;

    // ENCAPSULATION : nobody can modify the Target's XP value or current HP but classes of the Target hierarachy.
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

    protected virtual void Start()
    {
        LeftBoundary = GameManager.Instance.LeftBoundary;
        RightBoundary = GameManager.Instance.RightBoundary;
    }

    protected virtual void Update()
    {
        Move();
    }

    private void Move()
    {
        var Speed = GameManager.Instance.TargetsSpeed;
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
        // POLYMORPHISM : Here we call the 'LoseHP' method. Which one will be called? Target.LoseHP? MegaTarget.LoseHP? ExplodingTarget.LoseHP?
        // We don't know, and that is what polymorphism is about. We can use derived classes as if they were the base class. Polymorphism will
        // take care of findind the real object's implementation method.
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
