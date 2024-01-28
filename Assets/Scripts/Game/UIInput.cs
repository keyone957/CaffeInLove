using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Game
{
    public class UIInput : UIWindow, IPointerDownHandler
    {
        private bool _isClicked = false;
        //++ 추가본
        private bool _isCtrButtonDown = false;

        private bool _isAutomode = false;

        public void OnPointerDown(PointerEventData eventData) => _isClicked = true;

        public bool PopIsClicked()
        {
            //게임 신일때
            if (SceneManager.GetActiveScene().name == "Game")
            {   //환경 설정의 출력 모드가 Auto일때
                if (PlayerPrefs.GetString("automode") == "Auto")
                {   //메뉴패널이 active true일때 자동모드 실행 x
                    if (GameSystem._Instance._UI._menuPanel.IsActivate())
                    {
                        return false;
                    }
                    //팝업 UI active true일때 자동모드 실행 x
                    else if (GameSystem._Instance._UI._popUp.IsActivate())
                    {
                        return false;
                    }
                    // 게임 메뉴를 열었을때 자동실행 x
                    else if (GameSystem._Instance._UI._Menu.gameObject.transform.Find("Container").gameObject.activeSelf)
                    {
                        return false;
                    }
                    //Invoke를 사용하여 Define._autoModeDelay를 설정해주어 일정 딜레이가 끝나면 실행 되게
                    else if (_isAutomode)
                    {
                        bool isClicked = _isClicked;
                        _isClicked = false;
                        if (_isCtrButtonDown)
                        {
                            return true;
                        }
                        return isClicked;
                    }
                    else if (!_isAutomode)
                    {
                        _isClicked = false;
                        _isAutomode = true;
                        Invoke("ResetCtrButtonDown", Define._autoModeDelay);
                        return true;
                    }
                    // 자동모드중에도 빠른 스킵이 가능하게 하기위함
                    else if (_isCtrButtonDown)
                    {
                        return true;
                    }
                }
                //수동모드일때는 원래 코드대로 실행
                else
                {   //백그라운드 이미지가 active가 true일때 클릭 못하게 
                    if (GameSystem._Instance._UI._UIBack.activeSelf)
                    {
                        return false;
                    }
                    else if (_isCtrButtonDown)
                    {
                        return true;
                    }
                    else
                    {
                        bool isClicked = _isClicked;
                        _isClicked = false;
                        return isClicked;
                    }
                }
            }
            // 카페 미니게임일때는 자동모드를 할필요가 없음
            else if (SceneManager.GetActiveScene().name == "cafe")
            {
                bool isClicked = _isClicked;
                _isClicked = false;
                return isClicked;
            }
            return false;
        }

        private void ResetCtrButtonDown()
        {
            _isAutomode = false;
        }
      
        // ++ 추가본
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // 좌측 컨트롤로 스킵
                _isCtrButtonDown = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                // 좌측 컨트롤을 떼면 스킵 종료
                _isCtrButtonDown = false;
            }
           
        }
    }
}