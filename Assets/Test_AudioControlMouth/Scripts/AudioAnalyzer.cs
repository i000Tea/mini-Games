using UnityEngine;

public class AudioAnalyzer : MonoBehaviour
{
    public GameObject cube;
    public AudioSource audioSource;
    public float analysisInterval;
    public int numSamples = 1024;

    public float minFrequency = 0.00000085f;
    public float maxFrequency = 2f;
    public float minYScale = 1f;
    public float maxYScale = 2f;
    public float smoothTime = 0.1f;

    private float timer = 0f;
    private float targetScale;
    private float currentScale;
    private float velocity;

    void Start()
    {
        currentScale = cube.transform.localScale.y;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= analysisInterval)
        {
            timer = 0f;
            AnalyzeAudio();
            UpdateCubeScale();
        }
    }

    void AnalyzeAudio()
    {
        float[] samples = new float[numSamples];
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        float sumFrequency = 0f;

        for (int i = 0; i < numSamples; i++)
        {
            float frequency = i * (AudioSettings.outputSampleRate / numSamples);
            float amplitude = samples[i];

            sumFrequency += frequency * amplitude;
        }

        float averageFrequency = sumFrequency / numSamples;
        Debug.Log("Æ½¾ùÆµÂÊ: " + averageFrequency);
        targetScale = Remap(averageFrequency, minFrequency, maxFrequency, minYScale, maxYScale);
    }

    void UpdateCubeScale()
    {
        currentScale = Mathf.SmoothDamp(currentScale, targetScale, ref velocity, smoothTime);
        Vector3 newScale = cube.transform.localScale;
        newScale.y = currentScale;
        cube.transform.localScale = newScale;
    }

    float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Lerp(outputMin, outputMax, Mathf.InverseLerp(inputMin, inputMax, value));
    }
}
