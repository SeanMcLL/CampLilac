using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SmartCamera : MonoBehaviour
{
    //A transform that holds the target the camera wants to focus on
    public Transform target;
    //How fast the camera will seek to the position of the target
    public float seekSpeed;
    //Bounds of the level
    public int left_level_bound;
    public int right_level_bound;
    public Transform leftLevelWall;
    public Transform rightLevelWall;
    

    // Update is called once per frame
    void LateUpdate()
    {
        //Seek to target without lerping
        //Set bounds to invisible walls so player cannot go beyond the level
        //camera will only track player through set level bounds and stop when you get to the end of a level
        if (target.position.x > right_level_bound)
        {
            target.position = new Vector3(right_level_bound, transform.position.y, transform.position.z);
        }
        else if (target.position.x < left_level_bound)
        {
            target.position = new Vector3(left_level_bound, transform.position.y, transform.position.z);
        }
        else 
        {
            //update camera position to the players position
            //transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
            //Every frame lerp the position of the camera from its current position to the target position based on the seekSpeed
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, target.position.x, seekSpeed*Time.deltaTime), transform.position.y, transform.position.z);
        }

        //------------------------------------------------------


    }
}
