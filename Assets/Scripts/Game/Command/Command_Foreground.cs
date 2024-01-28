using System.Text.RegularExpressions;

namespace Game
{
    public class Command_ForegroundFadeIn : Command
    {
        public const string _ID = "fg";
        public float _Duration { get; private set; }

        public override void Do()
        {
            GameSystem._Instance._Foreground.FadeIn(_Duration);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._floatGroup + "?$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Duration = StringDefine.ParseFloat(groups[1].Value);
        }

        public class Command_ForegroundFadeOut : Command
        {
            public const string _ID = "fgout";
            public float _Duration { get; private set; }

            public override void Do()
            {
                GameSystem._Instance._Foreground.FadeOut(_Duration);
            }

            protected override string _ParsePattern
            {
                get
                {
                    return "^" + StringDefine.Pattern._floatGroup + "?$";
                }
            }

            protected override void SetValue(GroupCollection groups, out CommandError error)
            {
                error = null;
                _Duration = StringDefine.ParseFloat(groups[1].Value);
            }
        }

        public class Command_ForegroundCover : Command
        {
            public const string _ID = "fgcover";
            public bool _ToLeft { get; private set; }
            public float _Duration { get; private set; }

            public override void Do()
            {
                GameSystem._Instance._Foreground.Cover(_ToLeft, _Duration);
            }

            protected override string _ParsePattern
            {
                get
                {
                    return "^(left|right)" + "(?: " + StringDefine.Pattern._floatGroup + ")?$";
                }
            }

            protected override void SetValue(GroupCollection groups, out CommandError error)
            {
                error = null;
                _ToLeft = (groups[1].Value == "left");
                _Duration = StringDefine.ParseFloat(groups[2].Value, Define._foregroundCoverDefaultDuration);
            }
        }

        public class Command_ForegroundSweep : Command
        {
            public const string _ID = "fgsweep";
            public bool _ToLeft { get; private set; }
            public float _Duration { get; private set; }

            public override void Do()
            {
                GameSystem._Instance._Foreground.Sweep(_ToLeft, _Duration);
            }

            protected override string _ParsePattern
            {
                get
                {
                    return "^(left|right)" + "(?: " + StringDefine.Pattern._floatGroup + ")?$";
                }
            }

            protected override void SetValue(GroupCollection groups, out CommandError error)
            {
                error = null;
                _ToLeft = (groups[1].Value == "left");
                _Duration = StringDefine.ParseFloat(groups[2].Value, Define._foregroundCoverDefaultDuration);
            }
        }
        // 눈 깜박이는 연출 시 사용
        public class Command_ForegroundCloseEye : Command
        {
            public const string _ID = "fgcloseeye";
            public bool _ToDown { get; private set; } // down인지 up인지 판별, down이면 아래로 up이면 위로 이동하여 화면을 덮는다.
            public float _Duration { get; private set; } // 행동 시작

            public override void Do()
            {
                GameSystem._Instance._Foreground.CloseEye(_ToDown, _Duration);
            }

            protected override string _ParsePattern
            {
                get
                {
                    return "^(up|down)" + "(?: " + StringDefine.Pattern._floatGroup + ")?$";
                }
            }

            protected override void SetValue(GroupCollection groups, out CommandError error)
            {
                error = null;
                _ToDown = (groups[1].Value == "down");
                _Duration = StringDefine.ParseFloat(groups[2].Value, Define._foregroundCoverDefaultDuration);
            }
        }
        // 눈 깜박이는 동안 사용
        public class Command_ForegroundOpenEye : Command// down인지 up인지 판별, down이면 아래로 00면 위로 이동하여 화면을 덮는다.
        {
            public const string _ID = "fgopeneye";
            public bool _ToDown { get; private set; }
            public float _Duration { get; private set; }

            public override void Do()
            {
                GameSystem._Instance._Foreground.OpenEye(_ToDown, _Duration);
            }

            protected override string _ParsePattern
            {
                get
                {
                    return "^(up|down)" + "(?: " + StringDefine.Pattern._floatGroup + ")?$";
                }
            }

            protected override void SetValue(GroupCollection groups, out CommandError error)
            {
                error = null;
                _ToDown = (groups[1].Value == "down");
                _Duration = StringDefine.ParseFloat(groups[2].Value, Define._foregroundCoverDefaultDuration);
            }
        }
    }

}