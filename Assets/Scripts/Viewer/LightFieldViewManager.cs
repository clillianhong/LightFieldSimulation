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

        public GameObject focalPoint
        {
            get { return _focalPoint; }
            set { _focalPoint = value; }
        }

        private LightField _lightField;


        #region Private Variables
        private MLInput.Controller _controller;
        private LFManagerState _currentState;
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

            string jsonPath = Loader.PathFromSessionName(lightFieldName) + "/capture.json";
            _lightField = new LightField(JsonUtility.FromJson<LightFieldJsonData>(Simulation.Utils.Loader.LoadJsonText(jsonPath)));
        }

        void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
        {
            if (button == MLInput.Controller.Button.Bumper && _currentState == LFManagerState.SETUP_FOCAL)
            {
                Debug.Log("entering");
                placeFocalPoint(_controller.Position, _controller.Orientation);
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

        void OnDestroy()
        {
            MLInput.OnControllerButtonDown -= OnButtonDown;
            MLInput.OnControllerButtonUp -= OnButtonUp;
            MLInput.Stop();
        }

        void placeFocalPoint(Vector3 position, Quaternion rotation)
        {
            focalPoint = Instantiate(focalPointPrefab, position, rotation);
        }

        void transitionState(LFManagerState newState)
        {
            LFManagerState oldState = _currentState;
            _currentState = newState;
        }
    }

}