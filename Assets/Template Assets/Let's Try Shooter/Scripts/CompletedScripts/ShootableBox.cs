using UnityEngine;
using System.Collections;

public class ShootableBox : MonoBehaviour {

	//The box's current health point total
	public int currentHealth = 3;
    AttackBox health;

    int alienDamage = 1;


    public void Damage(int damageAmount)
	{
		//subtract damage amount when Damage function is called
		currentHealth -= damageAmount;

		//Check if health has fallen below zero
		if (currentHealth <= 0) 
		{
			//if health has fallen below zero, deactivate it 
			gameObject.SetActive (false);
		}
	}

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Shootable")
        { 
            Debug.Log("HIT");
            health = collision.gameObject.GetComponent<AttackBox>();
        
            if (health != null)
            {
                Debug.Log("Blah");
                // Call the damage function of that script, passing in our gunDamage variable
                health.Damage(alienDamage);
            }
            
        }
    }
    
	

}
