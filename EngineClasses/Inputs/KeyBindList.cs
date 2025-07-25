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
using System.Xml.Linq;
#endregion

namespace RPGWithManagers
{
    public class KeyBindList
    {

        public List<KeyBind> keyBindList = new List<KeyBind>();

        public KeyBindList(XDocument Xml) 
        {
            List<XElement> bindsXml = (from t in Xml.Descendants("Key") select t).ToList<XElement>(); //retrieves the XElement of every desendants of "Key" attribute

            for (int i = 0; i < bindsXml.Count; i++) //loops through the XElements
            {
                keyBindList.Add(new KeyBind(bindsXml[i].Attribute("n").Value, bindsXml[i].Element("value").Value)); //adds the XElements into a list of Keybinds (attribute and value)
            }      
        }

        public virtual string GetKeyByName(string Name)
        {
            for (int i = 0; i < keyBindList.Count; i++) //loops through the mist of keybinds
            {
                if (keyBindList[i].name == Name) //if one name in the list matches the input name
                {
                    return keyBindList[i].key; //returns the associated key name to the keybind
                }               
            }
            return "";
        }

        public virtual KeyBind GetKeyBindByName(string Name) //returns the complete keybinds with a string as an input
        {
            for (int i = 0; i < keyBindList.Count; i++) //loops through the mist of keybinds
            {
                if (keyBindList[i].name == Name) //if one name in the list matches the input name
                {
                    return keyBindList[i]; //returns the complete keybind (name + associated key)
                }
            }
            return null;
        }

        public virtual XElement ReturnXML() //returns an XElement in order to be stored in an XML file
        {
            XElement xml = new XElement("Keys", ""); //creates a new Xelement

            for (int i = 0; i< keyBindList.Count; i++) //loops through the mist of keybinds
            {
                xml.Add(keyBindList[i].ReturnXML()); //add the XElement of every KeyBind
            }

            return xml; //return the XElement with all the keybind list stored as an XElement
        }
    }
}
