using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] bool m_canRotate = true;
    public Vector3 m_queueOffset;

    public GameObject[] m_glowSquareFX;
    public string glowSquareTag = "LandShapeFx";

    // Start is called before the first frame update
    void Start()
    {
        if(glowSquareTag != "")
        {
            m_glowSquareFX = GameObject.FindGameObjectsWithTag(glowSquareTag);
        }
    }

    public void LandShapeFX()
    {
        int i = 0;
        
        foreach (Transform child in gameObject.transform)
        {
            if (m_glowSquareFX[i])
            {
                m_glowSquareFX[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
                ParticlePlayer particlePlayer = m_glowSquareFX[i].GetComponent<ParticlePlayer>();

                if(particlePlayer != null)
                {
                    particlePlayer.Play();
                }
            }

            i++;
        }
    }

    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void RotateRight()
    {
        if(m_canRotate)
            transform.Rotate(0, 0, -90);
    }

    public void RotateLeft()
    {
        if(m_canRotate)
            transform.Rotate(0, 0, 90);
    }

    public void RotateClockwise(bool clockwise)
    {
        if (clockwise)
        {
            RotateRight();
        }
        else
        {
            RotateLeft();
        }
    }
}
