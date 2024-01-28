using UnityEngine;
using System.Text.RegularExpressions;

namespace Game
{
    public class Command_HideMenu : Command
    {
        public const string _ID = "hidemenu";

        public override void Do() => GameSystem._Instance._UI._Menu.SetActivate(false);

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

    public class Command_ShowMenu : Command
    {
        public const string _ID = "showmenu";

        public override void Do() => GameSystem._Instance._UI._Menu.SetActivate(true);

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

    public class Command_ShowLoading : Command
    {
        public const string _ID = "showloading";
        public const string _Name = "loading"; // 로딩 UI 구분자

        public override void Do() => GameSystem._Instance._UI.AddDynamicUI(_Name, Define._uiLoadingPrefabPath);

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

    public class Command_RemoveLoading : Command
    {
        public const string _ID = "removeloading";
        public const string _Name = Command_ShowLoading._Name;

        public override void Do() => GameSystem._Instance._UI.RemoveDynamicUI(_Name);

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