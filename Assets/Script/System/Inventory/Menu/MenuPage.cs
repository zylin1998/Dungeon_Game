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
    [SerializeField]
    private TeleportSpots teleportSpots;

    private UserOption userOption;

    private void Awake()
    {
        userOption = SaveSystem.LoadData<UserOption>(Path.Combine(Application.dataPath, "SaveData", "UserData.json"), SaveSystem.ESaveType.Json);
    }

    private void Start()
    {
        if (userOption == null) { resume.interactable = false; }

        SetButtons();
    }

    private void StartClick() 
    {
        userData.Initialized();
        
        SceneManager.LoadScene(teleportSpots.CrossSpot.scene);
    }

    private void ResumeClick() 
    {
        userData.Initialized(userOption.GetPack());

        teleportSpots.SetLocate(new TeleportSpots.TSData(teleportSpots.SaveSpot));

        SceneManager.LoadScene(teleportSpots.CrossSpot.scene);
    }

    private void SetButtons() 
    {
        if (start) { start.onClick.AddListener(StartClick); }
        if (resume) { resume.onClick.AddListener(ResumeClick); }
    }
}
