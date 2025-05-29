using System.Threading;
using UnityEngine;

public class Prey : MonoBehaviour
{
    public bool is_predator;
    public float speed;
    public float timer;
    public float time_of_life;
    public float timer_of_life;
    public float vision_range;
    public float turning_side_mult;
    public float field_of_view;
    public int count_of_rays;
    public int count_of_outputs;
    public int count_of_hidden_layers;
    public int hidden_layer_size;
    public float[] inputs;
    public float[] outputs;
    public float[][] layers;
    public float[][] biases;
    public float[][,] weights;
    public float turning_side;
    public int mutation_chance_percantage;

    public float time_to_reproduce;

    public Vision vision;
    public NeuralNetwork net;
    public GameObject prefabToSpawn;
    private Rigidbody rb;

    private float[][,] CopyWeights(float[][,] source)
    {
        float[][,] copy = new float[source.Length][,];
        for (int i = 0; i < source.Length; i++)
        {
            int dim0 = source[i].GetLength(0);
            int dim1 = source[i].GetLength(1);
            copy[i] = new float[dim0, dim1];
            
            for (int x = 0; x < dim0; x++)
            {
                for (int y = 0; y < dim1; y++)
                {
                    copy[i][x, y] = source[i][x, y];
                }
            }
        }
        return copy;
    }

    private float[][] CopyBiases(float[][] source)
    {
        float[][] copy = new float[source.Length][];
        for (int i = 0; i < source.Length; i++)
        {
            copy[i] = (float[])source[i].Clone();
        }
        return copy;
    }

    public void CopyNetwork(Prey source)
    {
        // Проверка исходных данных
        if (source.weights == null || source.biases == null)
        {
            Debug.LogError("Source network is not initialized!");
            return;
        }

        // Глубокое копирование
        weights = CopyWeights(source.weights);
        biases = CopyBiases(source.biases);
        
        // Копирование параметров
        count_of_hidden_layers = source.count_of_hidden_layers;
        hidden_layer_size = source.hidden_layer_size;
        count_of_rays = source.count_of_rays;
    }

    public void Start()
    {
        if (weights != null && biases != null) return;

        rb = GetComponent<Rigidbody>();
        vision = GetComponent<Vision>();
        net = GetComponent<NeuralNetwork>();

        outputs = new float[count_of_outputs];
        inputs = new float[3*count_of_rays];

        biases = new float[count_of_hidden_layers + 2][];
        layers = new float[count_of_hidden_layers + 2][];
        layers[0] = new float[3*count_of_rays];
        biases[0] = new float[3*count_of_rays];
        layers[count_of_hidden_layers + 1] = new float[count_of_outputs];
        biases[count_of_hidden_layers + 1] = new float[count_of_outputs];

        weights = new float[count_of_hidden_layers + 1][,];
        weights[0] = new float[3*count_of_rays, hidden_layer_size];
        weights[count_of_hidden_layers] = new float[hidden_layer_size, count_of_outputs];

        for (int i = 1; i <= count_of_hidden_layers; i++)
        {
            layers[i] = new float[hidden_layer_size];
            biases[i] = new float[hidden_layer_size];
        }

        for (int i = 1; i < count_of_hidden_layers; i++)
        {
            weights[i] = new float[hidden_layer_size, hidden_layer_size];
        }

        net.InitializeNetwork(layers, weights, biases, count_of_hidden_layers);
    }

    void Update()
    {
        vision.GetVisionData(layers, count_of_rays, field_of_view, vision_range, is_predator);

        net.ForwardPropagation(layers, weights, biases);
        outputs[0] = layers[layers.Length - 1][0];//Поворот
        outputs[1] = layers[layers.Length - 1][1];//Поворот
        outputs[2] = layers[layers.Length - 1][2];//Моментальное ускорение
        outputs[2] = Mathf.Clamp(outputs[2], -2f, 2f);

        turning_side = turning_side_mult*(outputs[0] + outputs[1]);

        turning_side = Mathf.Clamp(turning_side, -45f, 45f);
        transform.Rotate(0, turning_side, 0);
        Vector3 movement = transform.forward * (speed + outputs[2]) * Time.deltaTime;
        //Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        turning_side = 0;

        timer += Time.deltaTime;

        if (timer > time_of_life)
        {
            timer = 0;
            Destroy(gameObject);
        }

        if(timer >= time_to_reproduce)
        {
            timer = 0;
            Reproduce();
        }
    }

    public void Reproduce()
    {
        GameObject newObj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        Prey offspring = newObj.GetComponent<Prey>();
        
        // Принудительно задаем параметры перед копированием
        offspring.count_of_rays = this.count_of_rays;
        offspring.hidden_layer_size = this.hidden_layer_size;
        offspring.count_of_hidden_layers = this.count_of_hidden_layers;
        
        // Инициализируем сеть потомка
        offspring.Start();
        
        // Копируем и мутируем
        offspring.CopyNetwork(this);
        offspring.net.Mutate(offspring.weights, offspring.count_of_hidden_layers, 
                        offspring.biases, mutation_chance_percantage);

        //mutating agents params
        if(mutation_chance_percantage <= Random.Range(0, 101))
        {
            //speed += Random.Range(-1.0f, 1.0f);
            vision_range += Random.Range(-1.0f, 1.0f);
            //field_of_view += Random.Range(-5, 5);
            //turning_side_mult += Random.Range(-1.0f, 1.0f);
            //time_to_reproduce += Random.Range(-0.5f, 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "predator")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "death")
        {
            Destroy(gameObject);
        }
    }

}
