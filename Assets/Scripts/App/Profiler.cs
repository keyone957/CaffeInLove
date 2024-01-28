using UnityEngine;
using UnityEngine.UI;
using Unity.Profiling;

public class Profiler : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    [SerializeField]
    public Text text;
    float time;

    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder drawCallsCountRecorder;

    double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        unsafe
        {
            var samples = stackalloc ProfilerRecorderSample[samplesCount];
            recorder.CopyTo(samples, samplesCount);
            for (var i = 0; i < samplesCount; ++i)
                r += samples[i].Value;
            r /= samplesCount;
        }
        return r;
    }

    public void OnEnable()
    {
        DontDestroyOnLoad(parent);

        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
        drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
    }

    public void OnDisable()
    {
        mainThreadTimeRecorder.Dispose();
        drawCallsCountRecorder.Dispose();
    }

    public void Update()
    {
        time += (Time.unscaledDeltaTime - time) * 0.1f;

        text.text = $"Graphics: <b>{1.0f / time:0.0}</b> FPS (<b>{time * 1000.0f:0.0}</b>ms)\n" +
            $"CPU: main <b>{GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1}</b>ms, Batches: <b>{drawCallsCountRecorder.LastValue}</b>\n" +
            $"Screen: <b>{Screen.width}</b>x<b>{Screen.height}</b>x<b>{Screen.dpi}</b>";
    }
}