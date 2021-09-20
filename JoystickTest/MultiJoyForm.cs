/*
 * Copyright © 2021 RJP MultiJoy robbyxp1 @ github.com
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 */

using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JoystickTest
{
    public partial class MultiJoyForm : Form
    {
        Timer t = new Timer();
        List<JoyUC> juc = new List<JoyUC>();
        int nodevices = 0;
        int checknewcounter = 0;
        DirectInput dinput;

        public MultiJoyForm()
        {
            InitializeComponent();
            dinput = new DirectInput();
            Scan();
        }

        private void Scan()
        {
            t.Stop();
            juc.Clear();

            int voff = 10;
            var devices = dinput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
            nodevices = devices.Count;

            int controlswidth = 600;

            foreach (DeviceInstance di in devices)
            {
                bool doit = true;
                // doit = (di.InstanceName.Contains("Thrustmaster")) || (di.InstanceName.Contains("16000"));

                if (doit)
                {
                    System.Diagnostics.Debug.WriteLine("Detected {0} {1}", di.InstanceGuid, di.InstanceName.RemoveNuls());

                    JoyUC j = new JoyUC();
                    //j.Width = ClientRectangle.Width - 10;
                    int h = j.Init(dinput, di);

                    if (h != 0)
                    {
                        j.Height = h;
                        j.Location = new Point(5, voff);
                        voff += j.Height + 10;
                        panelScroll.Controls.Add(j);
                        juc.Add(j);
                        controlswidth = Math.Max(j.Right, controlswidth);
                    }
                }

                PerformLayout();
            }

            this.Size = new Size(controlswidth+50, Math.Min(1000,voff + 50));
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();

        }

        private void T_Tick(object sender, EventArgs e)
        {
            bool stickerror = false;
            foreach (JoyUC ju in juc)
            {
                if (!ju.CheckStick())
                {
                    stickerror = true;
                    break;
                }
            }
           
            if ( stickerror || ++checknewcounter % 20 == 0)
            {
                var devices = dinput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
                if ( devices.Count != nodevices)
                {
                    System.Diagnostics.Debug.WriteLine("New/deleted device");
                    panelScroll.Controls.Clear();
                    Scan();
                }

            }
        }
    }
}
