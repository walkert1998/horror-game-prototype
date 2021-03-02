using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private AudioClip[] m_FootstepSoundsWood;
        [SerializeField] private AudioClip[] m_FootstepSoundsDirt;
        [SerializeField] private AudioClip[] m_FootstepSoundsMetal;
        [SerializeField] private AudioClip[] m_FootstepSoundsMud;

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        //private Stamina m_Stamina;
        public WeaponManager weaponManager;
        //public Map map;
        public bool m_CanMove = true;
        public bool m_CanLook = true;
        //public Stealth playerCrouch;
        public float footstepVolume;

       // GameSettings gameSettings;

        public MouseLook GetMouseLook()
        {
            return m_MouseLook;
        }

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            //m_Stamina = GetComponent<Stamina>();
            //gameSettings = GetComponent<GameSettings>();
        }


        // Update is called once per frame
        private void Update()
        {
            if (m_CanLook)
            {
                RotateView();
            }
            if (m_CanMove)
            {
                // the jump state needs to read here to make sure it is not missed
                if (!m_Jump)
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                }

                if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
                {
                    StartCoroutine(m_JumpBob.DoBobCycle());
                    PlayLandingSound();
                    m_MoveDir.y = 0f;
                    m_Jumping = false;
                }
                if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
                {
                    m_MoveDir.y = 0f;
                }

                if (weaponManager.aiming)
                {
                    m_WalkSpeed = 1f;
                    m_MouseLook.XSensitivity = 0.5f;
                    m_MouseLook.YSensitivity = 0.5f;
                }
                else
                {
                    m_WalkSpeed = 2f;
                    m_MouseLook.XSensitivity = 2;
                    m_MouseLook.YSensitivity = 2;
                }




                m_PreviouslyGrounded = m_CharacterController.isGrounded;
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            if (m_CanMove)
            {
                float speed;
                GetInput(out speed);
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                   m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;


                if (m_CharacterController.isGrounded)
                {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (m_Jump)
                    {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

                ProgressStepCycle(speed);
                //UpdateCameraPosition(speed);

            }
            if (m_CanLook)
            {
                m_MouseLook.UpdateCursorLock();
            }
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            RaycastHit hit = new RaycastHit();
            string floorTag;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                floorTag = hit.collider.gameObject.tag;
                if (floorTag == "wood")
                {
                    int n = Random.Range(1, m_FootstepSoundsWood.Length);
                    m_AudioSource.clip = m_FootstepSoundsWood[n];
                    m_FootstepSoundsWood[n] = m_FootstepSoundsWood[0];
                    m_FootstepSoundsWood[0] = m_AudioSource.clip;
                }
                else if (floorTag == "mud")
                {
                    int n = Random.Range(1, m_FootstepSoundsMud.Length);
                    m_AudioSource.clip = m_FootstepSoundsMud[n];
                    m_FootstepSoundsMud[n] = m_FootstepSoundsMud[0];
                    m_FootstepSoundsMud[0] = m_AudioSource.clip;
                }
                else if (floorTag == "metal")
                {
                    int n = Random.Range(1, m_FootstepSoundsMetal.Length);
                    m_AudioSource.clip = m_FootstepSoundsMetal[n];
                    m_FootstepSoundsMetal[n] = m_FootstepSoundsMetal[0];
                    m_FootstepSoundsMetal[0] = m_AudioSource.clip;
                }
                else if (floorTag == "dirt")
                {
                    int n = Random.Range(1, m_FootstepSoundsDirt.Length);
                    m_AudioSource.clip = m_FootstepSoundsDirt[n];
                    m_FootstepSoundsDirt[n] = m_FootstepSoundsDirt[0];
                    m_FootstepSoundsDirt[0] = m_AudioSource.clip;
                }
                else
                {
                    int n = Random.Range(1, m_FootstepSounds.Length);
                    m_AudioSource.clip = m_FootstepSounds[n];
                    m_FootstepSounds[n] = m_FootstepSounds[0];
                    m_FootstepSounds[0] = m_AudioSource.clip;
                }
            }
            //if (playerCrouch.IsCrouched())
            //    footstepVolume = 0.05f;
            //else if (m_IsWalking)
            //    footstepVolume = 0.2f;
            //else
                footstepVolume = 1.0f;
            m_AudioSource.PlayOneShot(m_AudioSource.clip, footstepVolume);
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                //m_Camera.transform.localPosition = m_OriginalCameraPosition;
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running

            if (!weaponManager.aiming)
            {
                m_IsWalking = !(Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0);
                //    m_UseHeadBob = (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && gameSettings.GetHeadbob());
            }

            //if (playerCrouch.IsCrouched())
            //    m_IsWalking = true;
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);
            //if (horizontal != 0 || vertical != 0)
            //{
            //    HelpText.HideHelpText();
            //}
            
            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        public bool IsRunning()
        {
            return !m_IsWalking;
        }

        public void SetRotateZ (float value, float speed)
        {
            m_MouseLook.SetRotateZ(value, speed);
        }

        public void ResetCamRotation()
        {
            m_MouseLook.ResetCamRotation();
        }
    }

