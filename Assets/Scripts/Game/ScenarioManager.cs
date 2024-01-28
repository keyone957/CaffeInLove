using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using App;
using System.IO.Enumeration;

namespace Game
{
    public class ScenarioManager : MonoBehaviour
    {
        private Dictionary<string, ConstructorInfo> _constructorInfo = new Dictionary<string, ConstructorInfo>();
        private List<Command> _commands = new List<Command>();
        private Dictionary<string, int> _labels = new Dictionary<string, int>(); // <라벨명, 커맨드 인덱스>
        private int _commandIdx = 0; // 실행 시도할 커맨드 인덱스
        public int saveIdx = 0; //
        public int loadSelectIdx = 0;
        public void Initialize()
        {
            Type baseCommandType = typeof(Command);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            IEnumerable<Type> commandTypes = types.Where(t => t.IsSubclassOf(baseCommandType));

            foreach (Type commandType in commandTypes)
            {
                FieldInfo idField = commandType.GetField(CommandUtil._idFieldName);
                string id = idField.GetRawConstantValue() as string;
                ConstructorInfo constructorInfo = commandType.GetConstructor(Type.EmptyTypes);

                RegisterConstructorInfo(id, constructorInfo);
            }
        }
        public List<Command> rtCommand()
        {
            return _commands;
        }
        private void RegisterConstructorInfo(string commandID, ConstructorInfo constructorInfo) => _constructorInfo.Add(commandID, constructorInfo);

        private Command CreateCommandFromID(string commandID)
        {
            ConstructorInfo constructorInfo;
            if (!_constructorInfo.TryGetValue(commandID, out constructorInfo))
                return null;
            return constructorInfo.Invoke(null) as Command;
        }

        public void Load(string fileName)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(Define._scenarioRoot + "/" + fileName);
            if (textAsset == null)
            {
                Debug.LogError($"[ScenarioLoader.CannotFindFile]{fileName}");
                return;
            }
            AppSystem._GameSystemParam.Set(fileName);
            _commands.Clear();
            _labels.Clear();
            _commandIdx = 0;

