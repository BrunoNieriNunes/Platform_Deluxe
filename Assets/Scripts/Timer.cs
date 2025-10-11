/*
using UnityEngine;
using System;
using System.Collections;

public class Timer {
    private readonly float time;
    private float counter;
    private bool autoRestart;
    private float mult;
    private bool paused = false;
    private bool isRunning = false;

    public event Action Alarm;

    public Timer (float time, bool autoRestart = true, float mult = 1f) {
        this.time = time;
        this.counter = 0f;
        this.autoRestart = autoRestart;
        this.mult = mult;
    }

    public void StartTimer () {
        if (!isRunning) {
            StartCoroutine(RunTimer());
        }
    }

    public float Mult (float newMult = -1f) {
        if (newMult > 0f) {
            this.mult = newMult;
        }
        return this.mult;
    }

    public void Pause () {
        this.paused = true;
    }

    public void Resume () {
        this.paused = false;
    }

    public bool IsRunning () {
        return this.isRunning;
    }

    private IEnumerator RunTimer () {
        while (true) {
            this.isRunning = true;
            while (this.counter > 0f) {
                if (!paused) {
                    this.counter -= Time.deltaTime * mult;
                }
                yield return null;
            }
            Alarm?.Invoke();
            if (autoRestart) {
                this.counter = this.time;
            }
            else {
                this.isRunning = false;
                break;
            }
        }
    }
}
*/