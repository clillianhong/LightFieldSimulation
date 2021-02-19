using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Simulation.Utils;

namespace Simulation
{

    public class LightField
    {
        public string sessionName;
        public Vector3 focalPoint;
        public float sphereRadius;
        public CaptureView[] captures;

        public LightField(LightFieldJsonData data)
        {

            sessionName = data.sessionName;
            focalPoint = data.focalPoint;
            sphereRadius = data.sphereRadius;
            captures = new CaptureView[data.captures.Length];

            for (int x = 0; x < captures.Length; x++)
            {
                CaptureJSONData captureData = data.captures[x];
                string pngPath = Loader.PathFromSessionName(data.sessionName) + "CaptureImages/" + captureData.imageFileName;
                captures[x] = new CaptureView(captureData, Loader.TextureFromPNG(pngPath));
            }

            Debug.Log("successfully loaded light field with " + captures[0].capturePosition + " capture position");

        }
    }

    [System.Serializable]
    public class LightFieldJsonData
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


        //initialize when capturing in simulation
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

        //initialize with json data object
        public CaptureView(CaptureJSONData jSONData, Texture2D tex)
        {
            id = jSONData.imageFileName;
            texture = tex;
            viewProjMatrix = jSONData.transform.projMatrix;
            texture = tex;
            jsonData = jSONData;
            capturePosition = jsonData.transform.position;
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