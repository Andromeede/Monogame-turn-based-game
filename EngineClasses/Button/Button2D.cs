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
using System.Text.RegularExpressions;
using FontStashSharp;

namespace RPGWithManagers
{
    public class Button2D
    {
        private bool isPressed, isHovered, active;
        private string text;

        private Color curentColor,hoverColor;
        private Texture2D texture;
        private SpriteFontBase font;

        private Vector2 pos,dims;

        private object content;
        private SendFunction buttonFunction;
        private SendObject buttonFunctionArg;

        #region Accessors
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Vector2 Dims
        {
            get { return dims; }
        }

        public SpriteFontBase Font
        {
            get { return font; }
        }

        public string Text
        {
            get { return text; }
        }

        public Color Color
        { 
            get { return curentColor; } 
        }

        #endregion

        public Button2D(Texture2D Texture, Vector2 Pos, Vector2 Dims , SpriteFontBase Font, string Text, SendObject ButtonFunction, object Content) 
        {
            active = true;

            texture = Texture;
            font = Font;
           
            pos = Pos;
            dims = Dims;
            text = Text;

            buttonFunctionArg = ButtonFunction;
            content = Content;

            isPressed = false;
            curentColor = Color.White;
            hoverColor = new Color(200, 230, 255); //light blue
        }

        public Button2D(Texture2D Texture, Vector2 Pos, Vector2 Dims, SpriteFontBase Font, string Text, SendFunction ButtonFunction)
        {
            active = true;

            texture = Texture;
            font = Font;

            pos = Pos;
            dims = Dims;
            text = Text;

            buttonFunction = ButtonFunction;

            isPressed = false;
            curentColor = Color.White;
            hoverColor = new Color(200, 230, 255); //light blue
        }

        public virtual void Update() //update button function at a given offset position (position of the button)
        {          
            if (active)
            {
                if (GlobalUtil.Hover(pos, dims)) //if button is hovered
                {
                    isHovered = true;
                    curentColor = hoverColor;

                    if (GlobalUtil.mouse.LeftClick()) //if left click pressed
                    {
                        //isHovered = false;
                        isPressed = true;
                        curentColor = Color.Gray;
                    }
                    else if (GlobalUtil.mouse.LeftClickRelease()) //if left click released
                    {
                        RunBtnClick();     //calls the run button click function, can be overriden                  
                    }
                }
                else
                {
                    isHovered = false;
                    curentColor = Color.White;
                }

                if (!GlobalUtil.mouse.LeftClick() && !GlobalUtil.mouse.LeftClickHold()) //if left click is not pressed and not held
                {
                    isPressed = false;
                }
            }
            else
            {
                curentColor = Color.Gray;
            }           
        }



        public virtual void Reset() //reset button
        {
            isPressed = false;
        }

        public virtual void RunBtnClick() //button activation 
        {
            if (buttonFunctionArg != null && content != null) //if the button clicked is not null
            {
                buttonFunctionArg(content); //calls the SendObject with content as a parameter
            }
            else
            {
                buttonFunction();
            }

            Reset(); //reset button after sending the command
        }
    }
}
