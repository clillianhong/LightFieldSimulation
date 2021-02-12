using UnityEngine;
using System.IO;

namespace Simulation
{
    public class CameraCaptureController : MonoBehaviour
    {
        public int fileCounter;
        public KeyCode screenshotKey;

        public int captureWidth;
        public int captureHeight;

        CaptureViewCollection collectionManager;
        private Camera _camera;

        //autocapture
        public bool autoCapture;
        public KeyCode triggerOrbitCaptureKey;
        public int divisions;
        bool currentlyCapturing;
        OrbitViewManager viewManager;

        private void Start()
        {
            collectionManager = GameObject.Find("OrbitViewManager").GetComponent<CaptureViewCollection>();
            viewManager = GameObject.Find("OrbitViewManager").GetComponent<OrbitViewManager>();
            _camera = gameObject.GetComponent<Camera>();
            currentlyCapturing = false;
        }
        private void LateUpdate()
        {
            if (autoCapture)
            {
                if (!currentlyCapturing && Input.GetKeyDown(triggerOrbitCaptureKey))
                {
                    Debug.Log("Starting orbit capture");
                    StartOrbitingCapture();
                }
            }
            else
            {
                if (Input.GetKeyDown(screenshotKey))
                {
                    CaptureAndDisplay(true);

                }
            }

        }


        public void CaptureAndDisplay(bool renderCapture)
        {
            //get current camera image 

            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = _camera.targetTexture;

            _camera.Render();

            Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;

            //get current camera pose 

            Transform cameraTransform = _camera.gameObject.transform;



            // add to list of captures 
            collectionManager.captureViews.Add(new CaptureViewCollection.CaptureView(image, _camera.projectionMatrix * _camera.worldToCameraMatrix, cameraTransform.position, cameraTransform.position));

            if (renderCapture)
            {

                createCapturePoseLabel(cameraTransform, image);
            }
        }

        public void createCapturePoseLabel(Transform trans, Texture2D tex)
        {
            CaptureCreator.CreateCaptureGameObject(trans, tex, captureWidth, captureHeight, "capture-" + fileCounter);
            fileCounter++;
        }



        /// <summary>
        /// Save texture as a PNG within assets/outputFolder/
        /// </summary>
        /// 
        public void SaveToFile(Texture2D image, string outputFolder)
        {

            byte[] bytes = image.EncodeToJPG();
            Destroy(image);

            Debug.Log("Capturing Image");
            File.WriteAllBytes(Application.dataPath + "/" + outputFolder + "/" + fileCounter + ".png", bytes);
            fileCounter++;
        }


        void StartOrbitingCapture()
        {
            currentlyCapturing = true;
            for (int theta = 0; theta < 360; theta += divisions)
            {
                Vector3 worldX = transform.TransformDirection(Vector3.right);
                _camera.gameObject.transform.RotateAround(viewManager.focalPoint.position, worldX, 360 / divisions);
                _RotateHorizontal360();
            }


        }

        void _RotateHorizontal360()
        {

            for (int theta = 0; theta < 360; theta += divisions)
            {
                _camera.gameObject.transform.RotateAround(viewManager.focalPoint.position, Vector3.up, 360 / divisions);
                CaptureAndDisplay(false);
            }
        }



    }
}