            string text = textAsset.text;
            text = text.Trim().Replace("\r", "");
            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Length; ++i)
            {
                CommandError error = null;
                Command command = ParseToCommand(lines[i], out error);

                if (error != null)
                {
                    Debug.LogError($"[ScenarioLoader.InvalidLine]{fileName}({i.ToString()}){error}\n{lines[i]}");
                    continue;
                }
                if (command != null)
                    _commands.Add(command);
            }
            return;
        }

        private Command ParseToCommand(string line, out CommandError error)
        {
            if (line == "" || line.StartsWith("//"))
            {
                error = null;
                return null;
            }

            string commandID;
            string commandParam;
            StringDefine.SplitFirstWord(line, out commandID, out commandParam);

            Command command = CreateCommandFromID(commandID);
            if (command == null)
            {
                error = new CommandInvalidIDError(commandID);
                return null;
            }

            CommandError parseError = null;
            command.Parse(commandParam, out parseError);
            if (parseError != null)
            {
                error = parseError;
                return null;
            }

            CommandError onParsedError = null;
            command.OnParsed(out onParsedError);
            if (onParsedError != null)
            {
                error = parseError;
                return null;
            }

            error = null;
            return command;
        }

        /// <summary>
        /// 라벨을 추가한다.
        /// </summary>
        /// <returns>성공여부</returns>
        private bool AddLabel(string labelName, int commandIdx)
        {
            if (!_labels.ContainsKey(labelName))
            {
                _labels.Add(labelName, commandIdx);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 파싱 후 명령어 목록 추가 전에 라벨 추가 시도
        /// </summary>
        /// <returns>성공여부</returns>
        public bool AddLabelOnParsed(string labelName)
        {
            int commandIdx = _commands.Count;
            if (!AddLabel(labelName, commandIdx))
            {
                Debug.LogError($"[ScenarioLoader.AlreadyAddedLabel]{labelName}");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 지정한 이름의 라벨을 찾아 커맨드 인덱스를 리턴한다.
        /// </summary>
        /// <param name="labelName">라벨 명</param>
        /// <returns>라벨 커맨드 인덱스, 찾지 못하면 무효값</returns>
        private int FindLabelCommandIndex(string labelName)
        {
            int commandIdx;
            if (_labels.TryGetValue(labelName, out commandIdx))
                return commandIdx;
            else
                return Define._invalidLabelIndex;
        }

        /// <summary>
        /// 라벨을 찾아 다음에 실행할 커맨드를 지정한다.
        /// </summary>
        public void JumpToLabel(string labelName)
        {
            int idx = FindLabelCommandIndex(labelName);
            if (idx == Define._invalidLabelIndex)
            {
                Debug.LogError($"[ScenarioLoader.CannotFindLabel]{labelName}");
                return;
            }
            _commandIdx = idx;
        }

        public bool HasRemainedCommand() => (_commands.Count > _commandIdx);

        public void UpdateCommand()
        {
            int curIdx = _commandIdx;
            _commandIdx++; // 커맨드 실행에 의해 인덱스가 다시 바뀔 수 있으므로 미리 증가시켜둔다.
            Command command = _commands[curIdx]; // 커맨드 목록이 바뀔 수 있으므로 별도 변수에 담아둔다.
            Debug.Log(command + "우아");
            command.Do();
            
            //Dictionary<string, Sprite> test = GameSystem._Instance._SpriteManager.rt();
            //foreach (KeyValuePair<string, Sprite> kbp in test)
            //{
            //    Debug.Log("Key: " + kbp);
            //    Debug.Log(kbp.Value + "value");
            //}
            //커맨드가 하나하나 진행될때 마다 슬롯의 오브젝트 이름의 1번에다가 자동저장
            string slotInfo = "";
            if (SceneManager.GetActiveScene().name == "Game")
            {   
                //1번 슬롯에 저장과 동시에 저장시간을 등록함
                GameObject gs = GameSystem._Instance._UI._saveMenu.transform.Find("1").gameObject;
                gs.transform.Find("Date").gameObject.SetActive(true);
                string saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // 씬 정보가 없거나 playerprefs에 들어가는 값들의 타입,형식이 이상하다면 초기화
                // ( 자동저장슬롯(1번 슬롯), 시나리오 인덱스, 시간, 현재 진행중인 시나리오 이름))
                if ((PlayerPrefs.GetString("1").Split("|")[0] == "") || (PlayerPrefs.GetString("1").Split("|").Length < 4))
                    PlayerPrefs.SetString("1", "1|" + (curIdx).ToString() + saveDate + "|" + "Scenario001");
                else
                    slotInfo = "1" + "|" + (curIdx).ToString() + "|" + saveDate + "|" + AppSystem._GameSystemParam._ScenarioName;
                PlayerPrefs.SetString("1", slotInfo);
                gs.transform.Find("Date").GetComponent<Text>().text = saveDate;
                GameSystem._Instance._UI._loadMenu.setDate("1", saveDate);
            }
            else if (SceneManager.GetActiveScene().name == "Cafe")
            {   //cafe에서는 slotinfo[0]=미니 게임 진행 주차
                GameObject gs = CafeSystem._Instance._UI._saveMenu.transform.Find("1").gameObject;
                gs.transform.Find("Date").gameObject.SetActive(true);
                string saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                slotInfo = "1" + "|" + (_commandIdx).ToString() + "|" + saveDate + "|" + PlayerPrefs.GetString("1").Split("|")[3];

                PlayerPrefs.SetString("1", slotInfo);
                gs.transform.Find("Date").GetComponent<Text>().text = saveDate;
                CafeSystem._Instance._UI._loadMenu.setDate("1", saveDate);
            }
            //==> 시나리오가 진행될때 마다 자동저장
        }


        //현재 실행중인 commandidx를 return 
        public int rtCommnadIdx()
        {
            return _commandIdx;
        }

        // select문에서 저장을 하거나 로드를 할때 
        // select이전에 대화 문으로 Load or Save를 하기 때문에
        //ex) select
        //    selectitem ~
        //    selectiem ~
        //    selectend
        //이런 식으로 구성이 되어 있으면 세이브 할때 select ,selectend와 selectitem 의 갯수를 빼면(위에서는 4를 빼야함)
        //선택지 이전의 커맨드로 갈 수 있기 때문에 
        //save할때 인덱스도 그와 맞는 인덱스로 저장을 해야 하므로 
        //빼줘야 할 값을 return 해주는 기능
        public int RtSeclectCount(int saveIdx)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(Define._scenarioRoot + "/" + AppSystem._GameSystemParam._ScenarioName);
            string text = textAsset.text;
            text = text.Trim().Replace("\r", "");
            text = Regex.Replace(text, @"^\s*//.*(\r|\n)?", "", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^\s*$\n|\r", "", RegexOptions.Multiline);//**줄바꿈 빈내용 삭제하는 파싱문
            int selectCount = 0;
            string[] lines = text.Split("\n");
            string commandID;
            string commandParam;
            bool withinSelect = false;

            for (int i = 0; i < saveIdx; ++i)
            {
                StringDefine.SplitFirstWord(lines[i], out commandID, out commandParam);

                if (commandID == "select")//select 명령어 만나면 withinSelect true 황성화 하여 selectend까지 spritem갯수 새는거
                {
                    withinSelect = true;
                }
                else if (commandID == "selectend")
                {
                    withinSelect = false;

                    // selectend 이후의 빈 문자열 체크
                    bool isBlankAfterSelectEnd = true;
                    for (int j = i + 1; j < saveIdx; ++j)
                    {
                        if (lines[j].Trim() != "")
                        {
                            isBlankAfterSelectEnd = false;
                            break;
                        }
                    }
                    if (isBlankAfterSelectEnd)
                    {
                        continue;
                    }
                    else
                    {
                        selectCount = 0; // selectend 이후에 문자열이 있으면 selectCount를 0으로 초기화
                        continue;//누적이 되지 않게 하기 위해
                    }
                }
                else if (withinSelect)
                {
                    if (commandID == "selectitem")//selectitem을만났을때 값을 selectCount값을 증가
                    {
                        selectCount++;
                    }
                }
            }
            int savecount = 0;
            if (selectCount != 0)
            {
                savecount = selectCount + 2;//select , selectend 2개이므로 +2
            }
            else
            {
                savecount = 0;
            }
            return savecount;
        }


        public void UpdateLoadGame(int loadIdx, string slot = "1")
        {                                                                               //불러오려는 시나리오이름
            TextAsset textAsset = Resources.Load<TextAsset>(Define._scenarioRoot + "/" + PlayerPrefs.GetString(slot).Split("|")[3]);
            string[] sceInfo = PlayerPrefs.GetString(slot).Split("|");
            if (sceInfo[0] == "")
            {
                Array.Resize(ref sceInfo, 4);
            }
            string saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //슬롯, commandidx, 날짜
            sceInfo[0] = slot;
            sceInfo[1] = loadIdx.ToString();
            sceInfo[2] = saveDate;
            // 없으면 초기화, 있으면 그대로
            if (sceInfo[3] == "") sceInfo[3] = GameSystem._Instance._ScenarioInfo._defaultMainFileName;
            string newSce = string.Join("|", sceInfo);
            PlayerPrefs.SetString("1", newSce);
            string text = textAsset.text;
            text = text.Trim().Replace("\r", "");
            text = Regex.Replace(text, @"^\s*//.*(\r|\n)?", "", RegexOptions.Multiline);//주석제거
            text = Regex.Replace(text, @"^\s*$\n|\r", "", RegexOptions.Multiline);//줄바꿈 빈내용 삭제하는 파싱문
            List<string> _sprInfoList = new List<string>();//불러올 이미지 정보들
            List<string> _removeSprNameList = new List<string>();//삭제할 이미지 이름
            List<string> _sprItemInfoList = new List<string>();//불러올 아이템 스프라이트 정보
            List<string> _removeItemInfoList = new List<string>();//삭제할 아이템 스프라이트 정보
            string[] lines = text.Split("\n");
            string commandID;
            string commandParam;
            
            string curBg = "";//배경사진 이름
            string curBgm = ""; //배경음악 이름
            string curName = ""; //대화창 이름
            string curItemSpr = "";//아이템 스프라이트 정보
            bool curRemoveItemSpr = false;//아이템 스프라이트 아이템 삭제 여부
            int selectCount = 0;// spritem갯수

            bool withinSelect = false;
            for (int i = 0; i < loadIdx; ++i)
            {
                StringDefine.SplitFirstWord(lines[i], out commandID, out commandParam);
                //불러올때 가장 최근의 배경사진, 배경음, 대화창 이름,스프라이트,아이템 스프라이트 세팅
                if (commandID == "bg")
                {
                    curBg = commandParam;
                }

                if (commandID == "bgm")
                {
                    curBgm = commandParam;
                }

                if (commandID == "name")
                {
                    curName = commandParam;
                }

                if (commandID == "spr")
                {
                    _sprInfoList.Add(commandParam);
                }
                //만약에 removespr을 만나면 그다음에 아무것도 없으면 모든 이미지를 삭제 하므로 sprinfolist를 clear
                //지정한 이미지를 삭제 하려면 이제 removespr commandparam에 맞는 이미지를 sprinfolist에서 삭제 
                else if (commandID == "removespr")
                {
                    if (commandParam == string.Empty)
                    {
                        _sprInfoList.Clear();
                    }
                    else
                    {
                        _sprInfoList.RemoveAll(x => x.Contains(commandParam));
                    }
                }
                if (commandID == "spritem")
                {
                    curItemSpr = commandParam;
                }

                if (commandID == "removeitem")
                {
                    curRemoveItemSpr = true;
                }


                //위 RtSeclectCount 함수와 동일
                if (commandID == "select")
                {
                    withinSelect = true;
                }
                else if (commandID == "selectend")
                {
                    withinSelect = false;

                    // selectend 이후의 빈 문자열 체크
                    bool isBlankAfterSelectEnd = true;
                    for (int j = i + 1; j < loadIdx; ++j)
                    {
                        if (lines[j].Trim() != "")
                        {
                            isBlankAfterSelectEnd = false;
                            break;
                        }
                    }
                    if (isBlankAfterSelectEnd)
                    {
                        continue;
                    }
                    else
                    {
                        selectCount = 0; // selectend 이후에 문자열이 있으면 selectCount를 0으로 초기화
                        continue;
                    }
                }
                else if (withinSelect)
                {
                    if (commandID == "selectitem")
                    {
                        selectCount++;
                    }
                }
            }
            //불러올때 초기에 가장 최근의 배경사진 세팅
            GameSystem._Instance._Background.LoadTexture(curBg);
            GameSystem._Instance._Background.SetTexture(curBg);
            //대화창 이름 세팅
            GameSystem._Instance._UI._Dialogue.SetName(curName);
            //배경음악 세팅
            SoundManager._Instance.PlayBGM(curBgm);
            //로드하기전에 게임에 이미지,스프라이트 아이템이미지 가 있을수 있으므로 초기화
            GameSystem._Instance._SpriteManager.ResetSprObj();
            GameSystem._Instance._SpriteManager.Remove();
            GameSystem._Instance._ItemSprite.ResetItemSprObj();
            //선택창이 게임씬에 나와 있는 경우 초기화
            if (GameSystem._Instance._UI._Select != null)
            {
                GameSystem._Instance._UI.SetNullSelect(GameSystem._Instance._UI._Select);
                GameSystem._Instance._UI.CloseWindow(GameSystem._Instance._UI._Select);
            }


            if (_sprInfoList.Count != 0)//스프라이트 이미지가 있을때 이미지 생성
            {
                for (int i = 0; i < _sprInfoList.Count; i++)
                {
                    Sprite spr = GameSystem._Instance._SpriteManager.GetOrCreate(_sprInfoList[i].Split(' ')[0]);
                    spr.SetPosition(StringDefine.ParseVector2(_sprInfoList[i].Split(' ')[1]));
                    spr.SetScale(StringDefine.ParseFloat(_sprInfoList[i].Split(' ')[2]));
                }
            }

            if (curItemSpr != string.Empty)//아이템 스프라이트 이미지 생성
            {
                //0: 아이템 스프라이트이름, 1: 위치, 2: 크기
                string[] parseItemSpr = curItemSpr.Split(' ');
                Sprite spr = GameSystem._Instance._ItemSprite.GetOrCreateItem(parseItemSpr[0]);
                spr.SetPosition(StringDefine.ParseVector2(parseItemSpr[1]));
                spr.SetScale(StringDefine.ParseFloat(parseItemSpr[2]));
                if (curRemoveItemSpr)//삭제할 아이템 스프라이트가 있을시 삭제
                {
                    GameSystem._Instance._ItemSprite.ResetItemSprObj();
                }
            }


            if (selectCount != 0)//선택지가 있을때
            {
                loadIdx = loadIdx - selectCount - 2;
                loadSelectIdx = selectCount + 2;//uipop에서 load할때 사용할 변수
            }

            _commandIdx = loadIdx;
            Command command = _commands[_commandIdx-1];

            command.Do();
        }

    }
}

