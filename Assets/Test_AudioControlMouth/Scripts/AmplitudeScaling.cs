using UnityEngine;

public class AmplitudeScaling : MonoBehaviour
{
    public GameObject cube;
    public AudioSource audioSource;
    public float analysisInterval;
    public int numSamples = 1024;

    public float minAmplitude = 0.00000085f;
    public float maxAmplitude = 2f;
    public float middleAmplitude = 1f;
    public float minScaleX = 3f;
    public float maxScaleX = 1.5f;
    public float smoothTime = 0.1f;

    private float timer = 0f;
    private float targetScale;
    private float currentScale;
    private float velocity;

    private void Start()
    {
        currentScale = cube.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= analysisInterval)
        {
            timer = 0f;
            AnalyzeAudio();
            UpdateCubeScale();
        }
    }

    private void AnalyzeAudio()
    {
        float[] samples = new float[numSamples];
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Rectangular);

        float sumAmplitude = 0f;

        for (int i = 0; i < numSamples; i++)
        {
            float amplitude = samples[i];
            sumAmplitude += amplitude;
        }

        float averageAmplitude = sumAmplitude / numSamples;
        if (averageAmplitude < minAmplitude || averageAmplitude > maxAmplitude)
        {

        }
        else
        {
            Debug.Log("Æ½¾ùÕñ·ù: " + averageAmplitude);
        }

        if (averageAmplitude < minAmplitude || averageAmplitude > maxAmplitude)
        {
            targetScale = minScaleX;
        }
        else
        {
            targetScale = maxScaleX;
        }
    }

    private void UpdateCubeScale()
    {
        currentScale = Mathf.SmoothDamp(currentScale, targetScale, ref velocity, smoothTime);
        Vector3 newScale = cube.transform.localScale;
        newScale.x = currentScale;
        cube.transform.localScale = newScale;
    }
}
