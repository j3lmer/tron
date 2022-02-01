using UnityEngine;

namespace controllers
{
    public class SpelerController : MonoBehaviour
    {
        Speler _speler;

        KeyCode _up, _left, _down, _right;

        private void Start()
        {
            _speler = gameObject.GetComponent<Speler>();
        }   

        private void Update()
        {
            CheckInputs();       
        }

        void CheckInputs()
        {
            if (Input.GetKeyDown(_up))
            {
                if (_speler.lastdir != Vector3.down)
                {
                    _speler.directionChanger(Vector3.up);
                
                }
            }

            if (Input.GetKeyDown(_down))
            {
                if (_speler.lastdir != Vector3.up)
                {
                    _speler.directionChanger(Vector3.down);
                }
            }

            if (Input.GetKeyDown(_left))
            {
            
                if (_speler.lastdir != Vector3.right)
                {
                    _speler.directionChanger(Vector3.left);
                }
            }
            if (Input.GetKeyDown(_right))
            {
                if (_speler.lastdir != Vector3.left)
                {
                    _speler.directionChanger(Vector3.right);
                }
            }

        }


        public void SetKeyCodes(KeyCode[] keycodes)
        {
            _up = keycodes[0];
            _left = keycodes[1];
            _down = keycodes[2];
            _right = keycodes[3];
        }
    }
}
