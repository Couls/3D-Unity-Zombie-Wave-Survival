using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private ParticleSystem particleSys;
    public float fillSpeed = 5f;
    private float targetProgress = 0f;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        particleSys = GetComponentInChildren<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        if (slider.value < targetProgress && slider.value < slider.maxValue)
        {
            slider.value += fillSpeed * Time.deltaTime;
            if (!particleSys.isPlaying)
                particleSys.Play();
        }
        else
        {
            particleSys.Stop();
        }
    }
    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
    public void SetProgress(float newProgress)
    {
        targetProgress = newProgress;
    }
}
