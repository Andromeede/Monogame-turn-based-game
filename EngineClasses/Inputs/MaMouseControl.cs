#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Drawing;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
#endregion

namespace RPGWithManagers
{
    public class MaMouseControl //mouse control class
    {   //internal variables
        public bool dragging, rightDrag;

        public Vector2 newMousePos, oldMousePos, firstMousePos, newMouseAdjustedPos, systemCursorPos, screenLoc;

        public MouseState newMouse, oldMouse, firstMouse;

        public MaMouseControl() //mouse control class constructor 
        {
            dragging = false;

            newMouse = Mouse.GetState();
            oldMouse = newMouse;
            firstMouse = newMouse;

            newMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);
            oldMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);
            firstMousePos = new Vector2(newMouse.Position.X, newMouse.Position.Y);

            GetMouseAndAdjust();

            //screenLoc = new Vector2((int)(systemCursorPos.X/Globals.screenWidth), (int)(systemCursorPos.Y/Globals.screenHeight));

        }

        #region Properties

        public MouseState First
        {
            get { return firstMouse; }
        }

        public MouseState New
        {
            get { return newMouse; }
        }

        public MouseState Old
        {
            get { return oldMouse; }
        }

        #endregion

        public void Update() // update mouse function
        {
            GetMouseAndAdjust();

            //if a left click or a right click is made 
            if (newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released || newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                firstMouse = newMouse; //update mouse state
                firstMousePos = newMousePos = GetScreenPos(firstMouse); //update mouse position
            }

            
        }

        public void UpdateOld() //update old mouse state
        {
            oldMouse = newMouse; //update old mouse state 
            oldMousePos = GetScreenPos(oldMouse); //update old mouse position
        }

        public virtual float GetDistanceFromClick() //get the distance from two clics
        {
            return GlobalUtil.GetDistance(newMousePos, firstMousePos); //calculate the distance between two mouse cick
        }

        public virtual void GetMouseAndAdjust() //get current mouse state and position
        {
            newMouse = Mouse.GetState(); //get mouse state 
            newMousePos = GetScreenPos(newMouse); //get mouse position

        }

        public int GetMouseWheelChange() //get mouse wheel value to check if scrolling
        {
            return newMouse.ScrollWheelValue - oldMouse.ScrollWheelValue;
        }


        public Vector2 GetScreenPos(MouseState Mouse) //get the mouse position
        {
            Vector2 tempVec = new Vector2(Mouse.Position.X, Mouse.Position.Y); //fills the position vector with the mouse position


            return tempVec; //return the position vector
        }

        public virtual bool LeftClick() //left click function
        {   //checks if a left click occured 
            if( newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= GlobalUtil.screenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= GlobalUtil.screenHeight)
            {
                return true;
            }

            return false;
        }

        public virtual bool LeftClickHold() //checks if the left click is holded
        {
            bool holding = false;
            //checks if a left click occured and is still pressed
            if ( newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= GlobalUtil.screenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= GlobalUtil.screenHeight)
            {
                holding = true;
                //checks if the position of the mouse is different from the clicking pos
                if(Math.Abs(newMouse.Position.X - firstMouse.Position.X) > 8 || Math.Abs(newMouse.Position.Y - firstMouse.Position.Y) > 8)
                {
                    dragging = true;
                }
            }

            

            return holding;
        }

        public virtual bool LeftClickRelease() //checks if the left click has been released
        {   //if the left click has been released
            if(newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && oldMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                dragging = false;
                return true;
            }

            return false;
        }

        public virtual bool RightClick() //right click function
        {   //checks if a right click occured 
            if (newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.RightButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= GlobalUtil.screenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= GlobalUtil.screenHeight)
            {
                return true;
            }

            return false;
        }

        public virtual bool RightClickHold()//checks if the right click is holded
        {
            bool holding = false;
            //checks if the right click is holded
            if ( newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && newMouse.Position.X >= 0 && newMouse.Position.X <= GlobalUtil.screenWidth && newMouse.Position.Y >= 0 && newMouse.Position.Y <= GlobalUtil.screenHeight)
            {
                holding = true;
                //checks if the position of the mouse is different from the clicking pos
                if (Math.Abs(newMouse.Position.X - firstMouse.Position.X) > 8 || Math.Abs(newMouse.Position.Y - firstMouse.Position.Y) > 8)
                {
                    rightDrag = true;
                }
            }



            return holding;
        }

        public virtual bool RightClickRelease()//checks if the right click has been released
        {//if the right click has been released
            if ( newMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released && oldMouse.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                dragging = false;
                return true;
            }

            return false;
        }

        public void SetFirst() //not used for now
        {

        }
    }
}
