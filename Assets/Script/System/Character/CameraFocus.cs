using UnityEngine;
using Cinemachine;
using ComponentPool;
using RoleSystem;

public class CameraFocus : MonoBehaviour
{
    #region Parameter Field

    private CinemachineVirtualCamera freeLookCamera;
    
    private CharacterState characterState;
    
    #endregion

    #region Script Behaviour

    private void Awake()
    {
        Components.Add(this, "CameraFocus", EComponentGroup.Script);
        
        freeLookCamera = GetComponent<CinemachineVirtualCamera>();
    }

    #endregion

    #region Camera Sight Update

    public void ChangeFocus() 
    {
        if(characterState == null) { characterState = Components.GetStaff<CharacterState>("CharacterState", EComponentGroup.Script); }

        freeLookCamera.Follow = characterState.CurrentCharacter;
    }

    public void CameraIsEnable(bool state) => freeLookCamera.enabled = state;
    
    #endregion
}
