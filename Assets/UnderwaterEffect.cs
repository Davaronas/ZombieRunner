using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using PostProcessingProfile = UnityEngine.PostProcessing.PostProcessingProfile;
// using UnityEngine.Rendering.PostProcessing;





public class UnderwaterEffect : MonoBehaviour
{

   // [SerializeField] GameObject underWaterImage;
   
    PostProcessingBehaviour[] postProcessingBehaviours = new PostProcessingBehaviour[2];
    public PostProcessingProfile normalProfile, underwaterProfile;
    playerBehavior player_behav;
    [SerializeField]GameObject mainCamera;
    [SerializeField]GameObject airMeter;
    [SerializeField] GameObject crouchImage;
    AudioSource swimmingAudio;
    AudioReverbZone underWaterAudioZone;

        
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera");
        player_behav = FindObjectOfType<playerBehavior>();
        swimmingAudio = GetComponent<AudioSource>();
        postProcessingBehaviours = Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>();
        underWaterAudioZone = GetComponent<AudioReverbZone>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            player_behav.runAudio.mute = true;
        }

        if(other.gameObject.tag == "WaterDetection")
        {
            Transform player = other.transform.parent;
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
            player_behav.notAbleToCrouch = true;
            player_behav.runAudio.Pause();
            if (player_behav.crouching == true)
            {
                player.GetComponent<CapsuleCollider>().height = 1.6f;
                player.GetComponent<CapsuleCollider>().radius = 0.3f;
                player.GetComponent<playerBehavior>().staminaKeyPressed = false;
                player.GetComponent<playerBehavior>().crouching = false;
                crouchImage.SetActive(false);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 0.3f, mainCamera.transform.position.z);
            }

        }

        if(other.gameObject.tag == "MainCamera")
        {
            //underWaterImage.SetActive(true);
            foreach (PostProcessingBehaviour postProcessingBehaviour in postProcessingBehaviours)
            postProcessingBehaviour.profile = underwaterProfile;
            airMeter.SetActive(true);
            underWaterAudioZone.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        

        if (other.gameObject.tag == "WaterDetection")
        {
            Rigidbody playerRigidbody = other.transform.parent.GetComponent<Rigidbody>();
            player_behav.isJumping = true;
            Forward_Backward_Movement(other,playerRigidbody);
            Sideway_Movement(other,playerRigidbody);
           

            //   Downward_Movement(other);
            //  Upward_Movement(other);


        }
    }

    private  void Sideway_Movement(Collider other, Rigidbody playerRigidbody)
    {
        if (Input.GetAxis("Horizontal") > 0/*&& other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude < 2f*/)
        {
            // other.transform.parent.GetComponent<Rigidbody>().AddForce(other.transform.right * 10f);
            playerRigidbody.velocity = mainCamera.transform.right; //* 0.02f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else
        if (Input.GetAxis("Horizontal") < 0 /*&& other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude < 2f*/)
        {
            //  other.transform.parent.GetComponent<Rigidbody>().AddForce(-other.transform.right * 10f);
            playerRigidbody.velocity = -mainCamera.transform.right; //* 0.02f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            playerRigidbody.velocity = new Vector3
               (0f, 0f, 0f);

            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
                swimmingAudio.Stop();
        }
    }

    private  void Upward_Movement(Collider other)
    {
        if (Input.GetButton("Jump") /*&& other.transform.parent.GetComponent<Rigidbody>().velocity.y > -2f*/)
        {
            // other.transform.parent.GetComponent<Rigidbody>().AddForce(other.transform.up * 20f);
            other.transform.parent.GetComponent<Rigidbody>().velocity = other.transform.up;//* 0.02f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            other.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(other.transform.parent.GetComponent<Rigidbody>().velocity.x, 0f, other.transform.parent.GetComponent<Rigidbody>().velocity.z);
           
        }
    }

    private  void Downward_Movement(Collider other)
    {
        if (Input.GetButton("Crouch") /*&& other.transform.parent.GetComponent<Rigidbody>().velocity.y > -2f*/)
        {
            // other.transform.parent.GetComponent<Rigidbody>().AddForce(-other.transform.up * 20f);
            other.transform.parent.GetComponent<Rigidbody>().velocity = -other.transform.up; //* 0.02f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            other.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(other.transform.parent.GetComponent<Rigidbody>().velocity.x, 0f, other.transform.parent.GetComponent<Rigidbody>().velocity.z);

        }
    }

    private  void Forward_Backward_Movement(Collider other, Rigidbody playerRigidbody)
    {
        if (Input.GetAxis("Vertical") > 0 /*&& other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude < 2f*/)
        {
            // other.transform.parent.GetComponent<Rigidbody>().AddForce(other.transform.forward * 20f);
            playerRigidbody.velocity = mainCamera.transform.forward*1.5f; //* 0.05f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else
        if (Input.GetAxis("Vertical") < 0 /*&& other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude < 2f*/)
        {
            // other.transform.parent.GetComponent<Rigidbody>().AddForce(-other.transform.forward * 10f);

            playerRigidbody.velocity = -mainCamera.transform.forward; // * 0.02f;
            if (swimmingAudio.isPlaying == false)
            {
                swimmingAudio.Play();
            }
        }
        else if (Input.GetAxis("Vertical") == 0  && Input.GetAxis("Horizontal") == 0)
        {
           playerRigidbody.velocity = new Vector3
                (0f, 0f, 0f);

            if (Input.GetAxis("Vertical") == 0  && Input.GetAxis("Horizontal") == 0)
                swimmingAudio.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player_behav.runAudio.mute = false;
        }

        if (other.gameObject.tag == "WaterDetection")
        {
           
            other.transform.parent.GetComponent<Rigidbody>().useGravity = true;
            player_behav.isJumping = false;
            player_behav.notAbleToCrouch = false;
            swimmingAudio.Stop();
        }

        if (other.gameObject.tag == "MainCamera")
        {
            // underWaterImage.SetActive(false);
            airMeter.SetActive(false);
            foreach (PostProcessingBehaviour postProcessingBehaviour in postProcessingBehaviours)
                postProcessingBehaviour.profile = normalProfile;
            underWaterAudioZone.enabled = false;
        }
    }


   
         
}
