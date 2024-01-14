using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int m_score = 0;
    int m_lines;
    int m_level = 1;

    public int m_linesPerLevel = 5;

    const int m_minLines = 1;
    const int m_maxLines = 4;

    public void ScoreLines(int n)
    {
        n = Mathf.Clamp(n, m_minLines, m_maxLines);

        switch (n)
        {
            case 1:
                m_score += 40 * m_level;
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;
        }
    }

    public void ResetScore()
    {
        m_level = 1;
        m_lines = m_linesPerLevel * m_level;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
    }
}
