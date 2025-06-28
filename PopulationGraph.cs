using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopulationGraph : MonoBehaviour
{
    [Header("Settings")]
    public int maxDataPoints = 100;
    public float updateInterval = 2f;
    public float lineWidth = 2f;

    [Header("UI References")]
    public RectTransform graphContainer;
    public Color preyColor = Color.green;
    public Color predatorColor = Color.red;

    private List<float> preyData = new List<float>();
    private List<float> predatorData = new List<float>();
    private float timer;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = graphContainer.GetComponent<RectTransform>();
        InitializeGraph();
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateData();
            RedrawGraph();
        }
    }

    void InitializeGraph()
    {
        preyData.Clear();
        predatorData.Clear();
        for (int i = 0; i < maxDataPoints; i++)
        {
            preyData.Add(0);
            predatorData.Add(0);
        }
    }

    void UpdateData()
    {
        int currentPrey = GameObject.FindGameObjectsWithTag("prey").Length;
        int currentPredator = GameObject.FindGameObjectsWithTag("predator").Length;

        preyData.Add(currentPrey);
        predatorData.Add(currentPredator);

        if (preyData.Count > maxDataPoints) preyData.RemoveAt(0);
        if (predatorData.Count > maxDataPoints) predatorData.RemoveAt(0);
    }

    void RedrawGraph()
    {
        foreach (Transform child in graphContainer)
        {
            Destroy(child.gameObject);
        }

        float maxValue = Mathf.Max(
            Mathf.Max(preyData.ToArray()),
            Mathf.Max(predatorData.ToArray())
        );

        if (maxValue == 0) maxValue = 1;

        DrawLine(preyData, preyColor, maxValue);
        DrawLine(predatorData, predatorColor, maxValue);
    }

    void DrawLine(List<float> data, Color color, float maxValue)
    {
        GameObject lineGO = new GameObject("Line");
        lineGO.transform.SetParent(graphContainer, false);
        
        // Добавляем необходимые компоненты
        var meshFilter = lineGO.AddComponent<MeshFilter>();
        var meshRenderer = lineGO.AddComponent<MeshRenderer>();
        var canvasRenderer = lineGO.AddComponent<CanvasRenderer>();

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> indices = new List<int>();

        float containerWidth = rectTransform.rect.width;
        float containerHeight = rectTransform.rect.height;

        for (int i = 0; i < data.Count; i++)
        {
            float xPos = (float)i / (data.Count - 1) * containerWidth;
            float yPos = (data[i] / maxValue) * containerHeight;

            // Создаем вершины для линии
            Vector3 left = new Vector3(xPos - lineWidth/2, yPos, 0);
            Vector3 right = new Vector3(xPos + lineWidth/2, yPos, 0);

            vertices.Add(left);
            vertices.Add(right);

            colors.Add(color);
            colors.Add(color);

            if (i > 0)
            {
                int start = i * 2 - 2;
                indices.Add(start);
                indices.Add(start + 1);
                indices.Add(start + 2);
                indices.Add(start + 1);
                indices.Add(start + 3);
                indices.Add(start + 2);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
        meshFilter.mesh = mesh;

        // Настройка материала
        meshRenderer.material = new Material(Shader.Find("UI/Default"));
    }
}