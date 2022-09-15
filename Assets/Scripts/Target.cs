using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    protected int hitsNeeded = 1;
    private const float m_speed = 1.0f;
    private const float m_leftBoundary = -4.5f;
    private const float m_rightBoundary = 4.5f;

    public enum Direction
    {
        None,
        Left,
        Right
    }
    public Direction MoveDirection { get; set; }


    private int m_ScoreValue = 1;

    public int ScoreValue
    {
        get { return m_ScoreValue; }
        protected set { m_ScoreValue = value; }
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        Hit();
    }

    protected void Hit()
    {
        hitsNeeded--;
        if (hitsNeeded <= 0)
        {
            Destroy(gameObject);
        }
    }
}
