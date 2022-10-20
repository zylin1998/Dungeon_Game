using UnityEngine;
using Cinemachine;
using ComponentPool;
using RoleSystem;

public class CameraFocus : MonoBehaviour
{
    #region Parameter Field

    private CinemachineVirtualCamera freeLookCamera;
    
    #endregion

    #region Script Behaviour

    private void Awake()
    {
        CustomContainer.AddContent(this, "Character");
        
        freeLookCamera = GetComponent<CinemachineVirtualCamera>();
    }

    #endregion

    #region Camera Sight Update

    public void ChangeFocus(Transform character) 
    {
        freeLookCamera.Follow = character;
    }

    public void CameraIsEnable(bool state) => freeLookCamera.enabled = state;

    #endregion

    private void OnDestroy()
    {
        CustomContainer.RemoveContent(this, "Character");
    }
}
