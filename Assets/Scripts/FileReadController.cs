using System.Collections.Generic;
using Leguar.TotalJSON;
[System.Serializable]
public class ReelStripsItem{
   public List<List<string>> ReelStrips;
}

[System.Serializable]
public class SpinItem{
    public int[] ReelIndex;
    public int ActiveReelCount;
    public int WinAmount;
}

[System.Serializable]
public class SpinsArray{
    public List<SpinItem> Spins;
}

public class FileReadController{
    ReelStripsItem reelStripsInJson;
    SpinsArray spinsInJson;
    int[] currentIndex;
    public FileReadController(string reelstripsFile, string spinsFile) {
        JSON jsonReel = JSON.ParseString(reelstripsFile);
        reelStripsInJson = jsonReel.Deserialize<ReelStripsItem>();

        JSON jsonSpins = JSON.ParseString(spinsFile);
        spinsInJson = jsonSpins.Deserialize<SpinsArray>();

        currentIndex = new int[]{0,0,0};
    }

    public SpinItem GetSpinResult(){
        return spinsInJson.Spins[UnityEngine.Random.Range(0, spinsInJson.Spins.Count)];
    }

    public (int,string) GetNextReelStrip(int reelIndex){
        int index = currentIndex[reelIndex]++;
        string next = reelStripsInJson.ReelStrips[reelIndex][index];
        if(currentIndex[reelIndex] >= reelStripsInJson.ReelStrips[reelIndex].Count) currentIndex[reelIndex] = 0;
        return (index, next);
    }
    
    
}
