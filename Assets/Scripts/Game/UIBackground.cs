using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    private Image _bg { get { return gameObject.GetComponent<Image>(); } }

    private void Awake() => InitializeScale();

    private void InitializeScale()
    {
        float camHeight = Screen.height;
        float camWidth = Screen.width;
        float camRatio = camWidth / camHeight;

        float oriWidth = _bg.sprite.texture.width;
        float oriHeight = _bg.sprite.texture.height;
        float oriRatio = oriWidth / oriHeight;

        float scale = 1.0f;

        if (camRatio >= oriRatio)
        {
            // 화면의 가로 비율이 원본 배경 가로 비율보다 큼
            // 예: 아이폰X = 19.5:9, 배경 = 16:9
            // 원본 배경 가로가 화면을 채우도록 원본 스케일 조정
            scale = camWidth / oriWidth;
        }
        else
        {
            // 예: 아이패드 프로 = 4:3, 배경 = 16:9
            // 원본 배경 세로가 기준 세로를 채우도록 원본 스케일 조정
            scale = camHeight / oriHeight;
        }
        float scaledWidth = oriWidth * scale;
        float scaledHeight = oriHeight * scale;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(scaledWidth, scaledHeight);
        gameObject.SetActive(false);
    }
}
