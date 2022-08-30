using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPage : MonoBehaviour
{
    [SerializeField]
    private Button start;
    [SerializeField]
    private Button resume;
    [SerializeField]
    private UserData userData;

    private UserData.Packed userPack;

    private void Awake()
    {
        userPack = SaveSystem.LoadJsonData<UserData.Packed>(Path.Combine(Application.dataPath, "SaveData", "UserData.json"));
    }

    private void Start()
    {
        if (userPack == null) { resume.interactable = false; }

        SetButtons();
    }

    private void StartClick() 
    {
        userData.GoTo("TownEntry", "Spot 1");
        userData.Initialize();
        SceneManager.LoadScene(userData.Scene);
    }

    private void ResumeClick() 
    {
        userData.Initialize(userPack);
        SceneManager.LoadScene(userPack.scene);
    }

    private void SetButtons() 
    {
        if (start) { start.onClick.AddListener(StartClick); }
        if (resume) { resume.onClick.AddListener(ResumeClick); }
    }
}
