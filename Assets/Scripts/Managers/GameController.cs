using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;

    Shape m_activeShape;

    [SerializeField] float m_dropInterval = 0.2f;
    float m_dropIntervalModded;
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

    bool m_gameOver = false;

    public GameObject m_gameOverPanel;

    SoundManager m_soundManager;

    public IconToggle m_rotIconToggle;
    bool m_clockwise = true;

    public bool m_isPaused = false;
    public GameObject m_pausePanel;

    ScoreManager m_scoreManager;

    Ghost m_ghost;

    Holder m_holder;


    // Start is called before the first frame update
    void Start()
    {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_ghost = GameObject.FindObjectOfType<Ghost>();
        m_holder = GameObject.FindObjectOfType<Holder>();

        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING! There is no Sound Manager defined!");
        }

        if (!m_scoreManager)
        {
            Debug.LogWarning("WARNING! There is no Score Manager defined!");
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

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }

        m_dropIntervalModded = m_dropInterval;
    }

    void PlayerInput()
    {
        if ((Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
            

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }
        else if ((Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }
        else if (Input.GetButtonDown("Rotate") && Time.time > m_timeToNextKeyRotate)
        {
            //m_activeShape.RotateRight();
            m_activeShape.RotateClockwise(m_clockwise);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                //m_activeShape.RotateLeft();
                m_activeShape.RotateClockwise(!m_clockwise);
                PlaySound(m_soundManager.m_errorSound, 0.5f);
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 0.5f);
            }
        }
        else if ((Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown)) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropIntervalModded;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            m_activeShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if (m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }else if (Input.GetButtonDown("ToggleRot"))
        {
            ToggleRotDirection();
        }else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
        else if (Input.GetButtonDown("Hold"))
        {
            Hold();
        }
    }

    void PlaySound(AudioClip clip, float volMultiplier = 1f)
    {
        if (m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    private void GameOver()
    {
        m_activeShape.MoveUp();
        m_gameOver = true;

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }

        PlaySound(m_soundManager.m_gameOverSound, 5f);
        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);
    }

    private void LandShape()
    {
        if (!m_activeShape)
        {
            return;
        }

        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);

        m_activeShape.LandShapeFX();

        if (m_ghost)
        {
            m_ghost.ResetShape();
        }

        if (m_holder)
        {
            m_holder.m_canRelease = true;
        }

        m_activeShape = m_spawner.SpawnShape();

        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_gameBoard.StartCoroutine("ClearAllRows");

        PlaySound(m_soundManager.m_dropSound, 0.75f);

        if(m_gameBoard.m_completedRows > 0)
        {
            m_scoreManager.ScoreLines(m_gameBoard.m_completedRows);
            if (m_scoreManager.m_didLevelUp)
            {
                PlaySound(m_soundManager.m_levelUpVocalClip);
                m_dropIntervalModded =Mathf.Clamp( m_dropInterval - (((float)m_scoreManager.m_level - 1) * 0.05f), 0.05f, 1f);
            }
            else
            {
                if (m_gameBoard.m_completedRows > 1)
                {
                    AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                    PlaySound(randomVocal);
                }
            }

            PlaySound(m_soundManager.m_clearRowSound, 0.75f);
        }
    }

    private void Update()
    {
        if(!m_spawner || !m_gameBoard || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager)
        {
            return;
        }

        PlayerInput();
    }

    private void LateUpdate()
    {
        if (m_ghost)
        {
            m_ghost.DrawGhost(m_activeShape, m_gameBoard);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if (m_rotIconToggle)
        {
            m_rotIconToggle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;
            }

            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }
    
    public void Hold()
    {
        if (!m_holder)
        {
            return;
        }

        if(!m_holder.m_heldShape)
        {
            m_holder.Catch(m_activeShape);

            m_activeShape = m_spawner.SpawnShape();
            PlaySound(m_soundManager.m_holdSound);
        }
        else if(m_holder.m_canRelease)
        {
            Shape temp = m_activeShape;
            m_activeShape = m_holder.Release();
            m_activeShape.transform.position = m_spawner.transform.position;
            m_holder.Catch(temp);
            PlaySound(m_soundManager.m_holdSound);
        }
        else
        {
            Debug.LogWarning("HOLDER WARNING! Wait for cooldown!");
            PlaySound(m_soundManager.m_errorSound);
        }

        if (m_ghost)
        {
            m_ghost.ResetShape();
        }
    }
}
