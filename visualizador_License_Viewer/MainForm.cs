using System;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;

namespace visualizador_License_Viewer
{
    public class MainForm : Form
    {
        PictureBox logo;
        Label lblProductKeyTitle;
        Label lblSerialTitle;
        TextBox txtProductKey;
        TextBox txtSerial;

        public MainForm()
        {
            InitializeComponents();
            LoadInfo();
        }

        void InitializeComponents()
        {
            this.Text = "Información del sistema";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(470, 320);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 10);

            logo = new PictureBox()
            {
                Image = Properties.Resources.logoWindows,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 250,
                Height = 120,
            };

            txtProductKey = new TextBox()
            {
                Width = 460,
                Height = 28,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Center
            };
            txtProductKey.KeyDown += TxtField_KeyDown;

            lblProductKeyTitle = new Label()
            {
                Text = "Clave de producto de Windows:",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };

            txtSerial = new TextBox()
            {
                Width = 460,
                Height = 28,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                TextAlign = HorizontalAlignment.Center
            };
            txtSerial.KeyDown += TxtField_KeyDown;

            // Etiqueta Número de serie
            lblSerialTitle = new Label()
            {
                Text = "Número de serie:",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };

            CenterElements();

            // Agregar controles al formulario
            this.Controls.Add(logo);
            this.Controls.Add(lblProductKeyTitle);
            this.Controls.Add(txtProductKey);
            this.Controls.Add(lblSerialTitle);
            this.Controls.Add(txtSerial);

            // Ajustar posiciones al redimensionar
            this.Resize += (s, e) => CenterElements();
        }

        private void CenterElements()
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            logo.Left = (formWidth - logo.Width) / 2;
            logo.Top = (formHeight - 350) / 2 + 10;

            // Posición relativa debajo del logo
            int spacing = 20;
            int topStart = logo.Bottom + spacing;

            lblProductKeyTitle.Top = topStart;
            lblProductKeyTitle.Left = (formWidth - lblProductKeyTitle.PreferredWidth) / 2;

            txtProductKey.Top = lblProductKeyTitle.Bottom + 5;
            txtProductKey.Left = (formWidth - txtProductKey.Width) / 2;

            lblSerialTitle.Top = txtProductKey.Bottom + 20;
            lblSerialTitle.Left = (formWidth - lblSerialTitle.PreferredWidth) / 2;

            txtSerial.Top = lblSerialTitle.Bottom + 5;
            txtSerial.Left = (formWidth - txtSerial.Width) / 2;
        }

        private void TxtField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    if (textBox.ReadOnly)
                    {
                        textBox.ReadOnly = false;
                        textBox.BorderStyle = BorderStyle.FixedSingle;
                        textBox.BackColor = Color.White;
                        textBox.ForeColor = Color.Black;
                        textBox.Cursor = Cursors.IBeam;
                        textBox.Focus();
                        textBox.SelectionStart = textBox.Text.Length;
                    }
                    else
                    {
                        textBox.ReadOnly = true;
                        textBox.BorderStyle = BorderStyle.None;
                        textBox.BackColor = this.BackColor;
                        textBox.ForeColor = Color.Black;
                        textBox.Cursor = Cursors.Default;
                        this.ActiveControl = null;
                    }
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        void LoadInfo()
        {
            string productKey = GetWindowsProductKey() ?? "(No disponible / requiere permisos)";
            string serial = GetSystemSerial() ?? "(No disponible)";

            txtProductKey.Text = productKey;
            txtSerial.Text = serial;
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
