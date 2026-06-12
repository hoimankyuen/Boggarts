using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Input
{
    public interface IInputReader
    {
        void EnablePlayerActions();
        void OnButton_North(InputAction.CallbackContext context);
        void OnButton_West(InputAction.CallbackContext context);
        void OnButton_South(InputAction.CallbackContext context);
        void OnButton_East(InputAction.CallbackContext context);
    }
    
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction Button_North = delegate {  };
        public UnityAction Button_East = delegate {  };
        public UnityAction Button_West = delegate {  };
        public UnityAction Button_South = delegate {  };
        public UnityAction Start = delegate {  };
        
        public UnityAction<float> Move = delegate {  };

        private InputSystem_Actions m_inputActions;

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
            Button_North?.Invoke();
        }

        public void OnButton_West(InputAction.CallbackContext context)
        {
            Button_West?.Invoke();
        }

        public void OnButton_South(InputAction.CallbackContext context)
        {
            Button_South?.Invoke();
        }

        public void OnButton_East(InputAction.CallbackContext context)
        {
            Button_East?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<float>());
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            Start?.Invoke();
        }
    }
}