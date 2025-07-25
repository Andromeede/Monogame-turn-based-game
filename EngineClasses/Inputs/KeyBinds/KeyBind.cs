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
    public class KeyBind
    {
        public string name, key;


        public KeyBind(string Name, string Key) 
        { 
            name = Name;
            key = Key;
        }
        
        public string Key
        {
            set 
            { 
                key = value;            
            }

        }

        public virtual XElement ReturnXML() //returns an XElement in order to be stored in an XML file
        {
            XElement xml = new XElement("Key", //creates the element "key"
                                new XAttribute ("n" , name), //add an attribute "n" and actual value to the "Key" element
                                new XElement("value", key)); //add an element "value" and actual value inside the "Key" element
            return xml;
        }
    }
}
