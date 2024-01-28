using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Game {
    public class TestLoadGame : UIWindow
    {
        [SerializeField]
        private Button _loadBtn = null;
        [SerializeField]
        private Button _saveBtn = null;
        private void Start()
        {
            _saveBtn.onClick.AddListener(testSaveBtn);
            _loadBtn.onClick.AddListener(testLoadBtn);
        }
        private void testLoadBtn()
        {
            //시나리오
            Debug.Log(App.AppSystem._GameSystemParam._ScenarioName + "시나리오 이름");
            //사진 

            Dictionary<string, Sprite> test = GameSystem._Instance._SpriteManager.rt();
            //GameSystem._Instance._SpriteManager.Remove();
            foreach (KeyValuePair<string, Sprite> kbp in test)
            {
                Sprite spr = kbp.Value;
                //Debug.Log("Key: " + kbp);
                Debug.Log(spr.GetComponent<Sprite>().transform.position.x+":"+spr.GetComponent<Sprite>().transform.position.y + "value");
                Debug.Log(spr.GetComponent<Sprite>().transform.localScale.x + "스케일");
                Sprite newspr=GameSystem._Instance._SpriteManager.GetOrCreate(kbp.Key);
                newspr.SetPosition(new Vector3(spr.GetComponent<Sprite>().transform.position.x, spr.GetComponent<Sprite>().transform.position.y));
                newspr.SetScale(spr.GetComponent<Sprite>().transform.localScale.x);
              
                
            }

            Debug.Log(GameSystem._Instance._ScenarioManager.rtCommnadIdx());

            List<Command> _commands = GameSystem._Instance._ScenarioManager.rtCommand();
            
            Command command = _commands[22];
            Debug.Log(_commands[22] + "가이지");
            //command.Do();
        }
        private void testSaveBtn()
        {
            List<string> sprInfo = new List<string>();
            List<string> hello = new List<string>();
            hello.Add(App.AppSystem._GameSystemParam._ScenarioName);
            hello.Add(GameSystem._Instance._ScenarioManager.rtCommnadIdx().ToString());
            Dictionary<string, Sprite> test = GameSystem._Instance._SpriteManager.rt();
            //GameSystem._Instance._SpriteManager.Remove();
            
            foreach (KeyValuePair<string, Sprite> testvalue in test)
            {
                Sprite spr = testvalue.Value;
                string sprName = testvalue.Key;
                //Debug.Log(testvalue.Key+"키");
                Debug.Log(spr.GetComponent<Sprite>().transform.position+" : "+spr.GetComponent<Sprite>().transform.localScale.x);
                Debug.Log(sprName+"이름");
                //string newsprinfo=spr.GetComponent<Sprite>().transform.position+"" 
                //Sprite newspr = GameSystem._Instance._SpriteManager.GetOrCreate(testvalue.Key);
                //newspr.SetPosition(new Vector3(spr.GetComponent<Sprite>().transform.position.x, spr.GetComponent<Sprite>().transform.position.y));
                //newspr.SetScale(spr.GetComponent<Sprite>().transform.localScale.x);
                
            }
            Debug.Log(GameSystem._Instance._UI._Dialogue.rtName().text+"이름");
            Debug.Log(GameSystem._Instance._UI._Dialogue.rtText().text+"내용");
            string.Join("|", hello);
            for (int i = 0; i < hello.Count; i++)
            {
                Debug.Log(hello[i] + "이거");
            }
                Debug.Log(App.AppSystem._GameSystemParam._ScenarioName + "시나리오 이름");
                //PlayerPrefs.SetString("test",hello.ToString());
            
        }


    }
}
