using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : MonoBehaviour
{
    public int currentTime;

    private TextMeshProUGUI timer;
    private bool end;

    void OnApplicationQuit()
    {
        end = true;
    }

    void Awake()
    {
        timer = GetComponent<TextMeshProUGUI>();

        UpdateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeUI();
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
}
