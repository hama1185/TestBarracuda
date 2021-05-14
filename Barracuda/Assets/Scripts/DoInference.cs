using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;

public class DoInference : MonoBehaviour
{

    // Start is called before the first frame update
    public NNModel modelasset;
    public RenderTexture input;
    public RenderTexture output;

    private Model runtimemodel;
    private IWorker worker;


    // Start is called before the first frame update
    void Start(){
        runtimemodel = ModelLoader.Load(modelasset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, runtimemodel);
    }

    // Update is called once per frame
    void Update(){
        Tensor inTensor = new Tensor(input, 3);
        Inference(inTensor);
        inTensor.Dispose();
    }
    // 推論
    void Inference(Tensor iTensor){
        worker.Execute(iTensor);
        Tensor outTensor = worker.PeekOutput();
        outTensor.ToRenderTexture(output, 0, 0, 1/255f, 0, null);
        outTensor.Dispose();
    }
    void OnDestroy(){
        worker.Dispose();
    }
}
