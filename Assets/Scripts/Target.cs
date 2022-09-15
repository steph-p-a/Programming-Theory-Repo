using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    protected int hitsNeeded = 1;
    private int m_ScoreValue = 1;

    public int ScoreValue {
        get { return m_ScoreValue; }
        protected set { m_ScoreValue = value; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
