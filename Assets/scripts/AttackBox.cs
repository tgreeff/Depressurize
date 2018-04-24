using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour {

    //The box's current health point total
    public int currentHealth = 3;

    public void Damage(int damageAmount)
    {
        //subtract damage amount when Damage function is called
        currentHealth -= damageAmount;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            //if health has fallen below zero, deactivate it 
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shootable"))
        {
            Debug.Log("There was a hit :" + currentHealth);
            Damage(1);
        }
    }
}
