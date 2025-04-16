namespace AceOfAces.Models
{
    public class MissileCooldownModel
    {
        public bool Available { get; set; } = true;
        public float Timer { get; set; }
        public float CooldownTime { get; }
        public float Progress => Available ? 1f : 1f - (Timer / CooldownTime);

        public MissileCooldownModel(float cooldownTime)
        {
            CooldownTime = cooldownTime;
        }

        public void StartCooldown()
        {
            Available = false;
            Timer = CooldownTime;
        }
    }
}
