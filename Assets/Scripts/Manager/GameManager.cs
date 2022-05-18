using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI height;

    [Space(10)]
    [SerializeField] private float mapHeight;

    private Transform player;
    private int currentTime;
    private bool end;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        UpdateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeight();
        UpdateTimeUI();
    }

    private void OnApplicationQuit()
    {
        end = true;
    }

    public void UpdateTimeUI()
    {
        int hour = currentTime / 3600;
        int minute = (currentTime % 3600) / 60;
        int second = (currentTime % 3600) % 60;
        timer.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
    }

    public async void UpdateTimer()
    {
        while (!end)
        {
            await Task.Delay(1000);
            currentTime += 1;
        }
    }

    public void UpdateHeight()
    {
        float y = Mathf.Clamp(player.position.y, 0, mapHeight);
        height.text = $"현재 높이: {Mathf.Round(y * 10f) / 10}m";
    }
}
