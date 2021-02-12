using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Responsible for the basic motion controls of a Camera 
*/

namespace Simulation
{
    public class CameraController : MonoBehaviour
    {

        private Transform focus;

        public float speed;
        public OrbitViewManager.CameraType type;


        OrbitViewManager viewManager;

        void Start()
        {
            viewManager = GameObject.Find("OrbitViewManager").GetComponent<OrbitViewManager>();
            focus = viewManager.focalPoint.transform;

            //move to appropriate distance 
            transform.position = new Vector3(focus.position.x + viewManager.distance, focus.position.y, focus.position.z);
            transform.LookAt(focus);
        }

        // Update is called once per frame
        void Update()
        {

            if (viewManager.Controlling() == type)
            {
                Vector3 worldX = transform.TransformDirection(Vector3.right);
                Vector3 worldY = transform.TransformDirection(Vector3.up);
                transform.RotateAround(focus.position, worldY, -Input.GetAxis("Horizontal") * speed * Time.deltaTime);
                transform.RotateAround(focus.position, worldX, Input.GetAxis("Vertical") * speed * Time.deltaTime);
                transform.LookAt(focus);
            }
        }




    }

}
