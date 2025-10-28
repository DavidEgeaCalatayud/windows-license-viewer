using System;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace visualizador_License_Viewer
{
    public class MainForm : Form
    {
        PictureBox logo;
        Label lblProductKey;
        Label lblSerial;

        public MainForm()
        {
            InitializeComponents();
            LoadInfo();
        }

        void InitializeComponents()
        {
            this.Text = "Información del sistema";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 500;
            this.Height = 350;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10);

            // Logo Windows
            logo = new PictureBox()

            {
                Image = Properties.Resources.logoWindows,
                SizeMode = PictureBoxSizeMode.Zoom,
                Left = 100,
                Top = 20,
                Width = 250,
                Height = 120,
            };
            this.Controls.Add(logo);

            // Clave de producto
            lblProductKey = new Label()
            {
                Text = "Clave de producto de Windows: ",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Top = 160,
                Left = 10
            };

            // Número de serie
            lblSerial = new Label()
            {
                Text = "Número de serie: ",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Top = 200,
                Left = 10
            };

            this.Controls.Add(lblProductKey);
            this.Controls.Add(lblSerial);
        }


        void LoadInfo()
        {
            string productKey = GetWindowsProductKey() ?? "(No disponible / requiere permisos)";
            string serial = GetSystemSerial() ?? "(No disponible)";

            lblProductKey.Text = "Clave de producto de Windows: " + productKey;
            lblSerial.Text = "Número de serie: " + serial;
        }

        string GetSystemSerial()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        var s = mo["SerialNumber"] as string;
                        if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                    }
                }

                using (var searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_ComputerSystemProduct"))
                {
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        var s = mo["UUID"] as string;
                        if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                    }
                }
            }
            catch { }
            return null;
        }

        string GetWindowsProductKey()
        {
            try
            {
                const string keyPath = @"SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key == null) return null;
                    var digitalProductId = key.GetValue("DigitalProductId") as byte[];
                    if (digitalProductId == null) return null;
                    return DecodeProductKey(digitalProductId);
                }
            }
            catch { return null; }
        }

        string DecodeProductKey(byte[] digitalProductId)
        {
            try
            {
                const int keyStartIndex = 52;
                byte[] key = new byte[15];
                Array.Copy(digitalProductId, keyStartIndex, key, 0, 15);

                const string chars = "BCDFGHJKMPQRTVWXY2346789";
                var decodedChars = new char[29];

                for (int i = 28; i >= 0; i--)
                {
                    if ((i + 1) % 6 == 0)
                    {
                        decodedChars[i] = '-';
                    }
                    else
                    {
                        int accumulator = 0;
                        for (int j = 14; j >= 0; j--)
                        {
                            accumulator = accumulator * 256 + key[j];
                            key[j] = (byte)(accumulator / 24);
                            accumulator %= 24;
                        }
                        decodedChars[i] = chars[accumulator];
                    }
                }

                return new string(decodedChars);
            }
            catch { return null; }
        }
    }
}
