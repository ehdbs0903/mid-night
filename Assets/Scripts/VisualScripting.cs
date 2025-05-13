using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualScripting : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentPanel;
    public GameObject itemPrefab;
    
    public GameObject player;

    public float moveDistance = 1f;
    public float moveDuration = 0.2f;
    
    public float rotationAngle = 90f;
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
    
    public void ExecuteCommands()
    {
        if (commandQueue.Count == 0) return;
        StartCoroutine(RunCommands());
    }

    private void RefreshUI()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var cmd in commandQueue)
        {
            var go = Instantiate(itemPrefab, contentPanel);
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = cmd.name;
        }
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

    private IEnumerator MoveProcess()
    {
        Vector3 startPos = player.transform.position;
        Vector3 endPos = startPos + player.transform.forward * moveDistance;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            player.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.position = endPos;
    }

    private IEnumerator RotateProcess(float angle)
    {
        Quaternion startRot = player.transform.rotation;
        Quaternion endRot   = startRot * Quaternion.Euler(0f, angle, 0f);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            player.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.rotation = endRot;
    }
}
