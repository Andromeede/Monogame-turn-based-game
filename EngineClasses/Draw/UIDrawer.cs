using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using FontStashSharp;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Reflection.Metadata;
using System;


namespace RPGWithManagers
{
    public class UIDrawer
    {
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;

        private FontSystem fontSystem;

        public UIDrawer(GraphicsDevice Device) 
        {
            device = Device;
            spriteBatch = new SpriteBatch(device);

            fontSystem = new FontSystem();
        }

        public void Begin()
        {
            ResetCamereffects();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend) ;
        }

        public void Draw(Sprite2D DrawElement, Vector2 Pos, Vector2 Dims, Color Color, float Opacity = 1.0f)
        {

            Vector2 origin = new Vector2(DrawElement.Texture.Bounds.Width / 2, DrawElement.Texture.Bounds.Height / 2);

            spriteBatch.Draw(
                DrawElement.Texture,
                new Rectangle((int)Pos.X, (int)Pos.Y, (int)Dims.X, (int)Dims.Y),
                null,
                Color * Opacity,
                0.0f,
                origin,
                new SpriteEffects(),
                0);
        }

        public void Draw(Texture2D DrawElement, Vector2 Pos, Vector2 Dims, Color Color) // Draw with a Texture2D, for simple objects
        {
            spriteBatch.Draw(
                DrawElement,
                new Rectangle((int)Pos.X, (int)Pos.Y, (int)Dims.X, (int)Dims.Y),
                null,
                Color,
                0.0f,
                new Vector2(DrawElement.Bounds.Width / 2, DrawElement.Bounds.Height / 2),
                new SpriteEffects(),
                0);
        }

        public void DrawButton(Button2D Button)
        {
            Draw(Button.Texture, Button.Pos, Button.Dims, Button.Color);
            Vector2 size = Button.Font.MeasureString(Button.Text);
            DrawString(Button.Font, Button.Text, Button.Pos - size/2, Color.Black);
        }

        public void DrawInventory(Inventory Inventory)
        {
            for (int i = 0; i < Inventory.InventorySlots.Count; i++)
            {
                Draw(Inventory.InventorySlots[i].Sprite, Inventory.InventorySlots[i].Pos, Inventory.InventorySlots[i].Dims, Color.Gray);
            }
        }

        public void DrawSkillButton(SkillButton SkillButton)
        {
            Draw(SkillButton.Texture, SkillButton.Pos, SkillButton.Dims, SkillButton.Color);
            Vector2 size = SkillButton.Font.MeasureString(SkillButton.Text);
            DrawString(SkillButton.Font, SkillButton.Text, SkillButton.Pos - size / 2, Color.Black);
            Draw(SkillButton.ButtonSkill.Icon, SkillButton.Pos, SkillButton.Dims, SkillButton.Color);
        }

        public void DrawString(SpriteFontBase font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position, color);
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

        public void ResetCamereffects()
        {
            BasicEffect Effect = new BasicEffect(device);
            Effect.FogEnabled = false;
            Effect.TextureEnabled = true;
            Effect.LightingEnabled = false;
            Effect.VertexColorEnabled = true;
            Effect.World = Matrix.Identity;
            Effect.View = Matrix.Identity;
            Effect.Projection = Matrix.Identity;
        }
    }
}
