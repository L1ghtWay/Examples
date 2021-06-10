using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach : MonoBehaviour
{
    private Vector3 savePos;
    private Quaternion saveRot;    
    private bool oneClick;
    private bool inCenter = false;

    private float timeForDoubleClick;
    private float speedRotateX = 150;
    private float speedRotateY = 150;

    [System.Obsolete]
    private void Update()
    {
        if (inCenter)
        {
            if (!Input.GetMouseButton(0))
                return;

            float rotX = Input.GetAxis("Mouse X") * speedRotateX * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * speedRotateY * Time.deltaTime;

            if (Mathf.Abs(rotX) > Mathf.Abs(rotY))
            {
                transform.Rotate(transform.up, -rotX);
            }
            else
            {
                transform.Rotate(transform.right, -rotY);
            }
        }
    }
    
#if UNITY_ANDROID

#endif

#if UNITY_EDITOR
    private void OnMouseDown()
    {       
        if (!oneClick)
        {
            oneClick = true;
            timeForDoubleClick = Time.time;
            Debug.Log("первый клик");
        }
        else
        {
            if (Time.time - timeForDoubleClick > 0.3f)
            {
                timeForDoubleClick = Time.time;
            }
            else
            {
                Debug.Log("второй клик");
                if (!inCenter) ApproachCube();
                else RemoveCube();
                oneClick = false;
            }
        }

        if (!inCenter)
        {
            savePos = transform.position;
            saveRot = transform.rotation;
        }        
    }
#endif


    private void ApproachCube ()
    {
        if (!LevelGenerator.Instance.CubeInCenter)
        {
            transform.position = new Vector3(0, 2, 2);
            inCenter = true;
            LevelGenerator.Instance.CubeInCenter = true;
            LevelGenerator.Instance.StopSwipe(); // Останавлием возможность передигать кубы при приближении одного из них
        }
    }


    private void RemoveCube ()
    {
        transform.position = savePos;
        transform.rotation = saveRot;
        inCenter = false;
        LevelGenerator.Instance.CubeInCenter = false;
        LevelGenerator.Instance.StartSwipe();                
    }
}