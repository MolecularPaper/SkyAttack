using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Height : MonoBehaviour
{
    [SerializeField] private float mapHeight;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform point;
    
    private Transform player;
    public float currentHeight;
    private float barHeight;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        barHeight = bar.rect.height - point.rect.height / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeight();
    }

    public void UpdateHeight()
    {
        currentHeight = Mathf.Clamp(player.position.y, 0, float.MaxValue);

        float y = Mathf.Lerp(0, barHeight, currentHeight/mapHeight);
        point.anchoredPosition = new Vector2(0, y);
    }
}
