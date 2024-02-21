using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class WallScript : MonoBehaviour
{
    /*
    //[SerializeField] Transform capsule;
    [SerializeField] Transform shoe_right;
    [SerializeField] Transform wall;
    private Vector4[] landmarks;
    private Vector3 new_heel;
    private Vector3 new_ankle;
    private Vector3 new_toe;
    private Vector3 knee_right;
    //private Vector3 old_heel;
    //private Vector3 old_ankle;
    //private Vector3 old_toe;
    //private Vector3 old_knee_right;
    //private Vector3 new_shoe_right;
    private int scale_old = 120;
    //private int counter = 0;
    //private int delta = 0;
    private Vector3 dir_new;
    private Vector3 dir_up;

    void Start()
    {
        landmarks = new Vector4[100];
        new_heel = new Vector3();
        new_ankle = new Vector3();
        new_toe = new Vector3();
        knee_right = new Vector3();
        dir_new = new Vector3();
        dir_up = new Vector3();
        //old_heel = new Vector3();
        //old_ankle = new Vector3();
        //old_toe = new Vector3();
        //old_knee_right = new Vector3();
        //new_shoe_right = new Vector3();

    }

    public Vector3 ScaleVector(Transform transform)
    {
        return new Vector3(10 * transform.localScale.x, 10 * transform.localScale.z, 1);
    }

    public Vector3 GetPositionFromNormalizedPoint(Transform screenTransform, float x, float y, bool isFlipped)
    {
        var relX = (isFlipped ? -1 : 1) * (x - 0.5f);
        //var relY = 0.5f - y;
        var relY = (isFlipped ? -1 : 1) * (0.5f - y);


        return Vector3.Scale(new Vector3(relX, relY, 0), ScaleVector(screenTransform)) + screenTransform.position;
    }


    void Update()
    {

        
        //capsule.position = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[17].x,
        //PoseVisuallizer3D.landmarks[17].y, true);

        //shoe_right.Find("toe").transform.position = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[32].x,
        //PoseVisuallizer3D.landmarks[32].y, true);
        new_toe = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[32].x,
        PoseVisuallizer3D.landmarks[32].y, true);
        new_heel = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[30].x,
            PoseVisuallizer3D.landmarks[30].y, true);
        new_ankle = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[28].x,
            PoseVisuallizer3D.landmarks[28].y, true);
        knee_right = GetPositionFromNormalizedPoint(wall, PoseVisuallizer3D.landmarks[26].x,
            PoseVisuallizer3D.landmarks[26].y, true);

       
        if (new_toe != null && new_heel != null || new_toe != null && new_ankle != null && knee_right != null)
        {
            if (new_toe != null && new_heel != null)
            {
                //Vector3 dir_old = shoe_right.Find("toe").transform.position - shoe_right.Find("heel").transform.position;
                //dir_old = dir_old.normalized;
                dir_new = new_toe - new_heel;
                //Debug.Log("shoe length: " + dir_new);
                //dir_up = knee_right - new_ankle;
                if (dir_new.x > 0)
                {
                    dir_up = Quaternion.Euler(0, 0, 90) * dir_new;
                }
                else
                {
                    dir_up = Quaternion.Euler(0, 0, -90) * dir_new;
                }

                dir_up = dir_up.normalized;
                dir_new = dir_new.normalized;
                //shoe_right.eulerAngles = dir_new;
                //shoe_right.Rotate(dir_new);
                //shoe_right.rotation = Quaternion.Euler(dir_new);
                //shoe_right.rotation = Quaternion.FromToRotation(new Vector3(0,0,1), dir_new);
                shoe_right.rotation = Quaternion.LookRotation(dir_new, dir_up);
                //Debug.Log("dir: " + dir_new);
                //Debug.Log("shoe_right_dir: " + shoe_right.rotation.eulerAngles);
            }
            else if (new_toe != null && new_ankle != null && knee_right != null)
            {
                //Vector3 dir_old = shoe_right.Find("toe").transform.position - shoe_right.Find("ankle").transform.position;
                //dir_old = dir_old.normalized;
                dir_new = new_toe - new_ankle;
                //Debug.Log("shoe length: " + dir_new);
                dir_new = dir_new.normalized;
                //dir_up = knee_right - new_ankle;
                dir_up = Quaternion.Euler(90, 0, 0) * dir_new;
                dir_up = dir_up.normalized;
                dir_up = Quaternion.Euler(0, 0, 0) * dir_up;
                //shoe_right.eulerAngles = dir_new;
                //Debug.Log("dir: " + dir_new);
                //Debug.Log("shoe_right_dir: " + shoe_right);
                shoe_right.rotation = Quaternion.LookRotation(dir_new, dir_up);
            }

           


            if (knee_right != null && new_heel != null)
            {
                //float scale = (new_toe - new_heel).magnitude / (old_toe - old_heel).magnitude;
                float scale = (knee_right - new_heel).magnitude;
                //Debug.Log("Scale knee - heel: " + scale);
                float scaleing = scale / 400;
                int sc = (int)(scale_old * scaleing);
                Vector3 vector = new Vector3(sc, sc, sc);
                shoe_right.localScale = vector;
                //Debug.Log("Scale after all: " + shoe_right.localScale);
            }


            //shoe_right.Find("toe").transform.position = new_toe;
            Vector3 pos = new Vector3();
            if (new_ankle != null && new_toe != null && new_heel != null)
            {
                //Vector3 dir_up = 
                pos.y = new_heel.y - 100.0f;
                //Debug.Log("y hight: " + pos.y);
                pos.x = (new_toe.x + new_ankle.x) / 2.0f;
                //pos.z = (new_ankle.z + new_toe.z) / 2 ;
                pos.z = new_heel.z - 30.0f;
                shoe_right.position = pos;

            }

            


        }
    }*/
}



