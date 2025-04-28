# 카페 인 러브- 비주얼 노벨

진행 기간: 2023.04 ~ 2023.05

사용한 기술 스택: C#, Unity

한 줄 설명: 비주얼 노벨 게임

개발 인원(역할): 개발자 2명 + 디자이너 1명 + 기획자 2명 + 스토리 작가 1명

비고: 팀 프로젝트 / 창업동아리 StoryG 미니 프로젝트

## 프로젝트 소개

---
    
- 카페인 러브 - 유저의 선택지에 따라 결말이 달라지는 비주얼 노벨
- 디자이너와 협업시 사용했던 피그마 주소
    
    [피그마 주소](https://www.figma.com/file/54PIxE9W6Ad5YeWR7Edo5v/Untitled?type=design&node-id=0-1&mode=design&t=mFsBqRLaCq0jlXYD-0)

![Image](https://github.com/user-attachments/assets/2212b667-5fdb-473a-800a-a3735497a6cd)

    

## 나의 역할

---

1. 시나리오 연출 커맨드 추가
    - 템플릿 메서드 패턴을 이용한 커맨드 시스템 구현
    - 정규식을 이용해 TXT파일을 파싱하여 게임 내 연출 및 스토리 진행 기능
    - CG 이미지 삽입
    - 클로즈업 & 시선이동
    - 페이드 인 / 아웃
    - 깜빡이는 페이드 인 /  아웃
2. 메인 시스템 및 부가기능
    - 환경설정
    - 유저의 진행상황에(txt파일에 따른 진행상황) 따른 세이브 로드 기능
    - 자동 저장기능

## 개발 내용 및 플레이 영상

---

### [게임 플레이 관련]

![Image](https://github.com/user-attachments/assets/e4fc4841-464d-4422-8f88-77f0d109f6ed)

⇒게임 시작시 메인 화면

![Image](https://github.com/user-attachments/assets/9ad7eed1-a84e-40a4-b339-5a31f7cfe8ed)

=⇒비주얼 노벨 특성상 필요한 기능인 선택지를 고르는 게임 화면

![Image](https://github.com/user-attachments/assets/284054c4-8464-4dea-ae5b-1b74ff97fe25)

=⇒각 에피소드가 끝나면 자동저장 ,간단한 에피소드 요약 및 미니게임을 통해 다음 에피소드나 서브 에피소드를 진행 할 수 있다. 또한 가구를 구매하면 얻는 돈이 늘어나 추가 에피소드 플레이 가능 및 스토리가 달라질 수도 있다.


### [부가기능 및 시나리오 연출]

![Image](https://github.com/user-attachments/assets/0c214593-5d3e-49e3-808e-b5e6c859191d)

=⇒환경설정 : 대화 출력모드, 대화 출력속도, 소리크기, 화면모드

![Image](https://github.com/user-attachments/assets/9aa1e51c-b7bd-4fd6-8a4c-fa98e8f4fe99)

![Image](https://github.com/user-attachments/assets/cad556ed-fd22-41cd-9218-83164765810d)

⇒플레이 도중 세이브 및 로드 기능 구현

[이미지 출력 영상](https://youtu.be/o3iojsZKz34)

⇒시나리오 진행중 구현한 연출중 일부기능인 아이템 이미지출력

### [시나리오 파일을 파싱하여 게임 흐름 구현]

![Image](https://github.com/user-attachments/assets/25083c34-6c98-4ec3-acdd-d5d34868e41a)

![Image](https://github.com/user-attachments/assets/bee3081a-c5bb-418e-933b-0259145f6f6a)

=⇒ 시나리오 작가가 개발자들이 개발한 커맨드 ID(명령어)를 가지고 시나리오 특수 효과 및 스토리를 작성하여 넘겨주면 개발자는 단순히 폴더에 시나리오 파일을 넣는 것 만으로도 작가가 원하는 시나리오 연출 및 스토리 진행을 구성할 수 있다. 

—> 커맨드 추상 클래스

<details>
<summary>Command.cs</summary>

```csharp
using System.Text.RegularExpressions;

namespace Game
{
    public static class CommandUtil
    {
        public const string _idFieldName = "_ID";
    }

    public abstract class Command
    {
        public abstract void Do();

        protected abstract string _ParsePattern { get; }

        public void Parse(string param, out CommandError error)
        {
            CommandError matchError = null;
            Match match = Match(param, out matchError);
            if (matchError != null)
            {
                error = matchError;
                return;
            }

            CommandError setValueError = null;
            SetValue(match.Groups, out setValueError);
            if (setValueError != null)
            {
                error = setValueError;
                return;
            }
            error = null;
        }

        private Match Match(string param, out CommandError error)
        {
            Match match = Regex.Match(param, _ParsePattern);
            if (!match.Success)
            {
                error = new CommandInvalidPatternError(_ParsePattern);
                return null;
            }
            else
            {
                error = null;
                return match;
            }
        }

        protected abstract void SetValue(GroupCollection groups, out CommandError error);

        public virtual void OnParsed(out CommandError error) => error = null;
    }
}

```

</details>

<details>
<summary>Command_Item.cs</summary>

=>스프라이트 이미지 추가 커맨드 예시 코드

```csharp
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

```

</details>


⇒TXT 파일을 파싱하여 시나리오 작가가 커맨드 ID를 추가하고 양식에 맞게 파일을 구성 해주면 비주얼 노벨에 적용이 된다.

### [게임 플레이 풀영상]

[게임 플레이 풀영상](https://youtu.be/-24oX6T16wU)

## 프로젝트 사용기술

---

### ⚒️ 클라이언트

- Unity
- C#

### ⚒️ 버전 관리 및 협업

- Git
- Notion
- Dooray
- Figma

### ⚒️ 개발 환경

- Visual Studio

## 프로젝트 성과 및 성취

---

이번 프로젝트에는 UIManager, GameSystem, ScenarioManager,SpriteManager 등 각 주요 역할을 담당하는 컴포넌트들을 싱글톤으로 구성하여 코드의 재사용성 및 메모리 최적화를 이뤄냈습니다. 

또한 Command 컴포넌트를 사용해 커맨드 ID를 생성하고, 추상 클래스와 오버라이딩 기법을 실전 프로젝트에서 직접 적용했습니다. 이 과정에서 저는 코드를 구조화하고 기능을 명확하게 구분하는 방식으로 프로젝트를 진행하였습니다.
게임의 전반적인 흐름을 제어하기 위해 코루틴을 사용하였고, 코드 플로우 차트를 작성해 전체 구조와 게임의 흐름을 이해하고 관리하는 데 기여했습니다.
