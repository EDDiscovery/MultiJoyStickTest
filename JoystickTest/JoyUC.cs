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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace JoystickTest
{
    public partial class JoyUC : UserControl
    {
        public JoyUC()
        {
            InitializeComponent();
        }

        DirectInput dinput;
        Joystick stick;
        RadioButton[] rb;
        int povcount;
        int butcount;
        int slidercount;

        public int Init(DirectInput pdinput, DeviceInstance di)
        {
            dinput = pdinput;

            groupBox1.Text = di.InstanceName;

            int min = 0;
            int max = 65535;

            trackBarX.Minimum = trackBarY.Minimum = trackBarZ.Minimum = trackBarZRot.Minimum = trackBarS1.Minimum = trackBarS2.Minimum = min;
            trackBarX.Maximum = trackBarY.Maximum = trackBarZ.Maximum = trackBarZRot.Maximum = trackBarS1.Maximum = trackBarS2.Maximum = max;
            trackBarX.Visible = trackBarY.Visible = trackBarZ.Visible = trackBarZRot.Visible = trackBarS1.Visible = trackBarS2.Visible = false;
            labelX.Visible = labelY.Visible = labelZ.Visible = labelZRot.Visible = labelS1.Visible = labelS2.Visible = false;

            try
            {
                stick = new SharpDX.DirectInput.Joystick(dinput, di.InstanceGuid);
                stick.Acquire();

                Capabilities c = stick.Capabilities;
                povcount = c.PovCount;
                butcount = c.ButtonCount;
                rb = new RadioButton[butcount];
                slidercount = 0;

                DeviceProperties p = stick.Properties;

                System.Diagnostics.Debug.WriteLine("ax {0} but {1} pov {2} vid {3:X} pid {4:X}", c.AxeCount, c.ButtonCount, c.PovCount, p.VendorId, p.ProductId);



                foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                {
                    if ((deviceObject.ObjectId.Flags & DeviceObjectTypeFlags.Axis) != 0)
                    {
                        System.Guid guid = deviceObject.ObjectType;
                        System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} {4}", deviceObject.Name, deviceObject.UsagePage, deviceObject.Usage, deviceObject.Offset , guid.ToString());

                        if (guid == ObjectGuid.XAxis)
                            labelX.Visible = trackBarX.Visible = true;
                        else if (guid == ObjectGuid.YAxis)
                            labelY.Visible = trackBarY.Visible = true;
                        else if (guid == ObjectGuid.ZAxis)
                            labelZ.Visible = trackBarZ.Visible = true;
                        else if (guid == ObjectGuid.RxAxis)        // these we just display as values.. but we want to take them out of the equation
                        { }
                        else if (guid == ObjectGuid.RyAxis)        // these we just display as values..
                        { }
                        else if (guid == ObjectGuid.RzAxis)
                            labelZRot.Visible = trackBarZRot.Visible = true;
                        else
                        {                                                   // must be sliders, only ones left with axis
                            if ( slidercount == 0 )
                                labelS1.Visible = trackBarS1.Visible = true; // labelS2.Visible = trackBarS2.Visible = true;
                            else if ( slidercount == 1)
                                labelS2.Visible = trackBarS2.Visible = true;

                            slidercount++;      // 3 on shown as numbers                
                        }

                        ObjectProperties o = stick.GetObjectPropertiesById(deviceObject.ObjectId);
                        //System.Diagnostics.Debug.WriteLine("  L" + o.LowerRange + " U" + o.UpperRange + " G" + o.Granularity + " D" + o.DeadZone + " Il" + o.LogicalRange.Minimum + " Iu" + o.LogicalRange.Maximum);
                        o.Range = new InputRange(min, max);
                    }
                }

                int hbase = 130;

                for (int i = 0; i < butcount; i++ )
                {
                    Panel p1 = new Panel();
                    p1.Location = new Point((i%16) * 50 + 5, hbase + 20 * (i/16));
                    p1.Size = new Size(50, 20);
                    RadioButton r = new RadioButton();
                    r.Location = new Point(1, 5);
                    r.AutoSize = true;
                    r.Size = new Size(85, 18);
                    r.Text = "B" + (i + 1);
                    p1.Controls.Add(r);
                    rb[i] = r;
                    groupBox1.Controls.Add(p1);
                }

                return hbase + 16 + 20 * ((butcount+15) / 16);  // size of UC.. include space for groupbox

            }
            catch
            {
                return 0;

            }

        }

        public void CheckStick()
        {
            //System.Diagnostics.Debug.WriteLine("Check stick " + stick.Properties.InstanceName);

            try
            {
                JoystickState js = stick.GetCurrentState();

                bool[] buttons = js.Buttons;

                for (int i = 0; i < butcount; i++)
                {
                    rb[i].Checked = buttons[i];
                }

                //System.Diagnostics.Debug.WriteLine("x={0} y={1} z={2}", js.X, js.Y, js.Z);
                trackBarX.Value = js.X;
                trackBarY.Value = js.Y;
                trackBarZ.Value = js.Z;
                trackBarZRot.Value = js.RotationZ;

                int[] sliders = js.Sliders;
                if (sliders.Length > 0)
                {
                    trackBarS1.Value = sliders[0];

                    if (sliders.Length > 1)
                        trackBarS2.Value = sliders[1];
                }

                int[] pov = js.PointOfViewControllers;
                string s = "POV ";
                for (int p = 0; p < povcount; p++)
                {
                    s += (p + 1) + ":";

                    if (pov[p] < 0)
                        s += "Not Pressed ";
                    else
                        s += (pov[0] / 100) + " degrees ";
                }

                textBox1.Text = s;

                textBox2.Text = "Other:" + js.RotationX + "," + js.RotationY;

                for (int i = 2; i < sliders.Length; i++)
                    textBox2.Text += "," + sliders[i];
            }
            catch
            {

            }
        }

    }
}
