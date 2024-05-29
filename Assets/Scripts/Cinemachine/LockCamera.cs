using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[ExecuteInEditMode, SaveDuringPlay, AddComponentMenu("")]
public class LockCamera : CinemachineExtension
{
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    [Space, Tooltip("The position the camera will lock to")]
    public Vector3 lockPosition = new Vector3(0, 0, 0);

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = lockX ? lockPosition.x : pos.x;
            pos.y = lockY ? lockPosition.y : pos.y;
            pos.z = lockZ ? lockPosition.z : pos.z;
            state.RawPosition = pos;
        }
    }
}
