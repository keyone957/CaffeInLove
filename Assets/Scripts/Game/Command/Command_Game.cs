using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

namespace Game
{
    /// <summary>
    /// 대기
    /// 시간 0 또는 미지정 시 클릭 대기
    /// </summary>
    public class Command_Wait : Command
    {
        public const string _ID = "wait";
        public float _Duration { get; private set; }

        public override void Do()
        {
            if (Mathf.Approximately(_Duration, 0.0f))
                GameSystem._Instance.WaitClick();
            else
                GameSystem._Instance.Wait(_Duration);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^(?:" + StringDefine.Pattern._floatGroup + ")?$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Duration = StringDefine.ParseFloat(groups[1].Value);
        }
    }

    public class Command_Label : Command
    {
        public const string _ID = "label";
        public string _Name { get; private set; }

        public override void Do()
        {
            // do nothing
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
        }

        public override void OnParsed(out CommandError error)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                bool result = GameSystem._Instance._ScenarioManager.AddLabelOnParsed(_Name);
                if (!result)
                {
                    error = new CommandError("Cannot add label:" + _Name);
                    return;
                }
            }
            else if (SceneManager.GetActiveScene().name == "Cafe")

            {
                bool result = CafeSystem._Instance._ScenarioManager.AddLabelOnParsed(_Name);
                if (!result)
                {
                    error = new CommandError("Cannot add label:" + _Name);
                    return;
                }
            }    
            error = null;
        }
    }

    public class Command_Jump : Command
    {
        public const string _ID = "jump";
        public string _LabelName { get; private set; }

        public override void Do()
        {
            GameSystem._Instance._ScenarioManager.JumpToLabel(_LabelName);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _LabelName = groups[1].Value;
        }
    }

    public class Command_Select : Command
    {
        public const string _ID = "select";

        public override void Do()
        {
            GameSystem._Instance._UI.CreateSelect();
        }

        protected override string _ParsePattern
        {
            get
            {
                return StringDefine.Pattern._emptyString;
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
        }
    }

    public class Command_SelectItem : Command
    {
        public const string _ID = "selectitem";
        public string _LabelName { get; private set; }
        public string _Text { get; private set; }

        public override void Do()
        {
            UISelect select = GameSystem._Instance._UI._Select;
            if (select == null)
            {
                Debug.LogError($"[Command_SelectItem.NotExistSelectUI]");
                return;
            }

            select.AddItem(_Text, _LabelName);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._stringGroup
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _LabelName = groups[1].Value;
            _Text = groups[2].Value;
        }
    }

    public class Command_SelectEnd : Command
    {
        public const string _ID = "selectend";

        public override void Do()
        {
            GameSystem._Instance.RegisterTask(SelectWaitTask());
        }

        private IEnumerator SelectWaitTask()
        {
            while (!GameSystem._Instance._UI._Select._SelectEnded)
                yield return null;

            GameSystem._Instance._ScenarioManager.JumpToLabel(GameSystem._Instance._UI._Select._SelectedLabelName);
            GameSystem._Instance._UI.RemoveSelect();
        }

        protected override string _ParsePattern
        {
            get
            {
                return StringDefine.Pattern._emptyString;
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
        }
    }

    public class Command_Title : Command
    {
        public const string _ID = "title";

        public override void Do()
        {
            GameSystem._Instance.RegisterTask(LoadTitleSceneTask());
        }

        private IEnumerator LoadTitleSceneTask()
        {
            GameSystem._Instance.LoadTitleScene();
            while (true)
                yield return null;
        }

        protected override string _ParsePattern
        {
            get
            {
                return StringDefine.Pattern._emptyString;
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
        }
    }

    public class Command_Scenario : Command
    {
        public const string _ID = "scenario";
        public string _ScenarioName { get; private set; }
        public string _LabelName { get; private set; }

        public override void Do()
        {
            GameSystem._Instance._ScenarioManager.Load(_ScenarioName);
            if (!string.IsNullOrEmpty(_LabelName))
                GameSystem._Instance._ScenarioManager.JumpToLabel(_LabelName);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + "(?: " + StringDefine.Pattern._wordGroup + ")?"
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _ScenarioName = groups[1].Value;
            _LabelName = groups[2].Value;
        }
    }
    public class Command_ZoomIn : Command
    {
        public const string _ID = "zoomin";
        public float _Ratio { get; private set; } // 배율
        public float _Duration { get; private set; } //시간
        public float _OriSize { get; private set; } // 원래 비율
        public override void Do()
        {
            // 현재 비율을 저장후 확대
            _OriSize = Camera.main.orthographicSize;
            GameSystem._Instance.RegisterTask(ZoomInTask(_Duration));
        }
        private IEnumerator ZoomInTask(float duration)
        {
            // duration동안 목표배율로 점차적으로 확대
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                Camera.main.orthographicSize = _OriSize * (1 / (Mathf.Lerp(_OriSize, _Ratio, rate)));
                yield return null;
            }
            Camera.main.orthographicSize = _OriSize * (1 / _Ratio);
        }
        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._floatGroup
                    + " " + StringDefine.Pattern._floatGroup +
                    "?$";
            }
        }
        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Ratio = StringDefine.ParseFloat(groups[1].Value);
            _Duration = StringDefine.ParseFloat(groups[2].Value);
        }
    }
    public class Command_ZoomOut : Command
    {
        public const string _ID = "zoomout";
        public float _Ratio { get; private set; } // 배율
        public float _Duration { get; private set; } // 시간
        public float _OriSize { get; private set; } // 원래 비율
        public override void Do()
        {
            // 원래 비율 저장
            _OriSize = Camera.main.orthographicSize;
            GameSystem._Instance.RegisterTask(ZoomOutTask(_Duration));
        }
        private IEnumerator ZoomOutTask(float duration)
        {
            // 원래 비율을 기준으로 해서 목표배율까지 점차 축소
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
            {
                float rate = (Time.time - startTime) / duration;
                Camera.main.orthographicSize = (Mathf.Lerp(_OriSize, _OriSize * _Ratio, rate));
                yield return null;
            }
            Camera.main.orthographicSize = _OriSize * _Ratio;
        }
        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._floatGroup
                    + " " + StringDefine.Pattern._floatGroup +
                    "?$";
            }
        }
        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Ratio = StringDefine.ParseFloat(groups[1].Value);
            _Duration = StringDefine.ParseFloat(groups[2].Value);
        }
    }
    public class Command_Cafe : Command
    {
        public const string _ID = "cafe";
        public override void Do()
        {
            // 카페 씬으로 이동
            //회상중이면 cafe명렁어 무시하고 타이틀로 이동
            if (PlayerPrefs.GetString("ONReview") == "True")
            {
                PlayerPrefs.SetString("ONReview" ,"False");
                GameSystem._Instance.LoadTitleScene();
                return;
            }
            else { GameSystem._Instance.LoadDoCafeSequence(); }
            
        }
        protected override string _ParsePattern
        {
            get
            {
                return StringDefine.Pattern._emptyString;
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
        }
    }

}