using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct SlotImageItem{
    public Sprite image;
    public string name;
}

public class SlotMachineController : MonoBehaviour
{
    public TextAsset reelstripsFile;
    public TextAsset spinsFile;
    public TextMeshProUGUI Score;
    [SerializeField] private ReelSpinController[] reelSpinList;
    [SerializeField] private SlotImageItem[] slotImageList;
    private FileReadController fileReader;
    private Dictionary<string, Sprite> slotImageDictionary;
    private SpinItem spinResult;
    bool isRunning;
    private void Start() {
        fileReader = new FileReadController(reelstripsFile.text, spinsFile.text);
        slotImageDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < slotImageList.Length; i++){
            slotImageDictionary.Add(slotImageList[i].name, slotImageList[i].image);
        }

        for (int i = 0; i < reelSpinList.Length; i++){
            reelSpinList[i].Init(i,SlotNextData);
        }
        isRunning = false;
    }

    private void Update() {
        if(!isRunning) return;
        for (int i = 0; i < reelSpinList.Length; i++){
            reelSpinList[i].SpinSlots(spinResult.ReelIndex[i]);
        }
    }
    public void StarMachine(){
        if(isRunning) return;
        spinResult = fileReader.GetSpinResult();
        isRunning = true;
        Score.text = "Total Win : 0";
        for (int i = 0; i < reelSpinList.Length; i++){
            reelSpinList[i].Reset();
        }
        StopMachine();
    }

    async void StopMachine(){
        await Task.Delay(1000);
        for (int i = 0; i < reelSpinList.Length; i++){
            await Task.Delay(300);
            reelSpinList[i].checkFinal = true;
            await Task.Run(() => {
                while (reelSpinList[i].IsRunning == true){ }
            });
        }
        isRunning = false;
        Score.text = "Total Win : " + spinResult.WinAmount;
        for (int i = 0; i < spinResult.ActiveReelCount; i++){
            reelSpinList[i].AnimateFinalSlot();
        }
    }

    private void SlotNextData(int id, SlotItem slot){
        (int index, string name) = fileReader.GetNextReelStrip(id);
        slot.UpdateSlot(index, slotImageDictionary[name]);
    }
}