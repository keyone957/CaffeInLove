using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashAnimate : MonoBehaviour
{
    [SerializeField]
    public bool _finished = false;

    [SerializeField]
    private Image _sprite = null;

    [SerializeField]
    private Sprite[] _frame = null;

    private void Start()
    {
        _finished = false;
        StartCoroutine(Do());
        StartCoroutine(Animate(0.8f));
    }

    public IEnumerator Do()
    {
        yield return StartCoroutine(Fade(true, 2.0f));
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(Fade(false, 1.0f));
        _finished = true;
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Title");
    }

    private IEnumerator Fade(bool isOut, float duration)
    {
        float startTime = Time.time;
        float startAlpha = (isOut ? 0.0f : 1.0f);
        float endAlpha = (isOut ? 1.0f : 0.0f);
        Color color = _sprite.color;

        while (Time.time - startTime - duration < 0)
        {
            float elapsed = (Time.time - startTime);
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            _sprite.color = color;
            yield return null;
        }

        color.a = endAlpha;
        _sprite.color = color;
    }

    private IEnumerator Animate(float duration)
    {
        for (int i = 0; i < 2; i++)
        {
            float startTime = Time.time;
            while (Time.time - startTime - duration < 0)
            {
                float elapsed = (Time.time - startTime);
                float a = Mathf.Lerp(0.0f, _frame.Length - 1, elapsed / duration);
                GetComponent<Image>().sprite = _frame[Mathf.RoundToInt(a)];
                yield return null;
            }
        }
        GetComponent<Image>().sprite = _frame[0];
    }
}