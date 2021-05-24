using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;
using TMPro;

public class HololensInference : MonoBehaviour
{
    public TextAsset obj;
    private string loadText;
    private string[] objlist;
    private WebCamTexture webCamTexture;
    public NNModel modelasset;
    public RenderTexture input;
    public TMP_Text output;

    private Model runtimemodel;
    private IWorker worker;

    // Start is called before the first frame update
    void Start(){
        loadText = obj.text;
        objlist = loadText.Split(char.Parse("\n"));
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
        runtimemodel = ModelLoader.Load(modelasset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, runtimemodel);
    }

    // Update is called once per frame
    void Update(){
        Graphics.Blit(webCamTexture, input);
        Tensor inTensor = new Tensor(input);
        Inference(inTensor);
        inTensor.Dispose();
    }
    // 推論
    void Inference(Tensor iTensor){
        worker.Execute(iTensor);
        Tensor outTensor = worker.PeekOutput();
        int target = 0;
        int count = 0;
        var max = -Mathf.Infinity;
        foreach(var item in outTensor.ToReadOnlyArray()){
            if(item > max){
                target = count;
                max = item;
            }
            count++;
        }
        
        outTensor.Dispose();
    }
    void OnDestroy(){
        worker.Dispose();
    }
}
