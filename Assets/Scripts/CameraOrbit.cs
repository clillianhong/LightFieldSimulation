﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Simulation{
    public class CameraOrbit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
	public Transform focus;

	[SerializeField, Range(1f, 20f)]
	float distance = 5f;
    public float speed; 
    public OrbitCameraController.CameraType type;

    OrbitCameraController cameraController;

    void Start()
    {
        cameraController = GameObject.Find("OrbitViewManager").GetComponent<OrbitCameraController>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraController.Controlling() == type){
            Vector3 worldX = transform.TransformDirection(Vector3.right);
            Vector3 worldY = transform.TransformDirection(Vector3.up);
            transform.RotateAround(focus.position, worldY, -Input.GetAxis ("Horizontal") * speed * Time.deltaTime);
            transform.RotateAround(focus.position, worldX, Input.GetAxis ("Vertical") * speed * Time.deltaTime);
            transform.LookAt(focus);
        }
        
    }
}

}
