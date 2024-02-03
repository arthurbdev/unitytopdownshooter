using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraBackground : MonoBehaviour
{
    private Camera maincam;
    [SerializeField] private float hue = 0.8f;

    private void Awake()
    {
        maincam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hue > 1)
        {
            hue = 0;
        }

        hue += Time.deltaTime * 0.05f;
        hue = hue % 1f;
        maincam.backgroundColor = Color.HSVToRGB(hue, 0.4f, 0.4f, false);
    }
}
