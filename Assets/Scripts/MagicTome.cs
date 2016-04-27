using UnityEngine;
using System.Collections;
namespace Leap.PinchUtility {

	/// <summary>
	/// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
	/// allows rotation, translation, and scale of the object (RTS).
	/// </summary>
	[ExecuteAfter(typeof(LeapPinchDetector))]
	public class MagicTome : MonoBehaviour {
		[SerializeField]
		private LeapPinchDetector _pinchDetectorLeft = null;

		[SerializeField]
		private LeapPinchDetector _pinchDetectorRight = null;

		private float _defaultNearClip;
        [Header("Shield")]
        //Stores a static gameobject used to shield the player
        public GameObject shield = null;
        [Header("Fireball")]
        //Stores the prefab of a fireball
        public GameObject fireball = null;
        //Stores the fireball speed in m/s
        public float fireballSpeed = 15.0f;
        //Stores the fireball offset in meters
        public float fireballOffset = 1;
        //Minimum and maximum fireball damage values
        public float minimumFireballDamage = 10f;
        public float maximumFireballDamage = 100f;
        //Fireball charge speed (charge/sec)
        public float fireballChargeSpeed = 50f;
        //Used for translating fireball damage to a particle effect size
        public float maximumFireballSize = 2.0f;
        //Hand objects (for position tracking)
        public GameObject leftFireSpawn = null;
        public GameObject rightFireSpawn = null;
        //Sounds used in casting
        public AudioClip castingSound;
        public AudioClip firingSound;

		//Hand states
        private HandState leftHandState = HandState.idle;
        private HandState rightHandState = HandState.idle;
        //Particle systems
        private ParticleSystem leftPalmParticles;
        private ParticleSystem rightPalmParticles;
        //Emitters
        private ParticleSystem.EmissionModule leftPalmEmission;
        private ParticleSystem.EmissionModule rightPalmEmission;
        //Audio controllers
        private AudioSource leftAudioSource;
        private AudioSource rightAudioSource;
        //Fireball charge
        private float leftFireballCharge = 0;
        private float rightFireballCharge = 0;

		//This type is used to store the state of the hands
		private enum HandState{
			idle,
			castingSpell,
            startCastingShield,
			castingShield
		}


		void Awake() {
            //Check for pinch detectors
			if (_pinchDetectorLeft == null || _pinchDetectorRight == null) {
				Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
				enabled = false;
			}
            //Get the particle systems
            leftPalmParticles = leftFireSpawn.GetComponent<ParticleSystem>();
            rightPalmParticles = rightFireSpawn.GetComponent<ParticleSystem>();
            //Disable the hand particles
            leftPalmEmission = leftPalmParticles.emission;
            rightPalmEmission = rightPalmParticles.emission;
            leftPalmEmission.enabled = false;
            rightPalmEmission.enabled = false;
			//Disable the shield
            shield.SetActive(false);
            //Get the audio emitters
            leftAudioSource = leftFireSpawn.GetComponent<AudioSource>();
            leftAudioSource.clip = castingSound;
            leftAudioSource.loop = true;
            //Get the audio emitters
            rightAudioSource = rightFireSpawn.GetComponent<AudioSource>();
            rightAudioSource.clip = castingSound;
            rightAudioSource.loop = true;
		}

