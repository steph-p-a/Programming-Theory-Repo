using UnityEngine;

public class Target : MonoBehaviour
{
    private int m_targetXP = 1;
    public int TargetXP
    {
        get { return m_targetXP; }
        protected set { m_targetXP = value; }
    }

    private int m_targetHP = 1;
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


    private const float m_speed = 1.0f;
    private const float m_leftBoundary = -4.5f;
    private const float m_rightBoundary = 4.5f;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (MoveDirection == Direction.Left)
        {
            transform.Translate(-m_speed * Time.deltaTime, 0, 0);
            if (transform.position.x < m_leftBoundary)
            {
                transform.position = new Vector3(m_rightBoundary, transform.position.y, transform.position.z);
            }
        }
        else if (MoveDirection == Direction.Right)
        {
            transform.Translate(m_speed * Time.deltaTime, 0, 0);
            if (transform.position.x > m_rightBoundary)
            {
                transform.position = new Vector3(m_leftBoundary, transform.position.y, transform.position.z);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Hit();
    }

    protected void Hit()
    {
        m_targetHP--;
        LoseHP(1);
        if (m_targetHP <= 0)
        {
            GameManager.Instance.AddScore(TargetXP);
            Destroy(gameObject);
        }
    }

    // The inherited classes will overload LoseHP
    protected void LoseHP(int hp)
    {
        // The base class does not implement this
    }
}
