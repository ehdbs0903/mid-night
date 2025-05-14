using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockCoding : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentPanel;
    public GameObject itemPrefab;
    public GameObject movePrefab;
    public GameObject rotateLeftPrefab;
    public GameObject rotateRightPrefab;
    public GameObject wateringPrefab;
    public GameObject harvestPrefab;
    public GameObject loopStartPrefab;
    public GameObject loopEndPrefab;
    public ParticleSystem waterParticle;
    public ParticleSystem harvestParticle;
    public TextMeshProUGUI commandCntText;

    [Header("Target")]
    public GameObject player;
    
    [Header("Move/Rotate Settings")]
    public float moveDistance     = 1f;
    public float moveDuration     = 0.2f;
    public float rotationAngle    = 90f;
    public float rotationDuration = 0.2f;

    [Header("Command Count")]
    public int optimalCommandCount = 21;
    private int currentCommandCount = 0;

    [Header("Harvest Settings")]
    [Tooltip("씬에 존재하는 총 작물(Soil) 개수")]
    public int totalCropCount = 4;
    private int harvestedCount = 0;

    [Header("Game Manager")]
    [Tooltip("GameUIManager를 인스펙터에서 연결하세요")]
    public GameUIManager gameUIManager;

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

    private List<Command> uiCommands   = new List<Command>();
    private List<Command> execCommands = new List<Command>();
    private bool isLooping             = false;
    private int  loopStartIndexExec    = 0;
    private int  loopCount             = 0;

    private void Start()
    {
        currentCommandCount = 0;
        harvestedCount      = 0;
        UpdateCommandCountUI();
    }

    private void UpdateCommandCountUI()
    {
        if (commandCntText != null)
            commandCntText.text = $"{currentCommandCount} / {optimalCommandCount}";
    }

    public void EnqueueMoveForward()
    {
        var cmd = new Command(MoveProcess, "Move Forward");
        uiCommands.Add(cmd);
        execCommands.Add(cmd);
        RefreshUI();
    }

    public void EnqueueRotateLeft()
    {
        var cmd = new Command(() => RotateProcess(-rotationAngle), "Rotate Left");
        uiCommands.Add(cmd);
        execCommands.Add(cmd);
        RefreshUI();
    }

    public void EnqueueRotateRight()
    {
        var cmd = new Command(() => RotateProcess(rotationAngle), "Rotate Right");
        uiCommands.Add(cmd);
        execCommands.Add(cmd);
        RefreshUI();
    }

    public void EnqueueWatering()
    {
        var cmd = new Command(WateringProcess, "Watering");
        uiCommands.Add(cmd);
        execCommands.Add(cmd);
        RefreshUI();
    }

    public void EnqueueHarvest()
    {
        var cmd = new Command(HarvestProcess, "Harvest");
        uiCommands.Add(cmd);
        execCommands.Add(cmd);
        RefreshUI();
    }

    public void EnqueueLoopStart()
    {
        if (!isLooping)
        {
            isLooping           = true;
            loopStartIndexExec  = execCommands.Count;
            loopCount           = 1;
            var placeholder     = new Command(NoOpProcess, "Loop Start");
            uiCommands.Add(placeholder);
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

        int innerCount = execCommands.Count - loopStartIndexExec;
        var innerExec  = execCommands.GetRange(loopStartIndexExec, innerCount);
        execCommands.RemoveRange(loopStartIndexExec, innerCount);
        var loopCmd    = new Command(() => LoopProcess(innerExec, loopCount), $"Loop x{loopCount}");
        execCommands.Add(loopCmd);
        uiCommands.Add(loopCmd);

        isLooping = false;
        RefreshUI();
    }

    public void ExecuteCommands()
    {
        if (execCommands.Count == 0) return;

        // 명령 개수 누적 및 UI 업데이트
        currentCommandCount += execCommands.Count;
        UpdateCommandCountUI();

        StartCoroutine(RunCommands());
    }

    private IEnumerator RunCommands()
    {
        foreach (var cmd in execCommands)
        {
            yield return StartCoroutine(cmd.action());
            yield return new WaitForSecondsRealtime(0.5f);
        }

        execCommands.Clear();
        uiCommands.Clear();
        isLooping = false;
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var cmd in uiCommands)
        {
            if (cmd.name == "Loop Start")
            {
                var go   = Instantiate(loopStartPrefab, contentPanel);
                var text = go.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                    text.text = $"반복 x{loopCount}";
            }
            else if (cmd.name.StartsWith("Loop x"))
            {
                Instantiate(loopEndPrefab, contentPanel);
            }
            else
            {
                Instantiate(GetPrefabForCommand(cmd.name), contentPanel);
            }
        }
    }

    private GameObject GetPrefabForCommand(string name)
    {
        switch (name)
        {
            case "Move Forward": return movePrefab;
            case "Rotate Left":  return rotateLeftPrefab;
            case "Rotate Right": return rotateRightPrefab;
            case "Watering":     return wateringPrefab;
            case "Harvest":      return harvestPrefab;
            default:             return itemPrefab;
        }
    }

    private IEnumerator NoOpProcess() { yield break; }

    private IEnumerator LoopProcess(List<Command> innerCommands, int count)
    {
        for (int i = 0; i < count; i++)
        {
            foreach (var cmd in innerCommands)
            {
                yield return StartCoroutine(cmd.action());
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

    private IEnumerator WateringProcess()
    {
        waterParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        waterParticle.Play();

        Vector3 origin = player.transform.position + Vector3.up * 0.5f;
        Vector3 dir    = -player.transform.up;
        float   maxDist= 2f;
        Debug.DrawRay(origin, dir * maxDist, Color.red, 10f);

        if (Physics.Raycast(origin, dir, out var hit, maxDist,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
        {
            var soil = hit.collider.GetComponent<Soil>();
            if (soil != null)
                soil.GrowCrop();
        }

        yield return new WaitForSecondsRealtime(0.3f);
    }

    private IEnumerator HarvestProcess()
    {
        harvestParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        harvestParticle.Play();
        
        Vector3 origin = player.transform.position + Vector3.up * 0.5f;
        Vector3 dir    = -player.transform.up;
        float   maxDist= 2f;
        
        if (Physics.Raycast(origin, dir, out var hit, maxDist,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
        {
            var soil = hit.collider.GetComponent<Soil>();
            if (soil != null)
            {
                soil.HarvestCrop();
                
                harvestedCount++;
                
                if (harvestedCount >= totalCropCount && gameUIManager != null)
                {
                    gameUIManager.StageEnd();
                }
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);
    }
}
