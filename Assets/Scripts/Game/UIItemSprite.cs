using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game
{
    public class UIItemSprite : MonoBehaviour
    {
        private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();//스프라이트 아이템들 담아둘 딕셔너리
        public float duration = 1.0f;//이미지 이동, 확대 속도
        private SpriteRenderer spriteBg;//아이템스프라이트 뒤에 제일큰 투명한 이미지
        private SpriteRenderer spriteMaskBg;//그 안에 테두리 있는 이미지
        private SpriteRenderer spriteRenderer;//실제 보여줄 스프라이트 아이템 생성

        private Vector3 originalScale;//확대할 원래 이미지 스케일
        private float targetScale = 2f;
        public Sprite GetOrCreateItem(string name)
        {   //스프라이트 아이템 생성
            Sprite spr;
            Sprite itemImg;
            UnityEngine.Sprite spriteItemBg = Resources.Load<UnityEngine.Sprite>(Define._spriteRoot + "/itemBg");//스프라이트 아이템 제일큰 뒷배경

            Sprite prefab = Resources.Load<Sprite>(Define._itemSpritePrefabPath);//스프라이트 아이템 기본 프리펩 생성
            spr = Object.Instantiate(prefab);
            spr.Initialize(spriteItemBg);
            spr.SetPosition(new Vector2(0.0f, 0.2f));
            spr.SetScale(0.2f);
            //각 스프라이트 아이템은 마스크 이미지를 가지고 있음(테두리가지고 있는 이미지)
            UnityEngine.Sprite maskSprite = Resources.Load<UnityEngine.Sprite>(Define._maskSpriteRoot + "/mask" + name);
            

            GameObject itemMask = spr.transform.Find("itemMask").gameObject;
            itemMask.GetComponent<SpriteMask>().sprite = maskSprite;

            SpriteRenderer itemMaskSprite = itemMask.GetComponent<SpriteRenderer>();
            itemMaskSprite.drawMode = UnityEngine.SpriteDrawMode.Sliced;
            itemMaskSprite.sprite = maskSprite;
            //테두리 크기가 0.2씩 늘려줘야 이미지가 잘리지 않고 spriteMaskBg에 들어감
            itemMaskSprite.size = new Vector2(itemMaskSprite.size.x + 0.2f, itemMaskSprite.size.y + 0.2f);

            if (!_sprites.TryGetValue(name, out itemImg))
            {
                UnityEngine.Sprite spriteResource = Resources.Load<UnityEngine.Sprite>(Define._spriteRoot + "/" + name);

                if (spriteResource == null)
                {
                    return null;
                }
                itemImg = spr.gameObject.transform.Find("itemMask").Find("Item").GetComponent<Sprite>();
                itemImg.Initialize(spriteResource);
                _sprites.Add(name, spr);
            }
            spriteRenderer = itemImg.GetComponent<SpriteRenderer>();
            spriteMaskBg = itemMaskSprite;
            spriteBg = spr.GetComponent<SpriteRenderer>();

            return itemImg;
        }

        public void StartFadeIn()//페이드인 코루틴 등록
        {
            GameSystem._Instance.RegisterTask(FadeIn());
        }

        public void StartZoomItem(string itemName, Vector2 target, float targetScale)//이미지 확대 하면서 이동
        {
            GameSystem._Instance.RegisterTask(ScaleUp(itemName, target, targetScale));
        }

        public void StartMoveXorY(string itemName, string dir, float targetY)// x축이나 y축으로 이동
        {
            GameSystem._Instance.RegisterTask(moveXorY(itemName, dir, targetY));
        }

        public void Remove(string name)//스프라이트 아이템 삭제
        {
            Sprite spr;

            if (!_sprites.TryGetValue(name, out spr))
            {
                return;
            }
            _sprites.Remove(name);
            GameSystem._Instance.RegisterTask(FadeOut());
        }
        // 로드할때 스프라이트 아이템 모두삭제
        public void ResetItemSprObj()
        {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "ItemBg(Clone)").ToArray();
            foreach (GameObject obj in gameObjects)
            {
                Destroy(obj);
            }
            _sprites.Clear();
        }

        private IEnumerator FadeIn()//스프라이트 아이템이 나타날때 페이드인
        {
            
            Color color = setAlpha(spriteBg, 0f);
            Color color1 = setAlpha(spriteMaskBg, 0f);
            Color color2 = setAlpha(spriteRenderer, 0f);
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                spriteBg.color = color;
                color1.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                spriteMaskBg.color = color1;
                color2.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                spriteRenderer.color = color2;

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            color.a = 1f;
            spriteBg.color = color;
            color1.a = 1f;
            spriteMaskBg.color = color1;
            color2.a = 1f;
            spriteRenderer.color = color2;
        }

        private IEnumerator FadeOut()//스프라이트 아이템 없어질때 페이드 아웃
        {
            if (spriteRenderer == null || spriteBg == null || spriteMaskBg == null)
            {
                yield break;
            }

            Color color = setAlpha(spriteBg, 1f);
            Color color1 = setAlpha(spriteMaskBg, 1f);
            Color color2 = setAlpha(spriteRenderer, 1f);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                spriteBg.color = color;
                color1.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                spriteMaskBg.color = color1;
                color2.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                spriteRenderer.color = color2;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            color.a = 0f;
            spriteRenderer.color = color;

            color1.a = 0f;
            spriteMaskBg.color = color;

            color2.a = 0f;
            spriteBg.color = color;

            Object.Destroy(spriteRenderer.gameObject);
            Object.Destroy(spriteBg.gameObject, duration);
            Object.Destroy(spriteMaskBg.gameObject, duration);//삭제할때 큰배경, 스프라이트 마스크 이미지, 스프라이트 아이템
        }


        //서서히 스프라이트 아이템을 확대 and 이동
        public IEnumerator ScaleUp(string name, Vector2 targetPosition, float targetScale)
        {
            Sprite spr;
            if (!_sprites.TryGetValue(name, out spr))
            {
                Debug.LogError($"[SpriteManager.Remove.InvalidName]{name}");
                yield break;
            }
            //실제 보여줄 스프라이트 아이템 이미지
            SpriteRenderer item = spr.gameObject.transform.Find("itemMask").Find("Item").GetComponent<SpriteRenderer>();
            float elapsedTime = 0f;

            originalScale = item.transform.localScale;
            Vector2 originalPosition = item.transform.localPosition;
            while (elapsedTime <= duration)
            {
                float t = elapsedTime / duration;
                item.transform.localScale = Vector3.Lerp(originalScale, originalScale * targetScale, t);
                item.transform.localPosition = Vector2.Lerp(originalPosition, targetPosition * 0.5f, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            item.transform.localScale = originalScale * targetScale;
            item.transform.localPosition = targetPosition * 0.5f;
        }
        //x축이나 y축으로 스프라이트 아이템 이미지를 이동시킴
        private IEnumerator moveXorY(string name, string dir, float targetDir)
        {
            //x -:오른쪽 +:왼쪽
            //y -:아래 +:위
            Sprite spr;
            if (!_sprites.TryGetValue(name, out spr))
            {
                Debug.LogError($"[SpriteManager.Remove.InvalidName]{name}");
                yield break;
            }
            SpriteRenderer item = spr.gameObject.transform.Find("itemMask").Find("Item").GetComponent<SpriteRenderer>();
            float elapsedTime = 0f;

            Vector3 originalPosition = item.transform.localPosition;
            //y축으로 이동할때
            if (dir == "y")
            {
                Vector3 targetPosition = new Vector3(originalPosition.x, targetDir, originalPosition.z);

                while (elapsedTime <= duration)
                {
                    float t = elapsedTime / duration;
                    item.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                item.transform.localPosition = targetPosition;
            }
            //x축으로 이동할때
            else if (dir == "x")
            {
                Vector3 targetPosition = new Vector3(targetDir, originalPosition.y, originalPosition.z);

                while (elapsedTime <= duration)
                {
                    float t = elapsedTime / duration;
                    item.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                item.transform.localPosition = targetPosition;
            }

        }

        //스프라이트 아이템을 확대할때만
        public IEnumerator ScaleUp(string name, float targetScale)
        {
            Sprite spr;
            if (!_sprites.TryGetValue(name, out spr))
            {
                Debug.LogError($"[SpriteManager.Remove.InvalidName]{name}");
                yield break;
            }

            SpriteRenderer item = spr.gameObject.transform.Find("itemMask").Find("Item").GetComponent<SpriteRenderer>();
            float elapsedTime = 0f;
            originalScale = item.transform.localScale;
            while (elapsedTime <= duration)
            {
                float t = elapsedTime / duration;
                item.transform.localScale = Vector3.Lerp(originalScale, originalScale * targetScale, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            item.transform.localScale = originalScale * targetScale;
        }


        private Color setAlpha(SpriteRenderer _spriteRender, float alpha)//
        {
            Color color = _spriteRender.color;
            color.a = alpha;
            _spriteRender.color = color;

            return _spriteRender.color;
        }
    }
}
