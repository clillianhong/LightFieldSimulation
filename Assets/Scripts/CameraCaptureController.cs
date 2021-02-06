using UnityEngine;
using System.IO;

namespace Simulation {
public class CameraCaptureController : MonoBehaviour
{
    public int fileCounter;
    public KeyCode screenshotKey;

    public int captureWidth;
    public int captureHeight;

    CaptureViewCollection collectionManager;
    private Camera _camera;
 
    private void Start(){
        collectionManager = GameObject.Find("OrbitViewManager").GetComponent<CaptureViewCollection>();
        _camera = gameObject.GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            CaptureAndDisplay();
            
        }
    }

 
    public void CaptureAndDisplay()
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
        createCapturePoseLabel(cameraTransform, image);
        Debug.Log("list len " + collectionManager.captureViews.Count);
    }

    public void createCapturePoseLabel(Transform trans, Texture2D tex){
        CaptureCreator.CreateCaptureGameObject(trans, tex, captureWidth, captureHeight, "capture-" +fileCounter);
        fileCounter++;
    }



     /// <summary>
    /// Save texture as a PNG within assets/TestOutputImages/
    /// </summary>
    /// 
    public void SaveToFile(Texture2D image){
        
        byte[] bytes = image.EncodeToJPG();
        Destroy(image);
 
        Debug.Log("Capturing Image");
        File.WriteAllBytes(Application.dataPath + "/TestOutputImages/" + fileCounter + ".png", bytes);
        fileCounter++;
    }

    
}
}