		void Update() {
            //Process hand A
			switch(leftHandState)
			{
               
                case HandState.idle:
                    //Check if fist is closed
                    if (_pinchDetectorLeft.IsMakingFist)
                    {
                        //Set the fireball size
                        leftFireballCharge = minimumFireballDamage;
                        leftPalmParticles.startSize = (minimumFireballDamage / maximumFireballDamage) * maximumFireballSize;
                        //Enable the particles
                        leftPalmEmission.enabled = true;
                        //Start playing the casting sound
                        leftAudioSource.Play();
                        //Switch the state
                        leftHandState = HandState.castingSpell;
                        break; //Fist overrides pinch
                    }
                    break;
                //Actions for this will be processed in the right hand
                case HandState.startCastingShield:
                    break;
                //Actions for this will be processed in the right hand
                case HandState.castingShield:
                    break;
                case HandState.castingSpell:
                    //Increase the charge
                    leftFireballCharge = Mathf.Clamp(leftFireballCharge + fireballChargeSpeed * Time.deltaTime,minimumFireballDamage, maximumFireballDamage);
                    //Apply the charge to the fireball
                    leftPalmParticles.startSize = (leftFireballCharge / maximumFireballDamage) * maximumFireballSize;
                    //Wait until hand opens
                    if (!_pinchDetectorLeft.IsMakingFist)
                    {
                        //Stop the fireball effect
                        leftPalmEmission.enabled = false;
                        //Stop playing the casting sound
                        leftAudioSource.Stop();
                        //Play the firing sound
                        leftAudioSource.PlayOneShot(firingSound);
                        GameObject newFireball = Instantiate(fireball, leftFireSpawn.transform.position + fireballOffset * Camera.main.transform.forward, Camera.main.transform.rotation) as GameObject;
                        newFireball.GetComponent<Rigidbody>().velocity = fireballSpeed * newFireball.transform.forward;
                        newFireball.GetComponent<Fireball>().damageValue = leftFireballCharge;
                        newFireball.GetComponent<ParticleSystem>().startSize = leftPalmParticles.startSize;
                        //Set state back to idle
                        leftHandState = HandState.idle;
                    }
                    break;
			}
            //Process hand B
            switch(rightHandState)
            {

                case HandState.idle:
                    //Check if fist is closed
                    if (_pinchDetectorRight.IsMakingFist)
                    {
                        //Set the fireball size
                        rightFireballCharge = minimumFireballDamage;
                        //Set the inital fireball size
                        rightPalmParticles.startSize = (minimumFireballDamage / maximumFireballDamage) * maximumFireballSize;
                        //Enable the fireball
                        rightPalmEmission.enabled = true;
                        //Start playing the casting sound
                        rightAudioSource.Play();
                        //Switch the state
                        rightHandState = HandState.castingSpell;
                        break; //Fist overrides pinche fist overrides pinch
                    }
                    //Check if both hands want to cast shield
                    if (leftHandState != HandState.castingSpell && _pinchDetectorLeft.IsPinching && _pinchDetectorRight.IsPinching)
                    {
                        //Change state of both hands
                        leftHandState = HandState.startCastingShield;
                        rightHandState = HandState.startCastingShield;
                    }
                    break;
                //Case for making the shield appear
                case HandState.startCastingShield:
                    shield.SetActive(true);
                    //Change state of both hands
                    leftHandState = HandState.castingShield;
                    rightHandState = HandState.castingShield;
                    break;
                //State for currently casting the shield
                case HandState.castingShield:
                    //Continuation of shield state depends on both hands
                    if (_pinchDetectorLeft.IsPinching && _pinchDetectorRight.IsPinching)
                    {
                        shield.transform.position = (0.5f * (_pinchDetectorLeft.Position + _pinchDetectorRight.Position)) + (0.2f * Camera.main.transform.forward);
                        shield.transform.rotation = Camera.main.transform.rotation;
                    }
                    else
                    {
                        shield.SetActive(false);
                        //Change state of both hands
                        leftHandState = HandState.idle;
                        rightHandState = HandState.idle;
                    }
                    break;
                case HandState.castingSpell:
                    //Increase the charge
                    rightFireballCharge = Mathf.Clamp(rightFireballCharge + fireballChargeSpeed * Time.deltaTime,minimumFireballDamage, maximumFireballDamage);
                    //Apply the charge to the fireball
                    rightPalmParticles.startSize = (rightFireballCharge / maximumFireballDamage) * maximumFireballSize;
                    //Wait until hand opens
                    if (!_pinchDetectorRight.IsMakingFist)
                    {
                        //Stop the fireball effect
                        rightPalmEmission.enabled = false;
                        //Stop playing the casting sound
                        rightAudioSource.Stop();
                        //Play the firing sound
                        rightAudioSource.PlayOneShot(firingSound);
                        GameObject newFireball = Instantiate(fireball, rightFireSpawn.transform.position + fireballOffset * Camera.main.transform.forward, Camera.main.transform.rotation) as GameObject;
                        newFireball.GetComponent<Rigidbody>().velocity = fireballSpeed * newFireball.transform.forward;
                        newFireball.GetComponent<Fireball>().damageValue = rightFireballCharge;
                        newFireball.GetComponent<ParticleSystem>().startSize = rightPalmParticles.startSize;
                        //Set state back to idle
                        rightHandState = HandState.idle;
                    }
                    break;
            }



		}
            
	}

}
