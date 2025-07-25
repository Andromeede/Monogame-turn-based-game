using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RPGWithManagers
{
    public class MaTimer //Timer class
    {// internal parameters
        public bool goodToGo;
        protected int mSec;
        protected TimeSpan timer = new TimeSpan();


        public MaTimer(int m) //timer constructor
        {
            goodToGo = false;
            mSec = m;
        }
        public MaTimer(int m, bool StartLoaded) //second timer constructor with additional parameters 
        {
            goodToGo = StartLoaded;
            mSec = m;
        }

        public int MSec //getter and setter for the Msec value of each timer
        {
            get { return mSec; }
            set { mSec = value; }
        }
        public int Timer //getter for the timer value
        {
            get { return (int)timer.TotalMilliseconds; }
        }

        public void UpdateTimer() //update timer function
        {
            timer += GlobalUtil.gameTime.ElapsedGameTime; //update elapsed time in the timer
        }

        public void UpdateTimer(float Speed) //update timer function based on the timer speed 
        {
            timer += TimeSpan.FromTicks((long)(GlobalUtil.gameTime.ElapsedGameTime.Ticks * Speed)); //update elapsed time (slow/faster pace)
        }

        public virtual void AddToTimer(int Msec) //add time to existing timer
        {
            timer += TimeSpan.FromMilliseconds(Msec);//adds the input time to the timer
        }

        public bool Test() // test if the timer is completed
        {
            if (timer.TotalMilliseconds >= mSec || goodToGo) //if the timer is completed
            {
                return true; //sets the completion
            }
            else
            {
                return false;
            }
        }

        public void Reset() //reset the timer 
        {
            timer = timer.Subtract(new TimeSpan(0, 0, mSec / 60000, mSec / 1000, mSec % 1000));//substract the timer value to the timer elapsed time
            if (timer.TotalMilliseconds < 0) //If the timer value is less than 0
            {
                timer = TimeSpan.Zero; //replace by zero
            }
            goodToGo = false;
        }

        public void Reset(int Newtimer) //reset the timer to a new value
        {
            timer = TimeSpan.Zero;
            MSec = Newtimer;
            goodToGo = false;
        }

        public void ResetToZero() //reset the timer to zero
        {
            timer = TimeSpan.Zero;
            goodToGo = false;
        }

        public virtual XElement ReturnXML()
        {
            XElement xml = new XElement("Timer",
                                    new XElement("mSec", mSec),
                                    new XElement("timer", Timer));



            return xml;
        }

        public void SetTimer(TimeSpan Time) //set timer interval 
        {
            timer = Time;
        }

        public virtual void SetTimer(int Msec) // set timer value
        {
            timer = TimeSpan.FromMilliseconds(Msec);
        }
    }
}
