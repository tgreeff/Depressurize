 #pragma strict
 
 public var target : Transform;  
 public var start : Transform;   
 public var projectile : Rigidbody;

 public var power : float = 1600;
 
 public var maximumLookDistance : float = 30;
 public var maximumAttackDistance : float = 10;
 public var minimumDistanceFromPlayer : float = 2;
 
 public var rotationDamping : float = 2;
 
 public var shotInterval : float = 0.5;
 
 private var shotTime : float = 0;
 
 function Update()
 {
     var distance = Vector3.Distance(target.position, transform.position);
 
     if(distance <= maximumLookDistance)
     {
         LookAtTarget();
 
         //Check distance and time
         if(distance <= maximumAttackDistance && (Time.time - shotTime) > shotInterval)
         {
             Shoot();
         }
     }   
 }
 
 
 function LookAtTarget()
 {
     var dir = target.position - transform.position;
     dir.y = 0;
     var rotation = Quaternion.LookRotation(dir);
     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
 }
 
 
 function Shoot()
 {
     //Reset the time when we shoot
     shotTime = Time.time;
	 var clone : Rigidbody;
     clone = Instantiate(projectile, start.position + (target.position - transform.position).normalized, Quaternion.LookRotation(target.position - transform.position));
	 var fwd: Vector3 = transform.TransformDirection(Vector3.forward);
	 clone.AddForce(fwd * power);
 }