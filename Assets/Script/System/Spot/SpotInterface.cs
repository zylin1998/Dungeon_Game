using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDocumentSpotHandler 
{
    public string SceneName { get; }

    public string SpotName { get; }

    public void Documental();
}

public interface ICroseSceneSpotHandler 
{
    public string TargetScene { get; }

    public string TargetSpot { get; }

    public void CrossScene();
}

public interface ITeleportSpotHandler 
{
    public string SceneName { get; }

    public string SpotName { get; }

    public bool isOn { get; set; }
}