using AceOfAces.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Button
{
    private const float ScaleFactor = 6f; 

    private MouseState _currentMouseState;
    private readonly int _height = 24;
    private readonly int _width = 61;
    private bool _isHovering;
    private MouseState _previousMouseState;

    public event EventHandler Click;

    public bool Clicked { get; private set; }

    public Color PenColour { get; set; }

    public Vector2 Position { get; set; }

    public Rectangle Rectangle
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
        }
    }

    public string Text { get; set; }

    public Button(int width, int height)
    {
        _width = width;
        _height = height;
        PenColour = Color.Black;
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

        int mouseX = (int)(_currentMouseState.X / ScaleFactor);
        int mouseY = (int)(_currentMouseState.Y / ScaleFactor);
        var mouseRectangle = new Rectangle(mouseX, mouseY, 1, 1);

        _isHovering = false;

        if (mouseRectangle.Intersects(Rectangle))
        {
            _isHovering = true;

            if (_currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
            }
        }
    }
}