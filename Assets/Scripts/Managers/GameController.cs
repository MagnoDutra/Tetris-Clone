using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;

    Shape m_activeShape;

    // Start is called before the first frame update
    void Start()
    {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        if (m_spawner)
        {          
            if(m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }
    }

    private void Update()
    {
        if(!m_spawner || !m_gameBoard)
        {
            return;
        }

        if (m_activeShape)
        {
            m_activeShape.MoveDown();
        }
    }
}
