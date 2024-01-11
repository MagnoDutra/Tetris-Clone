using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;

    Shape m_activeShape;

    [SerializeField] float m_dropInterval = 0.2f;
    float m_timeToDrop;
    /*
    float m_timeToNextKey;
    
    [Range(0.02f, 1f)]
    [SerializeField] private float m_keyRepeatRate = 0.25f;
    */
    float m_timeToNextKeyLeftRight;
    
    [Range(0.02f, 1f)]
    [SerializeField] private float m_keyRepeatRateLeftRight = 0.15f;

    float m_timeToNextKeyDown;

    [Range(0.01f, 1f)]
    [SerializeField] private float m_keyRepeatRateDown = 0.02f;

    float m_timeToNextKeyRotate;

    [Range(0.02f, 1f)]
    [SerializeField] private float m_keyRepeatRateRotate = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }
        else
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);

            if (m_activeShape == null)
            {
                m_activeShape = m_spawner.SpawnShape();
            }
        }
    }

    void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
        }
        else if (Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate)
        {
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropInterval;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                LandShape();
            }

        }
    }

    private void LandShape()
    {

        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape();

        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_gameBoard.ClearAllRows();
    }

    private void Update()
    {
        if(!m_spawner || !m_gameBoard || !m_activeShape)
        {
            return;
        }

        PlayerInput();
    }
}
