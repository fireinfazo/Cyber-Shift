using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEngine;

[EditorTool("Room Conector", typeof(RoomEmpty))]
public class RoomConnector : EditorTool
{
    [SerializeField] private Texture2D ToolIcon;

    private Transform oldTarget;
    private RoomPoint[] allPoints;
    private RoomPoint[] targetPoints;

    private void OnEnable()
    {
        Debug.Log("Enabled Room Conector!");
    }
    public override GUIContent toolbarIcon
    {
        get
        {
            return new GUIContent
            {
                image = ToolIcon,
                text = "Connect room",
                tooltip = "This instrument help you work with room"
            };
        }
    }
    public override void OnToolGUI(EditorWindow window)
    {
        Transform targetTransform = ((RoomEmpty)target).transform;

        if (targetTransform != oldTarget)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(targetTransform.gameObject);

            if (prefabStage != null)
                allPoints = prefabStage.prefabContentsRoot.GetComponentsInChildren<RoomPoint>();
            else
                allPoints = FindObjectsOfType<RoomPoint>();

            targetPoints = targetTransform.GetComponentsInChildren<RoomPoint>();

            oldTarget = targetTransform;
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetTransform, "Move with custom snap tool");

            if (((RoomEmpty)target).isGround) newPosition.y = 0;

            MoveWithSnapping(targetTransform, newPosition);
        }
    }
    private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
    {
        Vector3 bestPosition = newPosition;
        float closestDistance = float.PositiveInfinity;

        foreach (RoomPoint point in allPoints)
        {
            if (point.transform.parent == targetTransform) continue;

            foreach (RoomPoint ownPoint in targetPoints)
            {
                if (ownPoint.type != point.type) continue;

                Vector3 targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                float distance = Vector3.Distance(targetPos, newPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPosition = targetPos;
                }
            }
        }

        if (closestDistance < 5.5f)
        {
            targetTransform.position = bestPosition;
        }
        else
        {
            targetTransform.position = newPosition;
        }
    }
}