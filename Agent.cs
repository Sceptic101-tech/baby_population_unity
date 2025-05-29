using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //public bool is_predator;
    //public float speed;
    //public float timer;
    //public float vision_range;
    //public float field_of_view;
    //public int count_of_rays = 0;
    //public int count_of_outputs = 2;
    //public int count_of_hidden_layers;//Число всех слоев сети
    //public int hidden_layer_size;
    //public float[] inputs;
    //public float[] outputs;//выходы для направления движения
    //public float[][] layers;
    //public float[][] biases;
    //public float[][,] new_weights;
    //public float[] idle_agent_state;
    //public float turning_side;

    //public Vision vision;
    //public NeuralNetwork net;
    //public Rigidbody rb;
    //public GameObject prefabToSpawn; // Префаб объекта, который нужно создавать

    //void Start()
    //{
    ////    vision = GetComponent<Vision>();
    ////    net = GetComponent<NeuralNetwork>();
    ////    rb = GetComponent<Rigidbody>();   

    ////    outputs = new float[count_of_outputs];
    ////    inputs = new float[count_of_rays];


    ////    for (int i = 0; i < count_of_outputs; i++)
    ////        outputs[i] = 0;

    ////    biases = new float[count_of_hidden_layers + 2][];
    ////    layers = new float[count_of_hidden_layers + 2][];
    ////    layers[0] = new float[count_of_rays];
    ////    biases[0] = new float[count_of_rays];
    ////    layers[count_of_hidden_layers + 1] = new float[count_of_outputs];
    ////    biases[count_of_hidden_layers + 1] = new float[count_of_outputs];

    ////    new_weights = new float[count_of_hidden_layers + 1][,];
    ////    new_weights[0] = new float[count_of_rays, hidden_layer_size];
    ////    new_weights[count_of_hidden_layers] = new float[hidden_layer_size, count_of_outputs];

    ////    for (int i = 1; i <= count_of_hidden_layers; i++)
    ////    {
    ////        layers[i] = new float[hidden_layer_size];
    ////        biases[i] = new float[hidden_layer_size];
    ////    }

    ////    //создание весов между скрыми слоями
    ////    for (int i = 1; i < count_of_hidden_layers; i++)
    ////    {
    ////        new_weights[i] = new float[hidden_layer_size, hidden_layer_size];
    ////    }

    ////    net.InitializeNetwork(layers, new_weights, biases, count_of_hidden_layers, count_of_rays, hidden_layer_size, count_of_outputs);
    ////    idle_agent_state = new float[count_of_rays];

    ////    for (int i = 0; i < count_of_rays; i++)
    ////        idle_agent_state[i] = Random.Range(-3f, 3f);
    ////}

    ////void Update()
    ////{
    ////    vision.GetVisionData(this);

    ////    net.ForwardPropagation(layers, new_weights, biases);
    ////    outputs[0] = layers[layers.Length - 1][0];
    ////    outputs[1] = layers[layers.Length - 1][1];

    ////    turning_side = Mathf.Abs(outputs[0]) > Mathf.Abs(outputs[1]) ? -Mathf.Abs(outputs[0]) : Mathf.Abs(outputs[1]);
    ////    //turning_side = outputs[0] + outputs[1];

    ////    Vector3 dir = Quaternion.Euler(0, turning_side, 0) * transform.forward; //Направление движения
    ////    transform.Translate(dir * speed * Time.deltaTime, Space.World);
    ////    //rb.AddForce(dir * speed);
    ////    turning_side = 0;
    ////    net.MakeNeuronsZero(layers);
    ////}

    ////void FixedUpdate()
    ////{

    ////}

    ////public void Reproduce()
    ////{
    ////    GameObject newObj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    ////    net.Mutate(new_weights, count_of_hidden_layers);
    ////}

}
