using UnityEngine;
using System;

public class Health {
    private readonly uint maxHealth;
    private uint health;
    public event Action OnDeath;

    public Health(uint maxHealth) {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public bool TakeDamage(uint damage) {
        if (damage >= this.health) {
            this.health = 0;
            OnDeath?.Invoke();
            return true;
        }
        else {
            this.health -= damage;
            return false;
        }
    }

    public void Heal(uint healing) {
        if (this.health + healing > this.maxHealth) {
            this.health = this.maxHealth;
        }
        else {
            this.health += healing;
        }
    }
}
