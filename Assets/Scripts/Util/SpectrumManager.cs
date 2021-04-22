using System.Linq;
using UnityEngine;

public class SpectrumManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cubes;

    [SerializeField]
    private Color[] colors;

    private const int sampleSize = 512;
    private const int freqBandsSize = 8;

    private float[] spectrumSamples = new float[sampleSize];
    private float[] frequencyBands = new float[freqBandsSize];

    private FixedSizeMetalList metalQueue = new FixedSizeMetalList(100);

    public static SpectrumManager Instance { get; private set; }
    public float MetalFactor { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        AudioListener.GetSpectrumData(spectrumSamples, 0, FFTWindow.Blackman);
        MakeFrequentcyBands();

        for (var i = 0; i < frequencyBands.Length; i++)
        {
            cubes[i].transform.localScale = new Vector3(1, (frequencyBands[i]) + 1, 1);
        }

        MetalFactor = frequencyBands.Sum() / freqBandsSize;
        metalQueue.Add(IsFrameMetal());
        Recolor();
    }

    public bool IsMetal()
    {
        return metalQueue.IsMetalList();
    }

    private bool IsFrameMetal()
    {
        return MetalFactor >= 1.75f;
    }

    private void MakeFrequentcyBands()
    {
        var count = 0;

        for(var i = 0; i < freqBandsSize; i++)
        {
            var avg = 0f;
            var sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for(var j = 0; j < sampleCount; j++)
            {
                avg += spectrumSamples[count] * (count + 1);
                count++;
            }

            avg /= count;

            frequencyBands[i] = avg * 10;
        }
    }

    private void Recolor()
    {
        if (IsMetal())
        {
            foreach(var c in cubes)
            {
                var randomColor = colors[Random.Range(0, colors.Length - 1)];
                var renderer = c.GetComponent<Renderer>();

                if (renderer.material.color == Color.white)
                { 
                    renderer.material.color = randomColor;
                    renderer.material.SetColor("_EmissionColor", randomColor);
                }

            }
        }
        else
        {
            foreach (var c in cubes)
            {
                var renderer = c.GetComponent<Renderer>();
                renderer.material.color = Color.white;
                renderer.material.SetColor("_EmissionColor", Color.white);
            }
        }
    }
}
