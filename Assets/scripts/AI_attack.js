#pragma strict

var target : Transform; //the enemy's target
var moveSpeed = 3; //move speed
var rotationSpeed = 3; //speed of turning
var attackThreshold = 3; // distance within which to attack
var chaseThreshold = 10; // distance within which to start chasing
var giveUpThreshold = 20; // distance beyond which AI gives up
var attackRepeatTime = 1; // delay between attacks when within range

private var attackTime = 0.0f;
private var chasing = false;



var myTransform : Transform; //current transform data of this enemy


function Awake()
{
    myTransform = transform; //cache transform data for easy access/preformance
}


function Start()
{
     target = GameObject.FindWithTag("Player").transform; //target the player
	 attackTime = Time.time;
}


function Update () {


	// check distance to target every frame:
	 var distance = (target.position - myTransform.position).magnitude;
	 if (chasing) {
		 //rotate to look at the player
		 myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
		 Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
		 //move towards the player
		
		myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime * 2;
		 
		 // give up, if too far away from target:
		 if (distance > giveUpThreshold) {
			 chasing = false;
		 }
		 // attack, if close enough, and if time is OK:
		 if (distance < attackThreshold && Time.time > attackTime) {
			 // Attack! (call whatever attack function you like here)
			 attackTime = Time.time + attackRepeatTime;
		 }
	 } 
	 else {
		 // not currently chasing.
		 // start chasing if target comes close enough
		 if (distance < chaseThreshold) {
			 chasing = true;
		 }
	 }
 }