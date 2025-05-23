using Microsoft.Xna.Framework;

namespace AceOfAces.Core.FSM;


public class StateComponent : DrawableGameComponent
{
    public StateMachine StateMachine { get; }


    public StateComponent(Game game) : base(game) => StateMachine = new StateMachine(game);


    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        StateMachine.Update(deltaTime);
    }

    public override void Draw(GameTime gameTime)
    {
        StateMachine.Draw();
    }
}

