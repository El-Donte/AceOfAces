
namespace AceOfAces.Models;

public class MissileCooldownModel
{
    public bool AvailableToFire { get; set; } = true;
    public float Timer { get; set; }
    public float CooldownTime { get; }
    public float Progress => AvailableToFire ? 1f : 1f - (Timer / CooldownTime);

    public MissileCooldownModel(float cooldownTime)
    {
        CooldownTime = cooldownTime;
    }

    public void StartCooldown()
    {
        AvailableToFire = false;
        Timer = CooldownTime;
    }
}

