using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation{


    
    public class CaptureViewCollection : MonoBehaviour
{
    // Start is called before the first frame update

    // public List<CaptureView> captureViews;

    public Transform focalPoint; 
    public Transform captureCamera;

    public List<CaptureView> captureViews; 

    public Vector3 comparisonPoint; 

    void Start()
    {
        captureViews = new List<CaptureView>(); 
        comparisonPoint = focalPoint.position + new Vector3(Vector3.Distance(captureCamera.position, focalPoint.position), 0, 0);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public CaptureView [] FindNearestCapture(int k, Vector3 position){

        CaptureView [] o = new CaptureView[k];
        CaptureView leastView = new CaptureView(); 
        float minDist = float.PositiveInfinity;

        foreach (CaptureView view in captureViews){
            float curDist = Vector3.Distance(position, view.capturePosition);

            if (minDist > curDist){
                minDist = curDist;
                leastView = view; 
            }
        }

        o[0] = leastView; 

        return o; //TODO
    }
        



    public struct CaptureView {
        public Texture2D texture; 
        public Matrix4x4 viewProjMatrix; 

        public Vector3 capturePosition;

        Vector3 comparisonPosition;


        public CaptureView(Texture2D tex, Matrix4x4 vpm, Vector3 pos, Vector3 comPos){
            texture = tex;
            viewProjMatrix = vpm;
            capturePosition = pos;
            comparisonPosition = comPos;
        }

        public int Compare(CaptureView first, CaptureView second){
            float distanceFirst = Vector3.Distance(first.capturePosition, comparisonPosition);
            float distanceSecond = Vector3.Distance(second.capturePosition, comparisonPosition);
            if (distanceFirst == distanceSecond){
                return 0;
            }else{
                if(distanceFirst > distanceSecond) return 1;
                return 0;
            }
        }

    }   

}



}

