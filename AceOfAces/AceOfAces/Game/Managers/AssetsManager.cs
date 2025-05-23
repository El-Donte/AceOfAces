using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AceOfAces.Managers;

public static class AssetsManager
{
    public static ContentManager ContentManager { get; set; }
    public static GraphicsDevice Graphics { get; set; }

    public static Texture2D PlayerTexture { get; private set; }
    public static List<Texture2D> CloudTextures { get; private set; }
    public static Texture2D MissileTexture { get; private set; }
    public static Texture2D EnemyTexture { get; private set; }
    public static Texture2D CooldownTexture { get; private set; }
    public static Texture2D PixelTexture { get; private set; }
    public static Texture2D ParticleTexture { get; private set; }
    public static SpriteFont Font { get; private set; }
    public static Texture2D ButtonTexture { get; private set; }

    public static void Initialize(ContentManager contentManager, GraphicsDevice graphicsDevice) => (ContentManager, Graphics) = (contentManager, graphicsDevice);

    public static void LoadContent()
    {
        PlayerTexture = ContentManager.Load<Texture2D>("jets/jet");

        CloudTextures = new List<Texture2D>() {
            ContentManager.Load<Texture2D>("Clouds/Clouds1"), 
            ContentManager.Load<Texture2D>("Clouds/Clouds2"), 
            ContentManager.Load<Texture2D>("Clouds/Clouds3") 
        };

        MissileTexture = ContentManager.Load<Texture2D>("missile");
        EnemyTexture = ContentManager.Load<Texture2D>("jets/jet");
        CooldownTexture = ContentManager.Load<Texture2D>("missileCooldown");

        PixelTexture = new Texture2D(Graphics, 1, 1);
        PixelTexture.SetData([Color.White]);

        ParticleTexture = ContentManager.Load<Texture2D>("particle");

        Font = ContentManager.Load<SpriteFont>("Fonts/font");

        ButtonTexture = ContentManager.Load<Texture2D>("button");
    }
}

