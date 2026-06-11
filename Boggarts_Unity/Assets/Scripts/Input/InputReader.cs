using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Input
{
    public interface IInputReader
    {
        Vector2 MoveDirection { get; }
        void EnablePlayerActions();
    }
    
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction Button_North = delegate {  };
        public UnityAction Button_East = delegate {  };
        public UnityAction Button_West = delegate {  };
        public UnityAction Button_South = delegate {  };

        private InputSystem_Actions m_inputActions;

        public Vector2 MoveDirection { get; }

        public void EnablePlayerActions()
        {
            if (m_inputActions == null)
            {
                m_inputActions = new InputSystem_Actions();
                m_inputActions.Player.SetCallbacks(this);
            }
            m_inputActions.Enable();
        }
        
        public void DisablePlayerActions()
        {
            m_inputActions.Disable();
        }

        public void OnButton_North(InputAction.CallbackContext context)
        {
            Button_North.Invoke();
        }

        public void OnButton_West(InputAction.CallbackContext context)
        {
            Button_West.Invoke();
        }

        public void OnButton_South(InputAction.CallbackContext context)
        {
            Button_South.Invoke();
        }

        public void OnButton_East(InputAction.CallbackContext context)
        {
            Button_East.Invoke();
        }
    }
}