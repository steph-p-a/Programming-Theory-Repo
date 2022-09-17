using UnityEngine;

public class Target : MonoBehaviour
{
    protected int m_targetXP = 1;
    public int TargetXP
    {
        get { return m_targetXP; }
        protected set { m_targetXP = value; }
    }

    protected int m_targetHP = 1;
    public int TartgetHP
    {
        get { return m_targetHP; }
        protected set { m_targetHP = value; }
    }
    public enum Direction
    {
        None,
        Left,
        Right,
        Last = Right,
    }
    public Direction MoveDirection { get; set; }


    protected const float Speed = 1.0f;
    private const float LeftBoundary = -4.5f;
    private const float RightBoundary = 4.5f;

    // Update is called once per frame
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
        if (m_targetHP <= 0)
        {
            GameManager.Instance.AddScore(TargetXP);
            Destroy(gameObject);
        }
    }

    // The inherited classes will overload LoseHP
    public virtual void LoseHP(int hp)
    {
        m_targetHP -= hp;
    }
}
