using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour
{
    public RawImage screen;

    WebCamTexture webCamTexture;
    WebCamDevice[] webCamDevices;
    int selectCamera = 0;

    // Start is called before the first frame update
    void Start(){
        webCamTexture = new WebCamTexture();
        webCamDevices = WebCamTexture.devices;
        screen.texture = webCamTexture;
        webCamTexture.Play();
    }

    public void ChangeWebCamera(){
        int cameras = webCamDevices.Length;
        Debug.Log(cameras);
        if(cameras < 1){
            return;
        }

        selectCamera++;
        //本来は減算しないが謎のカメラを検出するため-1する
        if(selectCamera >= cameras - 1){
            selectCamera = 0;
        }

        webCamTexture.Stop();
        webCamTexture = new WebCamTexture(webCamDevices[selectCamera].name);
        screen.texture = webCamTexture;
        webCamTexture.Play();
    }
}
