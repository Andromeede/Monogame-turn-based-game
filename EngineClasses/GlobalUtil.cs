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
    public delegate void SendFunction(); // send a function without any arguments
    public delegate void SendObject(object i); //send a function with one argument that can be anything
    
    public class GlobalUtil
    {
        public static int screenHeight, screenWidth;

        public static MaKeyboard keyboard;//keyboard
        public static MaMouseControl mouse;//mouse

        public static GameTime gameTime;//gametime

        public static System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fr-FR");//"en-US"

        public static FontSystem globalUtilFontSystem;

        public static int ConvertToInt(object Info)
        {
            return Convert.ToInt32(Info, culture);
        }

        public static string ConvertToString(object Info)
        {
            return Convert.ToString(Info, culture);
        }

        public static float ConvertToFloat(object Info)
        {
            return Convert.ToUInt32(Info, culture);
        }
        public static float GetDistance(Vector2 pos, Vector2 target) //returns the distance between two points
        {
            return (float)Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
        }

        public static SpriteFontBase GetFontArial18()
        {
            globalUtilFontSystem = new FontSystem();

            globalUtilFontSystem.AddFont(File.ReadAllBytes("Content/Fonts/arial.ttf")); //C:\Windows\Fonts  

            SpriteFontBase font18 = globalUtilFontSystem.GetFont(18);

            return font18;
        }

        public static bool Hover(Vector2 pos, Vector2 dims)
        {
            Vector2 mousePos = new Vector2(GlobalUtil.mouse.newMousePos.X, GlobalUtil.mouse.newMousePos.Y);

            if (mousePos.X >= pos.X - dims.X / 2 && mousePos.X <= pos.X + dims.X / 2 && mousePos.Y >= pos.Y - dims.Y / 2 && mousePos.Y <= pos.Y + dims.Y / 2)
            {
                return true;
            }

            return false;
        }
        public static Vector2 RadialMovement(Vector2 focus, Vector2 pos, float speed) //returns the direction vector between two points and a given speed
        {
            float dist = GetDistance(pos, focus); //get the distance from the two points

            if (dist <= speed) //if the distance is smaller than the speed
            {
                return focus - pos; //returns a vector which is difference between the two positions vectors
            }
            else
            {
                return (focus - pos) * speed / dist; //returns a vector which is the reachable position vector 
            }                                        //according to the total distance between the two points and the speed 
        }                                            //of the given object

        public static float RadialAbsFloat(float focus, float current, float speed)
        {
            float diff = Math.Abs(current - focus);

            if (diff <= speed)
            {
                return focus - current;
            }
            else
            {
                return (focus - current) * speed / diff;
            }
        }

        public static float RadialFloat(float focus, float current, float speed)
        {
            float diff = current - focus;

            if (diff <= speed)
            {
                return focus - current;
            }
            else
            {
                return (focus - current) * speed / diff;
            }
        }

        public static float RotateTowards(Vector2 Pos, Vector2 focus) //returns an angle from two points
        {

            float h, sineTheta, angle;
            if (Pos.Y - focus.Y != 0) //Checks if the both points are no aligned on the y-axis
            {
                h = GetDistance(Pos, focus); //get the distance from the two points
                sineTheta = (float)(Math.Abs(Pos.Y - focus.Y) / h); //gets the sin(theta) value from the absolute position divided by
            }                                                       //the length to scale it as 1 on the trigonometric circle
            else
            {
                h = Pos.X - focus.X;
                sineTheta = 0;
            }

            angle = (float)Math.Asin(sineTheta); //gets the angle from the arcsine function

            // Drawing diagonial lines here.
            //Quadrant 2
            if (Pos.X - focus.X > 0 && Pos.Y - focus.Y > 0) //checks that the focus point is on the top left of the base one
            {
                angle = (float)(Math.PI * 3 / 2 + angle); //returns the corrected angle
            }
            //Quadrant 3
            else if (Pos.X - focus.X > 0 && Pos.Y - focus.Y < 0) //checks that the focus point is on the bottom left of the base one
            {
                angle = (float)(Math.PI * 3 / 2 - angle);//get the corrected angle
            }
            //Quadrant 1
            else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y > 0) //checks that the focus point is on the top right of the base one
            {
                angle = (float)(Math.PI / 2 - angle);//get the corrected angle
            }
            //Quadrant 4
            else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y < 0)// checks that the focus point is on the bottom right of the base one
            {
                angle = (float)(Math.PI / 2 + angle);//get the corrected angle
            }
            //both points on the same X-axis wih target point on the left side of base point
            else if (Pos.X - focus.X > 0 && Pos.Y - focus.Y == 0)
            {
                angle = (float)Math.PI * 3 / 2;//get the corrected angle
            }
            //both points on the same X-axis wih target point on the right side of base point
            else if (Pos.X - focus.X < 0 && Pos.Y - focus.Y == 0)
            {
                angle = (float)Math.PI / 2;//get the corrected angle
            }
            //both points on the same Y-axis wih target point is under the base point
            else if (Pos.X - focus.X == 0 && Pos.Y - focus.Y > 0)
            {
                angle = (float)0;//get the corrected angle
            }
            //both points on the same Y-axis wih target point is above the base point
            else if (Pos.X - focus.X == 0 && Pos.Y - focus.Y < 0)
            {
                angle = (float)Math.PI;//get the corrected angle
            }

            return angle; //returns the angle between the two points

        }
    }
}

