using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;

public class DoInference : MonoBehaviour
{
    public TextAsset obj;
    private string loadText;
    private string[] objlist;

    int mode = 0;
    public NNModel objectasset;
    public NNModel styleasset;
    public RenderTexture input;
    public RenderTexture output;

    private Model runtimemodel;
    private IWorker worker;

    public Text TextOut;


    // Start is called before the first frame update
    void Start(){
        runtimemodel = ModelLoader.Load(styleasset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, runtimemodel);
        loadText = obj.text;
        objlist = loadText.Split(char.Parse("\n"));
    }

    // Update is called once per frame
    void Update(){
        Tensor inTensor = new Tensor(input, 3);
        if(mode == 0){
            InferenceStyle(inTensor);
        }
        else{
            InferenceObject(inTensor);
        }

        inTensor.Dispose();
    }
    // 推論
    void InferenceStyle(Tensor iTensor){
        worker.Execute(iTensor);
        Tensor outTensor = worker.PeekOutput();
        outTensor.ToRenderTexture(output, 0, 0, 1/255f, 0, null);
        outTensor.Dispose();
    }
    void InferenceObject(Tensor iTensor){
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
        TextOut.text = objlist[target];
        outTensor.Dispose();
    }

    void OnDestroy(){
        worker.Dispose();
    }

    public void ChangeModel(){
        if(mode == 0){
            runtimemodel = ModelLoader.Load(objectasset);
            mode = 1;
        }
        else{
            runtimemodel = ModelLoader.Load(styleasset);
            mode = 0;
        }
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, runtimemodel);
    }
}
