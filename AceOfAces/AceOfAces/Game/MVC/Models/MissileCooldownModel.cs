namespace AceOfAces.Models;

public class MissileCooldownModel
{
    private bool _availableToFire = true;
    public bool AvailableToFire => _availableToFire;

    private readonly float _cooldownTime;
    private float _timer;
    public float Timer 
    {
        get => _timer;
        set
        {
            _timer = value;
            if (_timer <= 0)
            {
                _availableToFire = true;
            }
        }
    }

    public float Progress => _availableToFire ? 1f : 1f - (_timer / _cooldownTime);

    public MissileCooldownModel(float cooldownTime)
    {
        _cooldownTime = cooldownTime;
    }

    public void StartCooldown()
    {
        _availableToFire = false;
        Timer = _cooldownTime;
    }
}