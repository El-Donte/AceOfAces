using AceOfAces.Core;
using AceOfAces.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AceOfAces.Views
{
    public class CooldownBarView : IView
    {
        private readonly Texture2D _texture;
        private readonly int _width;
        private readonly int _height;
        private readonly MissileCooldownModel _model;
        private readonly Vector2 _screenMargin;

        public SpriteBatch SpriteBatch { get; set; }
        public Camera2d Camera { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public CooldownBarView(MissileCooldownModel model,Texture2D texture,Vector2 screenMargin)
        {
            _model = model;
            _texture = texture;
            _width = texture.Width;
            _height = texture.Height;
            _screenMargin = screenMargin;
        }

        public void Draw()
        {
            Vector2 screenPosition = new Vector2(
                GraphicsDevice.Viewport.Width - _width - _screenMargin.X,
                GraphicsDevice.Viewport.Height - _height - _screenMargin.Y
            );

            Matrix invertedMatrix = Matrix.Invert(Camera.TransformMatrix);
            screenPosition = Vector2.Transform(screenPosition, invertedMatrix);

            SpriteBatch.Draw(_texture, screenPosition, null, Color.White * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None,0f);

            int fillHeight = (int)(_height * _model.Progress);
            Rectangle sourceRect = new Rectangle(
                0,
                _height - fillHeight, 
                _width,
                fillHeight
            );

            Vector2 fillPosition = new Vector2(
                screenPosition.X,
                screenPosition.Y + (_height - fillHeight)
            );

            SpriteBatch.Draw(_texture, fillPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}