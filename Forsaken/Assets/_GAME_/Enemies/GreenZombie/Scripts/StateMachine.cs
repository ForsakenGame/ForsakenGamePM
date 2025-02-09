using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State initialState; // Punto de entrada del estado
    public State _currentState; // Estado actual que se ejecuta 

    // Start is called before the first frame update
    void Start()
    {
        _currentState = initialState; // Punto de entrada de la maquina de estado
    }

    // Update is called once per frame
    void Update()
    {
        State newState = _currentState.Run(gameObject);
        if (newState != _currentState)
        {
            // Significa que la accion se ha cumplido
            // y debemos de cambiar de estado
            _currentState = newState;
        }
    }
}
