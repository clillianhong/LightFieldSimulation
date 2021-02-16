using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation
{

    [System.Serializable]
    public class LightField
    {

        public string sessionName;
        public Vector3 focalPoint;
        public float sphereRadius;
        public CaptureJSONData[] captures;


    }

    public struct CaptureView
    {
        public string id;
        public Texture2D texture;
        public Matrix4x4 viewProjMatrix;

        public CaptureJSONData jsonData;

        public Vector3 capturePosition;


        public CaptureView(string imageFileName, Texture2D tex, Matrix4x4 vpm, Transform trans, Vector3 pos)
        {
            id = imageFileName;
            texture = tex;
            viewProjMatrix = vpm;
            capturePosition = pos;
            jsonData = new CaptureJSONData();
            jsonData.imageFileName = imageFileName;
            jsonData.transform = new TransformJSONData();
            jsonData.transform.forward = trans.forward;
            jsonData.transform.up = trans.up;
            jsonData.transform.right = trans.right;
            jsonData.transform.position = trans.position;
            jsonData.transform.projMatrix = vpm;
        }
    }



    [System.Serializable]
    public class CaptureJSONData
    {
        public string imageFileName;

        public TransformJSONData transform;


    }
    [System.Serializable]
    public class TransformJSONData
    {
        public Vector3 forward;
        public Vector3 up;
        public Vector3 right;
        public Vector3 position;
        public Matrix4x4 projMatrix;
    }

}