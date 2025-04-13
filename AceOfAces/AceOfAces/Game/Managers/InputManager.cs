using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AceOfAces.Managers
{
    public static class InputManager
    {
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;
        private static Vector2 _inputDirection;
        public static Vector2 InputDirection => _inputDirection;

        public static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
            var keyboardState = Keyboard.GetState();

            _inputDirection = Vector2.Zero;

            if (IsKeyDown(Keys.W)) _inputDirection.Y++;
            if (IsKeyDown(Keys.S)) _inputDirection.Y--;
            if (IsKeyDown(Keys.A)) _inputDirection.X--;
            if (IsKeyDown(Keys.D)) _inputDirection.X++;

            if (IsKeyPressed(Keys.OemComma))
            {
                GameManager.isDebug = !GameManager.isDebug;
            }

            if (_inputDirection != Vector2.Zero)
            {
                _inputDirection.Normalize();
            }
        }

        public static bool IsKeyDown(Keys key) => _currentState.IsKeyDown(key);
        public static bool IsKeyPressed(Keys key) => _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
        private static bool IsKeyReleased(Keys key) => !_currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
    }
}
