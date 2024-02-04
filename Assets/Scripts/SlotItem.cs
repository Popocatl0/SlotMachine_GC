using UnityEngine;
using DG.Tweening;

public class SlotItem : MonoBehaviour
{
    int currentIndex;
    SpriteRenderer render;
    public int CurrentIndex => currentIndex;
    private void Awake() {
        render = GetComponent<SpriteRenderer>();
    }

    public void UpdateSlot(int index, Sprite image){
        currentIndex = index;
        render.sprite = image;
    }

    public void StartAnimation(){
        render.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopAnimation(){
        render.DOKill();
        render.color = Color.white;
    }
}
