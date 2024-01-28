using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Game
{
    public class Command_Sprite : Command
    {
        public const string _ID = "spr";
        public string _Name { get; private set; }
        public Vector2 _Position { get; private set; }
        public float _Scale { get; private set; }

        public override void Do()
        {
            Sprite spr = GameSystem._Instance._SpriteManager.GetOrCreate(_Name);
            if (spr == null)
            {
                Debug.LogError("[Command_Sprite.Do.InvalidName]");
                return;
            }

            spr.SetPosition(_Position);
            spr.SetScale(_Scale);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._vector2Group
                    + " " + StringDefine.Pattern._floatGroup
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
            _Position = StringDefine.ParseVector2(groups[2].Value);
            _Scale = StringDefine.ParseFloat(groups[3].Value);
        }
    }

    public class Command_RemoveSprite : Command
    {
        public const string _ID = "removespr";
        public string _Name { get; private set; }

        public override void Do()
        {
            if (_Name == string.Empty)
                GameSystem._Instance._SpriteManager.Remove();
            else
                GameSystem._Instance._SpriteManager.Remove(_Name);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^(?:" + StringDefine.Pattern._wordGroup + ")?$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
        }
    }

    public class Command_Item : Command
    {
        public const string _ID = "spritem";
        public string _Name { get; private set; }
        public Vector2 _Position { get; private set; }
        public float _Scale { get; private set; }

        public override void Do()
        {
            Sprite spr = GameSystem._Instance._ItemSprite.GetOrCreateItem(_Name);

            GameSystem._Instance._ItemSprite.StartFadeIn();
            if (spr == null)
            {
                Debug.LogError("[Command_Sprite.Do.InvalidName]");
                return;
            }

            spr.SetPosition(_Position);
            spr.SetScale(_Scale);

        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._vector2Group
                    + " " + StringDefine.Pattern._floatGroup
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
            _Position = StringDefine.ParseVector2(groups[2].Value);
            _Scale = StringDefine.ParseFloat(groups[3].Value);
        }
    }

    public class Command_item : Command
    {
        public const string _ID = "removeitem";
        public string _Name { get; private set; }

        public override void Do()
        {
            GameSystem._Instance._ItemSprite.Remove(_Name);

        }

        protected override string _ParsePattern
        {
            get
            {
                return "^(?:" + StringDefine.Pattern._wordGroup + ")?$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
        }
    }

    public class Command_itemZoomIn : Command
    {
        public const string _ID = "zoominitem";
        public string _Name { get; private set; }
        public Vector2 _Position { get; private set; }
        public float _Scale { get; private set; }
        public override void Do()
        {
            GameSystem._Instance._ItemSprite.StartZoomItem(_Name, _Position, _Scale);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._vector2Group
                    + " " + StringDefine.Pattern._floatGroup
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
            _Position = StringDefine.ParseVector2(groups[2].Value);
            _Scale = StringDefine.ParseFloat(groups[3].Value);
        }
    }

    public class Command_itemMoveY : Command
    {
        public const string _ID = "moveitem";
        public string _Name { get; private set; }
        public string _Dir { get; private set; }
        public float _Position { get; private set; }

        public override void Do()
        {
            //x -:오른쪽 +:왼쪽
            //y -:아래 +:위
            GameSystem._Instance._ItemSprite.StartMoveXorY(_Name, _Dir, _Position);
        }

        protected override string _ParsePattern
        {
            get
            {
                return "^" + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._wordGroup
                    + " " + StringDefine.Pattern._floatGroup
                    + "$";
            }
        }

        protected override void SetValue(GroupCollection groups, out CommandError error)
        {
            error = null;
            _Name = groups[1].Value;
            _Dir = groups[2].Value;
            _Position = StringDefine.ParseFloat(groups[2].Value);
        }
    }
    public class Command_RecordCg : Command
    {
        public const string _ID = "record";
        public string _Name { get; private set; }

        public override void Do()
        {
            //해당 CG 잠금 해제 이름:0->이름:1
            string[] Ecg = PlayerPrefs.GetString("ECG").Split("|");
            for (int i = 0; i < Ecg.Length; i++) 
            {
                string name = Ecg[i].Split(":")[0];
                if (name == _Name)
                {
                    string info = name + ":1";
                    Ecg[i] = info;
                    string newCg = string.Join("|", Ecg);
                    PlayerPrefs.SetString("ECG", newCg);
                    break;
                } 
            }
            
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
    }
}