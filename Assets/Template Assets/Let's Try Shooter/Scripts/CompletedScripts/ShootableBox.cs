using UnityEngine;
using System.Collections;

public class ShootableBox : MonoBehaviour {

	//The box's current health point total
	public int currentHealth = 3;
    AttackBox health;

    int num;
    int dropNum;
    int alienDamage = 1;

    // ammo
    public GameObject drop;

    // h20
    public GameObject water;

    //c02
    public GameObject air;

    // health
    public GameObject healthDrop;

    // 3-d print?
    public GameObject coins;

    // empty drop?
    public GameObject ash;

    public void Damage(int damageAmount)
	{
		//subtract damage amount when Damage function is called
		currentHealth -= damageAmount;

		//Check if health has fallen below zero
		if (currentHealth <= 0) 
		{
            

            //if health has fallen below zero, deactivate it 
            gameObject.SetActive(false);

            // reactivate 6 seconds later
            Invoke("Show", 6);


            // drop drops
            dropNum = roll();


            // ammo
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(drop, transform.position, transform.rotation);
                drop.SetActive(true);
            }

            // water
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(water, transform.position, transform.rotation);
                water.SetActive(true);
            }

            // air
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(air, transform.position, transform.rotation);
                air.SetActive(true);
            }

            // health
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(healthDrop, transform.position, transform.rotation);
                healthDrop.SetActive(true);
            }

            // coins
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(coins, transform.position, transform.rotation);
                coins.SetActive(true);
            }


            // ash
            if (dropNum < 40 && dropNum >= 0)
            {
                Instantiate(ash, transform.position, transform.rotation);
                ash.SetActive(true);
            }


            // reset health
            currentHealth = 3;
        }
	}


    void Show()
    {
        gameObject.SetActive(true);
    }

    int roll()
    {
        num = Random.Range(0, 30);
        return num;

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







