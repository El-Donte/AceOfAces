using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AceOfAces.Core.FSM;

public class StateMachine
{
    private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
    private IState _currentState;

    public Game GameEngine { get; }

    public StateMachine(Game gameEngine)
    {
        GameEngine = gameEngine;
    }

    public void Add(string stateName, IState state)
    {
        _states[stateName] = state;
    }

    public void Change(string stateName)
    {
        if (!_states.ContainsKey(stateName))
        {
            throw new KeyNotFoundException($"{stateName} нет такого состояния!");
        }

        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = _states[stateName];
        _currentState.Enter();
    }
    public void Update(float deltaTime)
    {
        if (_currentState != null)
        {
            _currentState.Update(deltaTime);
        }
    }

    public void Draw()
    {
        if (_currentState != null)
        {
            _currentState.Draw();
        }
    }
}