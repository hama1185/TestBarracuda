using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;
//物体認識
public class ObjectCognition : MonoBehaviour
{
    public NNModel modelasset;
    public Texture input;
    public Text output;

    private Model runtimemodel;
    private IWorker worker;

    // Start is called before the first frame update
    void Start(){
        runtimemodel = ModelLoader.Load(modelasset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Compute, runtimemodel);
    }

    // Update is called once per frame
    void Update(){
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
        Debug.Log(target + 1);
        outTensor.Dispose();
    }
    void OnDestroy(){
        worker.Dispose();
    }
}
