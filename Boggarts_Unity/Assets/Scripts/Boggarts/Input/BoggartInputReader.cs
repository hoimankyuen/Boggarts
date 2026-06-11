using Input;
using UnityEngine;

public class BoggartInputReader : MonoBehaviour
{
    public InputReader inputReader;

    private void Start()
    {
        inputReader.EnablePlayerActions();

        inputReader.Button_North += OnButtonNorth;
        inputReader.Button_East += OnButtonEast;
        inputReader.Button_West += OnButtonWest;
        inputReader.Button_South += OnButtonSouth;
    }

    private void OnButtonNorth()
    {
        Debug.Log("OnButtonNorth");
    }

    private void OnButtonEast()
    {
        Debug.Log("OnButtonEast");
    }

    private void OnButtonWest()
    {
        Debug.Log("OnButtonWest");
    }

    private void OnButtonSouth()
    {
        Debug.Log("OnButtonSouth");
    }
}
