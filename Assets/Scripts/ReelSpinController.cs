using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ReelSpinController : MonoBehaviour
{
    public float maxspeed = 10.0f;
    public bool checkFinal;
    [SerializeField] private SlotItem[] SlotArray;
    [SerializeField] private Transform StartPoint, EndPoint, CenterPoint;
    private int reelID;
    public delegate void NextSlotEvent(int id, SlotItem slot);
    private NextSlotEvent nextSlot;
    bool isRunning;
    public bool IsRunning => isRunning;
    float speed = 5;
    int finalSlotIndex;
    public void Init(int id, NextSlotEvent slotEvent){
        reelID = id;
        nextSlot += slotEvent;
        isRunning = false;
        checkFinal = false;
        for (int i = SlotArray.Length - 1 ; i >= 0; i--){
            nextSlot(reelID, SlotArray[i]);
        }
    }

    public void Reset(){
        isRunning = true;
        checkFinal = false;
        speed = 5;
        SlotArray[finalSlotIndex].StopAnimation();
    }

    public void AnimateFinalSlot(){
        SlotArray[finalSlotIndex].StartAnimation();
    }

    public void SpinSlots(int finalIndex){
        if(!isRunning) return;
        for (int i = 0; i < SlotArray.Length; i++){
            MoveSlot(SlotArray[i]); 
            if(SlotArray[i].CurrentIndex == finalIndex) finalSlotIndex = i;
        }

        if(checkFinal && CheckFinalSlot(SlotArray[finalSlotIndex], finalIndex)){ 
            isRunning=false;
            CenterSlots();
        }
        if(speed >= maxspeed) return;
        speed += Time.deltaTime * maxspeed;
        speed = Mathf.Clamp(speed, 0, maxspeed);
    }
    
    void MoveSlot(SlotItem slot){
        slot.transform.localPosition = Vector3.MoveTowards(slot.transform.localPosition, EndPoint.localPosition, speed * Time.deltaTime);

        if( Mathf.CeilToInt(slot.transform.localPosition.y) <= EndPoint.localPosition.y){
            slot.transform.localPosition = StartPoint.localPosition;
            nextSlot(reelID, slot);
        }
    }

    public void CenterSlots(){
        SlotArray[finalSlotIndex].transform.localPosition = CenterPoint.localPosition;
        for (int i = 0; i < SlotArray.Length; i++){
            SlotArray[i].transform.localPosition = new Vector3( 0, Mathf.RoundToInt(SlotArray[i].transform.localPosition.y/2)*2, 0);
        }
    }

    bool CheckFinalSlot(SlotItem slot, int finalIndex){
        return slot.CurrentIndex == finalIndex && slot.transform.localPosition.y <= CenterPoint.localPosition.y;
    }
}
