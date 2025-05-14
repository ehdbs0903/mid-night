using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualScripting : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Scroll View → Viewport → Content")]
    public Transform contentPanel;
    [Tooltip("Content에 추가할 텍스트 아이템 Prefab")]
    public GameObject itemPrefab;

    [Header("Target")]
    public GameObject player;

    [Header("Move/Rotate Settings")]
    public float moveDistance     = 1f;
    public float moveDuration     = 0.2f;
    public float rotationAngle    = 90f;
    public float rotationDuration = 0.2f;
    
    private struct Command
    {
        public Func<IEnumerator> action;
        public string name;
        public Command(Func<IEnumerator> action, string name)
        {
            this.action = action;
            this.name   = name;
        }
    }
    
    private List<Command> commandQueue = new List<Command>();
    
    private bool  isLooping      = false;
    private int   loopStartIndex = 0;
    private int   loopCount      = 0;
    
    public void EnqueueMoveForward()
    {
        commandQueue.Add(new Command(MoveProcess, "Move Forward"));
        RefreshUI();
    }

    public void EnqueueRotateLeft()
    {
        commandQueue.Add(new Command(() => RotateProcess(-rotationAngle), "Rotate Left"));
        RefreshUI();
    }

    public void EnqueueRotateRight()
    {
        commandQueue.Add(new Command(() => RotateProcess(rotationAngle), "Rotate Right"));
        RefreshUI();
    }
    
    public void EnqueueLoopStart()
    {
        if (!isLooping)
        {
            isLooping      = true;
            loopStartIndex = commandQueue.Count;
            loopCount      = 1;
        }
        else
        {
            loopCount++;
        }
        RefreshUI();
    }

    public void EnqueueLoopEnd()
    {
        if (!isLooping) return;

        int innerCount = commandQueue.Count - loopStartIndex;
        var inner      = commandQueue.GetRange(loopStartIndex, innerCount);

        commandQueue.RemoveRange(loopStartIndex, innerCount);

        commandQueue.Add(
            new Command(
                () => LoopProcess(inner, loopCount),
                $"Loop x{loopCount}"
            )
        );

        isLooping = false;
        RefreshUI();
    }

    
    public void ExecuteCommands()
    {
        if (commandQueue.Count == 0) return;
        StartCoroutine(RunCommands());
    }

    private IEnumerator RunCommands()
    {
        foreach (var cmd in commandQueue)
        {
            yield return StartCoroutine(cmd.action());
            yield return new WaitForSecondsRealtime(0.5f);
        }
        
        commandQueue.Clear();
        RefreshUI();
    }
    
    private void RefreshUI()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        int total = commandQueue.Count;
        for (int i = 0; i <= total; i++)
        {
            if (isLooping && i == loopStartIndex)
            {
                var startGO = Instantiate(itemPrefab, contentPanel);
                var startTMP = startGO.GetComponentInChildren<TextMeshProUGUI>();
                startTMP.text = $"Loop Start x{loopCount}";
            }

            if (i < total)
            {
                var cmdGO = Instantiate(itemPrefab, contentPanel);
                var cmdTMP = cmdGO.GetComponentInChildren<TextMeshProUGUI>();
                cmdTMP.text = commandQueue[i].name;
            }
        }

        if (isLooping)
        {
            var endGO = Instantiate(itemPrefab, contentPanel);
            var endTMP = endGO.GetComponentInChildren<TextMeshProUGUI>();
            endTMP.text = "Loop End";
        }
    }

    private IEnumerator LoopProcess(List<Command> innerCommands, int count)
    {
        for (int i = 0; i < count; i++)
        {
            foreach (var cmd in innerCommands)
            {
                yield return cmd.action();
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }

    private IEnumerator MoveProcess()
    {
        Vector3 startPos = player.transform.position;
        Vector3 endPos   = startPos + player.transform.forward * moveDistance;
        float   t        = 0f;

        while (t < moveDuration)
        {
            player.transform.position = Vector3.Lerp(startPos, endPos, t / moveDuration);
            t += Time.deltaTime;
            yield return null;
        }
        player.transform.position = endPos;
    }

    private IEnumerator RotateProcess(float angle)
    {
        Quaternion startRot = player.transform.rotation;
        Quaternion endRot   = startRot * Quaternion.Euler(0f, angle, 0f);
        float      t        = 0f;

        while (t < rotationDuration)
        {
            player.transform.rotation = Quaternion.Slerp(startRot, endRot, t / rotationDuration);
            t += Time.deltaTime;
            yield return null;
        }
        player.transform.rotation = endRot;
    }
}
