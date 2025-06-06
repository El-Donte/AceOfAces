using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Button
{
    private readonly float _scaleFactor = 6f; 
    private readonly int _height = 24;
    private readonly int _width = 61;

    private MouseState _currentMouseState;
    private MouseState _previousMouseState;
    private bool _isHovering;

    public event Action Click;

    public Vector2 Position { get; set; }

    public Rectangle Rectangle
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
        }
    }

    public Button(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var colour = Color.White * 0.0f;

        if (_isHovering)
        {
            colour = Color.Gray * 0.5f;
        }

        spriteBatch.Draw(AssetsManager.PixelTexture, Rectangle, colour);
    }

    public void Update()
    {
        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();

        int mouseX = (int)(_currentMouseState.X / _scaleFactor);
        int mouseY = (int)(_currentMouseState.Y / _scaleFactor);
        var mouseRectangle = new Rectangle(mouseX, mouseY, 1, 1);

        _isHovering = false;

        if (mouseRectangle.Intersects(Rectangle))
        {
            _isHovering = true;

            if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke();
            }
        }
    }
}