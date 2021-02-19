using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using Simulation.Utils;

namespace Simulation.Viewer
{

    public enum LFManagerState
    {
        SETUP_FOCAL,
        SETUP_RADIUS,

        ACTIVE,
        STOPPED
    }
    public class LightFieldViewManager : MonoBehaviour
    {

        public GameObject focalPointPrefab;

        public string lightFieldName;
        private GameObject _focalPoint;

        public GameObject projectorPlanePrefab;
        private GameObject _projectorPlane;


        public GameObject focalPoint
        {
            get { return _focalPoint; }
            set { _focalPoint = value; }
        }

        private LightField _lightField;


        #region Private Variables
        private MLInput.Controller _controller;
        private LFManagerState _currentState;

        private Camera _camera;
        #endregion

        // Start is called before the first frame update
        void Start()
        {

            //start MLInput API 
            MLInput.Start();
            //assign callback
            MLInput.OnControllerButtonDown += OnButtonDown;
            MLInput.OnControllerButtonUp += OnButtonUp;
            _controller = MLInput.GetController(MLInput.Hand.Left);
            _currentState = LFManagerState.SETUP_FOCAL;
            Debug.Log("starting");

            string jsonPath = Loader.PathFromSessionName(lightFieldName) + "/capture.json";
            _lightField = new LightField(JsonUtility.FromJson<LightFieldJsonData>(Simulation.Utils.Loader.LoadJsonText(jsonPath)));
            _camera = Camera.main;
            InvokeRepeating("UpdateProjectorPlane", 2.0f, 0.3f);

        }

        void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
        {
            if (button == MLInput.Controller.Button.Bumper && _currentState == LFManagerState.SETUP_FOCAL)
            {
                Debug.Log("entering");
                Vector3 focalPos = _controller.Position;
                focalPoint = Instantiate(focalPointPrefab, focalPos, _controller.Orientation);
                _projectorPlane = Instantiate(projectorPlanePrefab, focalPos, _controller.Orientation);
                transitionState(LFManagerState.SETUP_RADIUS);
            }
        }

        void OnButtonUp(byte controllerId, MLInput.Controller.Button button)
        {
            if (button == MLInput.Controller.Button.Bumper)
            {

            }
            if (button == MLInput.Controller.Button.HomeTap)
            {

            }
        }


        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {

        }

        void UpdateProjectorPlane()
        {
            if (_currentState != LFManagerState.SETUP_FOCAL)
            {
                _projectorPlane.transform.up = -_camera.transform.forward;
            }
        }

        void OnDestroy()
        {
            MLInput.OnControllerButtonDown -= OnButtonDown;
            MLInput.OnControllerButtonUp -= OnButtonUp;
            MLInput.Stop();
        }



        void transitionState(LFManagerState newState)
        {
            LFManagerState oldState = _currentState;
            _currentState = newState;
        }
    }

}