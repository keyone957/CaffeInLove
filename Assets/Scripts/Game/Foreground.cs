using UnityEngine;
using System.Collections;

namespace Game
{
    public class Foreground : MonoBehaviour
    {
        private GameObject _go;
        protected GameObject _Go { get { if (_go == null) { _go = gameObject; } return _go; } }
        private Transform _trans;
        public Transform _Trans { get { if (_trans == null) { _trans = transform; } return _trans; } }
        [SerializeField]
        private GameObject _main = null;
        [SerializeField]
        private GameObject _gradationLeft = null;
        [SerializeField]
        private GameObject _gradationRight = null;
        [SerializeField]
        private GameObject _gradationDown = null;
        [SerializeField]
        private GameObject _gradationUp = null;
        private Renderer _mainRenderer;

        public void Initialize()
        {
            // 메인 판이 카메라를 덮도록
            float camHeight = Camera.main.orthographicSize * 2.0f;
            float camWidth = camHeight * Camera.main.aspect;
            _main.transform.localScale = new Vector3(camWidth, camHeight, 1.0f);

            float gradationLeftWidth = _gradationLeft.transform.localScale.x;
            _gradationLeft.transform.localScale = new Vector3(gradationLeftWidth, camHeight, 1.0f);
            _gradationLeft.transform.localPosition = new Vector3(-camWidth / 2.0f - gradationLeftWidth / 2.0f, 0.0f, 0.0f);

            float gradationRightWidth = _gradationRight.transform.localScale.x;
            _gradationRight.transform.localScale = new Vector3(gradationRightWidth, camHeight, 1.0f);
            _gradationRight.transform.localPosition = new Vector3(camWidth / 2.0f + gradationRightWidth / 2.0f, 0.0f, 0.0f);

            _gradationDown.transform.localScale = new Vector3(camWidth*1.05f, camHeight/2.0f, 1.0f);
            _gradationDown.SetActive(false);
            _gradationUp.transform.localScale = new Vector3(camWidth * 1.05f, camHeight / 2.0f, 1.0f);
            _gradationUp.SetActive(false);
            _mainRenderer = _main.GetComponent<Renderer>();
        }

        public void SetActivate(bool activate) => _Go.SetActive(activate);

        public void SetAlpha(float alpha)
        {
            const string tintName = "_Color";
            Color color = _mainRenderer.material.GetColor(tintName);
            color.a = alpha;
            _mainRenderer.material.SetColor(tintName, color);
        }

        public float GetMainWidth() => _main.transform.localScale.x;
        public float GetMainHeight() => _main.transform.localScale.y;

        public float GetGradationLeftWidth() => _gradationLeft.transform.localScale.x;

        public float GetGradationRightWidth() => _gradationRight.transform.localScale.x;
        public float GetGradationDownHeight() => _gradationDown.transform.localScale.y;
        public float GetGradationUpHeight() => _gradationUp.transform.localScale.y;

        // 불투명해지기
        public void FadeIn(float duration)
        {
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(true);
                SetAlpha(1.0f);
            }
            else
                GameSystem._Instance.RegisterTask(FadeInTask(duration));
        }

        private IEnumerator FadeInTask(float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                SetAlpha(Mathf.Lerp(0.0f, 1.0f, rate));
                yield return null;
            }
            SetAlpha(1.0f);
        }

        // 투명해지기
        public void FadeOut(float duration)
        {
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(false);
            }
            else
                GameSystem._Instance.RegisterTask(FadeOutTask(duration));
        }

        private IEnumerator FadeOutTask(float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                SetAlpha(Mathf.Lerp(1.0f, 0.0f, rate));
                yield return null;
            }
            SetAlpha(1.0f);
            SetActivate(false);
        }

        // 움직이며 가리기
        public void Cover(bool toLeft, float duration)
        {
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(true);
            }
            else
                GameSystem._Instance.RegisterTask(CoverTask(toLeft, duration));
        }

        private IEnumerator CoverTask(bool toLeft, float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);

            float startX = 0.0f;
            if (toLeft)
                startX = GetGradationRightWidth() + GetMainWidth();
            else
                startX = -GetGradationLeftWidth() - GetMainWidth();
            float z = _main.transform.position.z;

            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                float posX = Mathf.Lerp(startX, 0.0f, rate);
                _Trans.position = new Vector3(posX, 0.0f, z);
                yield return null;
            }
            _Trans.position = new Vector3(0.0f, 0.0f, z);
        }

        // 움직이며 지우기
        public void Sweep(bool toLeft, float duration)
        {
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(false);
            }
            else
                GameSystem._Instance.RegisterTask(SweepTask(toLeft, duration));
        }

        private IEnumerator SweepTask(bool toLeft, float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);

            float endX = 0.0f;
            if (toLeft)
                endX = -GetGradationLeftWidth() - GetMainWidth();
            else
                endX = GetGradationRightWidth() + GetMainWidth();
            float z = _main.transform.position.z;

            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                float posX = Mathf.Lerp(0.0f, endX, rate);
                _Trans.position = new Vector3(posX, 0.0f, z);
                yield return null;
            }
            _Trans.position = new Vector3(0.0f, 0.0f, z);
            SetActivate(false);
            _gradationDown.SetActive(false);
        }
        // 눈 감듯이 화면 검게 down이면 아래로, up이면 위로, duration 시간동안 진행
        public void CloseEye(bool toDown, float duration)
        {
            _gradationDown.SetActive(true);
            _gradationUp.SetActive(true);
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(true);
            }
            else
                GameSystem._Instance.RegisterTask(CloseEyeTask(toDown, duration));
        }

        private IEnumerator CloseEyeTask(bool toDown, float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);

            float startY = 0.0f;
            if (toDown)
                startY = GetGradationDownHeight() + GetMainHeight();
            else
                startY = -GetGradationUpHeight() - GetMainHeight();
            float z = _main.transform.position.z;
            // duration 시간동안 방향으로 위아래로 이동
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                float posY = Mathf.Lerp(startY, 0.0f, rate);
                _Trans.position = new Vector3(0.0f, posY, z);
                yield return null;
            }
            _Trans.position = new Vector3(0.0f, 0.0f, z);
            _gradationDown.SetActive(false);
            _gradationUp.SetActive(false);
        }

        // 눈을 뜨는 듯한 연출 up이면 위로, down이면 아래로 이동하면서 화면 보이게 duration동안 이동
        public void OpenEye(bool toUp, float duration)
        {
            _gradationDown.SetActive(true);
            _gradationUp.SetActive(true);
            if (duration == 0.0f)
            {
                GameSystem._Instance._UI._Dialogue.SetActivate(false);
                SetActivate(true);
            }
            else
                GameSystem._Instance.RegisterTask(OpenEyeTask(toUp, duration));
        }

        private IEnumerator OpenEyeTask(bool toUp, float duration)
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            SetActivate(true);

            float startY = 0.0f;
            if (toUp)
                startY = -GetGradationDownHeight() - GetMainHeight();
            else
                startY = GetGradationUpHeight() + GetMainHeight();
            float z = _main.transform.position.z;
            // duration 시간동안 방향으로 위아래로 이동, 
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                float posY = Mathf.Lerp(0.0f, startY, rate);
                _Trans.position = new Vector3(0.0f, posY, z);
                yield return null;
            }
            _Trans.position = new Vector3(0.0f, 0.0f, z);
            SetActivate(false);
            _gradationDown.SetActive(false);
            _gradationUp.SetActive(false);
        }
    }
}