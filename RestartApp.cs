using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class RestartApp : MonoBehaviour
{
    [SerializeField] private string targetTag = "predator";
    private float timer = 0;
    [SerializeField] private float check_delay = 8;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= check_delay)
        {
            timer = 0;
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
            if (taggedObjects.Length == 0)
            {
                RestartScene();
            }
        }
    }

    void RestartScene()
    {
        // Выберите один из вариантов:
        
        // Вариант 1: Перезагрузка текущей сцены (работает везде)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Вариант 2: Полный рестарт
        StartCoroutine(FullRestart());
    }

    IEnumerator FullRestart()
    {
        #if UNITY_EDITOR
        // Для редактора Unity
        UnityEditor.EditorApplication.isPlaying = false;
        yield break;
        #else
        // Для билда
        //string appPath = System.IO.Path.Combine(Application.dataPath, "../evolution_genetic_alg.exe");
        Process.Start("E:/Projects/unity projects/my_evolve/version_0_1/evolution_genetic_alg.exe");
        yield return new WaitForSeconds(1f);
        Application.Quit();
        #endif
    }
}

