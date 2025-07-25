using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Reflection.Metadata;
using System;
using System.Reflection;

namespace RPGWithManagers.EngineClasses
{
    public class GameDrawer
    {
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;

        public GameDrawer(GraphicsDevice Device) 
        {
            device = Device;
            spriteBatch = new SpriteBatch(device);
        }

        public void Begin()
        {                  
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

        public void Begin(Camera Camera)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect : SetCameraEffects(Camera));
        }

        public void Draw(Sprite2D DrawElement, Vector2 Pos, Vector2 Dims) // Draw with a Sprite2D, right now it is similar to a normal Texture2D but it will change
        {
            Vector2 origin = new Vector2(DrawElement.Texture.Bounds.Width / 2, DrawElement.Texture.Bounds.Height / 2);

            spriteBatch.Draw(
                DrawElement.Texture, 
                new Rectangle((int)Pos.X, (int)Pos.Y, (int)Dims.X, (int)Dims.Y),
                null,
                Color.White,
                0.0f,
                origin,
                new SpriteEffects(),
                0);
        }

        public void Draw(Texture2D DrawElement, Vector2 Pos, Vector2 Dims) // Draw with a Texture2D, for simple objects
        {           
            spriteBatch.Draw(
                DrawElement,
                new Rectangle((int)Pos.X, (int)Pos.Y, (int)Dims.X, (int)Dims.Y),
                null,
                Color.White,
                0.0f,
                new Vector2(DrawElement.Bounds.Width / 2, DrawElement.Bounds.Height / 2),
                new SpriteEffects(),
                0);
        }

        public void DrawQuantityDisplayBar(Unit Unit)
        {
            int BarWidth = (int)(Unit.HpBar.Dims.X * Unit.HpBar.Size) - Unit.HpBar.Border * 2;
            int BarHeight = (int)Unit.HpBar.Dims.Y - Unit.HpBar.Border * 2;

            Vector2 BarBKGPos = new Vector2(Unit.Pos.X - Unit.HpBar.Dims.X / 2, Unit.Pos.Y + 45);
            Vector2 BarPos = new Vector2(Unit.Pos.X - Unit.HpBar.Dims.X / 2 + Unit.HpBar.Border, Unit.Pos.Y + Unit.HpBar.Border + 45);

            spriteBatch.Draw(
               Unit.BarBKG.Texture,
               new Rectangle((int)BarBKGPos.X, (int)BarBKGPos.Y, (int)Unit.HpBar.Dims.X, (int)Unit.HpBar.Dims.Y),
               null,
               Color.White,
               0.0f,
               new Vector2(0, 0),
               new SpriteEffects(),
               0);

            spriteBatch.Draw(
            Unit.Bar.Texture,
                new Rectangle((int)BarPos.X, (int)BarPos.Y, BarWidth, BarHeight),
                null,
                Unit.HpBar.Color,
                0.0f,
                new Vector2(0,0),
                new SpriteEffects(),
                0);          
        }
        public void End()
        {
            spriteBatch.End();
        }

        public BasicEffect SetCameraEffects(Camera Camera)
        {
            BasicEffect CameraEffect = new BasicEffect(device);
            CameraEffect.FogEnabled = false;
            CameraEffect.TextureEnabled = true;
            CameraEffect.LightingEnabled = false;
            CameraEffect.VertexColorEnabled = true;
            CameraEffect.World = Matrix.Identity;
            CameraEffect.View = Camera.RotaView;
            CameraEffect.Projection = Camera.Projection;

            return CameraEffect;
        }
    }
}
