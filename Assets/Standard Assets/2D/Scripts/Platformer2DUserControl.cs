using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : NetworkBehaviour
    {
        public GameObject cratePrefab;
    
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!isLocalPlayer) return;

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CmdSpawnCrate();
            }
        }


        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;

            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }

        [Command]
        void CmdSpawnCrate()
        {
            Vector3 position = transform.position;
            float offset = transform.localScale.x/2;
            if (offset < 0)
            {
                offset *= 2;
            }
            position.x += offset;
            var crate = (GameObject)Instantiate(cratePrefab, position, transform.rotation);
            //crate.GetComponent<SpriteRenderer>().material.color = new Vector4(0.4f, 0.4f, 1.0f, 1);
            NetworkServer.Spawn(crate);
        }
    }
}
