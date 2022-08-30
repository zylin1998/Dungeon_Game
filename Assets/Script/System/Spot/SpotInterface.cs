using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDocumentHandler 
{
    public string SpotName { get; }

    public void Documental();
}

public interface ICroseSceneHandler 
{
    public string TargetScene { get; }

    public string TargetSpot { get; }

    public void CrossScene();
}
