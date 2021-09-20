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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

            int min = 0;
            int max = 65535;

            trackBarX.Minimum = trackBarY.Minimum = trackBarZ.Minimum = trackBarRX.Minimum = trackBarRY.Minimum = trackBarRZ.Minimum = trackBarS1.Minimum = trackBarS2.Minimum = min;
            trackBarX.Maximum = trackBarY.Maximum = trackBarZ.Maximum = trackBarRX.Maximum = trackBarRY.Maximum = trackBarRZ.Maximum = trackBarS1.Maximum = trackBarS2.Maximum = max;
            trackBarX.Visible = trackBarY.Visible = trackBarZ.Visible =
            trackBarRX.Visible = trackBarRY.Visible = trackBarRZ.Visible =
            trackBarS1.Visible = trackBarS2.Visible = false;
            labelX.Visible = labelY.Visible = labelZ.Visible =
            labelRX.Visible = labelRY.Visible = labelRZ.Visible =
            labelS1.Visible = labelS2.Visible = false;
            povBox.Visible = false;

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

                groupBox1.Text = di.InstanceName.Substring(0,di.InstanceName.IndexOf('\0')) + " : " + p.VendorId.ToString("X4") + p.ProductId.ToString("X4");

                System.Diagnostics.Debug.WriteLine("  ax {0} but {1} pov {2} vid {3:X} pid {4:X}", c.AxeCount, c.ButtonCount, c.PovCount, p.VendorId, p.ProductId);

                if (!di.InstanceName.RemoveNuls().Contains("16000"))
                {
                //    return 0;

                }

                foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                {
                    if ((deviceObject.ObjectId.Flags & DeviceObjectTypeFlags.Axis) != 0)
                    {
                        System.Guid guid = deviceObject.ObjectType;
                        System.Diagnostics.Debug.WriteLine("  {0} {1} {2} {3} {4}" , deviceObject.Name.RemoveNuls(), deviceObject.UsagePage, deviceObject.Usage, deviceObject.Offset , guid.ToString().RemoveNuls());

                        if (guid == ObjectGuid.XAxis)
                            labelX.Visible = trackBarX.Visible = true;
                        else if (guid == ObjectGuid.YAxis)
                            labelY.Visible = trackBarY.Visible = true;
                        else if (guid == ObjectGuid.ZAxis)
                            labelZ.Visible = trackBarZ.Visible = true;
                        else if (guid == ObjectGuid.RxAxis)       
                            labelRX.Visible = trackBarRX.Visible = true;
                        else if (guid == ObjectGuid.RyAxis)       
                            labelRY.Visible = trackBarRY.Visible = true;
                        else if (guid == ObjectGuid.RzAxis)
                            labelRZ.Visible = trackBarRZ.Visible = true;
                        else if ( guid == ObjectGuid.Slider )
                        {                                                   
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

                // compress up the tracks

                int diff = labelRX.Top - labelX.Top;

                if (labelX.Visible == false )       // move RX up to X if possible
                {
                    labelRX.Top -= diff;
                    trackBarRX.Top -= diff;
                }
                if (labelY.Visible == false )
                {
                    labelRY.Top -= diff;
                    trackBarRY.Top -= diff;
                }
                if (labelZ.Visible == false )
                {
                    labelRZ.Top -= diff;
                    trackBarRZ.Top -= diff;
                }           // if RZ in same place, but S1 is missing, and there is nothing else on RZ line
                else if (labelS1.Visible == false && labelRZ.Visible == true && labelRX.Visible == false && labelRY.Visible == false && labelS2.Visible == false)
                {
                    labelRZ.Location = labelS1.Location;
                    trackBarRZ.Location = trackBarS1.Location;
                }

                List<Control> tracks = new List<Control>() { trackBarX, trackBarY, trackBarZ, trackBarS1, trackBarRX, trackBarRY, trackBarRZ, trackBarS2};
                int maxtrackvisible = tracks.Where(w => w.Visible).Select(x => x.Bottom).Max();     // find bottom of all tracks

                if ( c.PovCount>0 )
                    povBox.Visible = true;

                extraInfoBox.Top = povBox.Top = maxtrackvisible + 4;

                int hbase = extraInfoBox.Bottom+4;

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

        public bool CheckStick()
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

                if (trackBarX.Value != js.X)
                    trackBarX.Value = js.X;
                if (trackBarY.Value != js.Y)
                    trackBarY.Value = js.Y;
                if (trackBarZ.Value != js.Z)
                    trackBarZ.Value = js.Z;
                if (trackBarRX.Value != js.RotationX)
                    trackBarRX.Value = js.RotationX;
                if (trackBarRY.Value != js.RotationY)
                    trackBarRY.Value = js.RotationY;
                if (trackBarRZ.Value != js.RotationZ)
                    trackBarRZ.Value = js.RotationZ;

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

                povBox.Text = s;

                extraInfoBox.Text = "";

                for (int i = 2; i < sliders.Length; i++)
                    extraInfoBox.Text += "," + sliders[i];

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
