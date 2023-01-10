using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.IO;

public class DataManager : IManager
{
    private UserData user;
    public UserData User => user;

    private string SAVE_PATH;
    private const string SAVE_FILE = "/Savefile.json";

    private float saveDelay = 3f;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    private void Init(){
        SAVE_PATH = Application.dataPath + "/Save";

        if(!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        LoadData(out user);

        Observable.Timer(TimeSpan.FromSeconds(saveDelay))
        .Repeat()
        .Subscribe(time => SaveUser())
        .AddTo(GameManager.Instance);
    }

    private void LoadData(out UserData data){
        if(File.Exists(SAVE_PATH + SAVE_FILE)){
            string dataString = File.ReadAllText(SAVE_PATH + SAVE_FILE);
            data = JsonUtility.FromJson<UserData>(dataString);
        }
        else{
            data = new UserData();
            SaveData(data);
        }
    }

    private void SaveData(UserData data){
        string dataString = JsonUtility.ToJson(data);

        File.WriteAllText(SAVE_PATH + SAVE_FILE, dataString);
    }

    public void SaveUser(){
        SaveData(user);
    }
}
