using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockCoding : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentPanel;
    public GameObject itemPrefab;  // 기본 prefab fallback

    public GameObject movePrefab;
    public GameObject rotateLeftPrefab;
    public GameObject rotateRightPrefab;
    public GameObject wateringPrefab;
    public GameObject harvestPrefab;
    public GameObject loopStartPrefab;
    public GameObject loopEndPrefab;
    
    public ParticleSystem waterParticle;
    public ParticleSystem harvestParticle;

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
    
    public void EnqueueWatering()
    {
        commandQueue.Add(new Command(WateringProcess, "Watering"));
        RefreshUI();
    }
    
    public void EnqueueHarvest()
    {
        commandQueue.Add(new Command(HarvestProcess, "Harvest"));
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

        // Loop Start indicator
        if (isLooping)
        {
            var startUI = Instantiate(loopStartPrefab, contentPanel);
        }

        // Commands
        foreach (var cmd in commandQueue)
        {
            var prefab = GetPrefabForCommand(cmd.name);
            var cmdGO = Instantiate(prefab, contentPanel);
        }

        // Loop End indicator
        if (isLooping)
        {
            var endUI = Instantiate(loopEndPrefab, contentPanel);
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
            default:               return itemPrefab;
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
    
    private IEnumerator WateringProcess()
    {
        waterParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        waterParticle.Play();
        // yield return new WaitUntil(() => !waterParticle.isPlaying);
        yield return new WaitForSecondsRealtime(0.3f);
    }

    private IEnumerator HarvestProcess()
    {
        harvestParticle.Play();
        // yield return new WaitUntil(() => !harvestParticle.isPlaying);
        yield return new WaitForSecondsRealtime(0.3f);
    }
}