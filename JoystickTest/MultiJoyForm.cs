/*
 * Copyright © 2017 RJP MultiJoy
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JoystickTest
{
    public partial class MultiJoyForm : Form
    {
        Timer t = new Timer();
        List<JoyUC> juc = new List<JoyUC>();

        public MultiJoyForm()
        {
            InitializeComponent();

            int voff = 10;
            DirectInput dinput = new DirectInput();

            foreach (DeviceInstance di in dinput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly))
            {
                bool doit = true;
               // doit = (di.InstanceName.Contains("Thrustmaster")) || (di.InstanceName.Contains("16000"));
               
                if (doit)
                {
                    System.Diagnostics.Debug.WriteLine("{0} {1}", di.InstanceGuid, di.InstanceName);

                    JoyUC j = new JoyUC();
                    j.Width = ClientRectangle.Width - 10;
                    int h = j.Init(dinput, di);

                    if (h != 0)
                    {
                        j.Height = h;
                        j.Location = new Point(5, voff);
                        voff += j.Height + 10;
                        this.Controls.Add(j);
                        juc.Add(j);
                    }
                }

                PerformLayout();
            }

            this.Size = new Size(this.Width, voff + 50);
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            foreach (JoyUC ju in juc)
                ju.CheckStick();
           
        }
    }
}
