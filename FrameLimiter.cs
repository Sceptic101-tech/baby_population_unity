using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Start()
    {
        // Ограничение частоты кадров
        Application.targetFrameRate = targetFPS;

        // Отключение автономного режима (для мобильных устройств)
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
