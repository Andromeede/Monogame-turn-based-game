#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
#endregion

namespace RPGWithManagers

{
    public class MaKeyboard //keyboard control class
    {//internal variables
        public KeyboardState newKeyboard, oldKeyboard;
        public List<MaKey> pressedKeys = new List<MaKey>(), previousPressedKeys = new List<MaKey>();

        public MaKeyboard()//keyboard constructor
        {

        }

        public virtual void Update() //Updates the keyboard status 
        {
            newKeyboard = Keyboard.GetState(); //get the keyboard state 

            GetPressedKeys(); //get the pressed keys

        }

        public void UpdateOld() //update previous keyboard state
        {
            oldKeyboard = newKeyboard; //assign a new variable

            previousPressedKeys = new List<MaKey>(); //create a list of keys
            for(int i=0;i<pressedKeys.Count;i++) //loops through all the pressed keys
            {
                previousPressedKeys.Add(pressedKeys[i]); //adds them in the previous pressed list
            }
        }


        public bool GetPress(string Key) //checks if a specific key is pressed
        {

            for(int i=0;i<pressedKeys.Count;i++) //loops through all the pressed keys
            {

                if(pressedKeys[i].key == Key) //if the corresponding key is pressed
                {
                    return true;
                }

            }
            

            return false;
        }


        public virtual void GetPressedKeys() //get the different keys that are pressed
        {
            //bool found = false;

            pressedKeys.Clear(); //clears the list 
            for(int i=0; i<newKeyboard.GetPressedKeys().Length; i++) //loops through the different pressed keys on the new keyboard
            {

                    pressedKeys.Add(new MaKey(newKeyboard.GetPressedKeys()[i].ToString(), 1)); //add the pressed keys value to the list 
  
            }
        }


        public bool GetSinglePress(string Key)
        {
            for (int i = 0; i < pressedKeys.Count; i++) //loops through all the pressed keys
            {

                bool isIn = false;


                for (int j = 0; j < previousPressedKeys.Count; j++) //loops through all the pressed keys
                {

                    if (pressedKeys[i].key == previousPressedKeys[j].key) //if the corresponding key is pressed
                    {
                        isIn = true;
                        break;
                    }

                }
                if (!isIn && (pressedKeys[i].key == Key || pressedKeys[i].print == Key))
                {
                    return true;
                }
            
            }
            return false;
        }
    }
}
