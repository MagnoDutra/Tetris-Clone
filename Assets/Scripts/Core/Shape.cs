using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] bool m_canRotate = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
