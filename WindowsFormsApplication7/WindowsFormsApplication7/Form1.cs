using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions; //extra
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;

namespace WindowsFormsApplication7
{

    public partial class Form1 : Form
    {
        int cR, cG, cB;
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "archivos jpg|*.jpg";
            openFileDialog1.ShowDialog();
            bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            /*Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            c = bmp.GetPixel(e.X, e.Y);
            textBox1.Text = c.R.ToString();
            textBox2.Text = c.G.ToString();
            textBox3.Text = c.B.ToString();
             */
            bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int sR, sG, sB;
            sR = 0;
            sG = 0;
            sB = 0;
            for (int i = e.X; i < e.X + 10;i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                { 
                    c = bmp.GetPixel(i, j);
                    sR = sR + c.R;
                    sG = sG + c.G;
                    sB = sB + c.B;
                }
            sR = sR/100;
            sG = sG/100;
            sB = sB/100;
            cR = sR;
            cG = sG;
            cB = sB;
            textBox1.Text = sR.ToString();
            textBox2.Text = sG.ToString();
            textBox3.Text = sB.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            for (int i=0;i<bmp.Width;i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    if (((74 <= c.R) && (c.R <= 104)) && ((84 <= c.G) && (c.G <= 114)) && ((74 <= c.B) && (c.B <= 104)))
                        bmp2.SetPixel(i, j, Color.Black);
                    else
                        bmp2.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));
                }
            pictureBox1.Image = bmp2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width-10; i=i+10)
                for (int j = 0; j < bmp.Height-10; j=j+10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (((cR - 20 <= sR) && (sR <= cR + 20)) && ((cG - 20 <= sG) && (sG <= cG + 20)) && ((cB - 20 <= sB) && (sB <= cB + 20)))
                        {
                            for (int ip = i; ip < i + 10; ip++)
                                for (int jp = j; jp < j + 10; jp++)
                                {
                                    bmp2.SetPixel(ip, jp, Color.Black);
                                }
                        }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                    }
                        
                }
            pictureBox1.Image = bmp2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OdbcConnection con = new OdbcConnection();
            OdbcCommand cmd = new OdbcCommand();

            con.ConnectionString = "DSN=prueba";

            cmd.CommandText = "insert into texturas(descripcion,cR,cG,cB,colorpintar) ";
            cmd.CommandText = cmd.CommandText + " values('"+textBox4.Text+"',"+textBox1.Text+","+textBox2.Text+","+textBox3.Text+",'"+SeleccionarColor()+"')";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            mostrar();
        }


        

        private void button5_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            Bitmap bmpR = new Bitmap(bmp.Width, bmp.Height);
            bmpR = bmp;
            int r, g, b;
            bool usado;
            String coloresUsados="";
            
            for (int k = 0; k < dataGridView1.RowCount-1; k++)
            {
                r = 0; g = 0; b = 0;
                usado = false;
                r = Int32.Parse(dataGridView1.Rows[k].Cells[2].Value.ToString());
                g = Int32.Parse(dataGridView1.Rows[k].Cells[3].Value.ToString());
                b = Int32.Parse(dataGridView1.Rows[k].Cells[4].Value.ToString());
                
                String data = dataGridView1.Rows[k].Cells[5].Value.ToString();

                int alpha = 255, red = 0, green = 0, blue = 0;
                String colorName;
                
                Color colorobtenido = new Color();

                if (Regex.IsMatch(data, @"Color \[A=\d+, R=\d+, G=\d+, B=\d+\]"))
                {
                    
                    Match match = Regex.Match(data, @"A=(\d+), R=(\d+), G=(\d+), B=(\d+)");
                    if (match.Success)
                    {
                        alpha = int.Parse(match.Groups[1].Value);
                        red = int.Parse(match.Groups[2].Value);
                        green = int.Parse(match.Groups[3].Value);
                        blue = int.Parse(match.Groups[4].Value);
                       

                    }
                }
                else if (Regex.IsMatch(data, @"Color \[.*\]"))
                {
                    
                    colorName = data.Replace("Color [", "").Replace("]", "");
                    colorobtenido = Color.FromName(colorName);
                    
                    alpha = colorobtenido.A;
                    red = colorobtenido.R;
                    green = colorobtenido.G;
                    blue = colorobtenido.B;
                    
                }



                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        c = bmpR.GetPixel(i, j);
                       
                         if ((c.R == r) && (c.G == g) && (c.B == b))
                         {
                            //colorPanel.BackColor = Color.FromArgb(red, green, blue);
                            usado = true;
                            
                            bmpR.SetPixel(i, j, Color.FromArgb(alpha,red, green, blue));
                         }
                         else
                         {
                             bmpR.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));
                         }


                    }
                }

              
                if (usado)
                {
                    coloresUsados = "Color: " + data + "\n" + coloresUsados;
                    //MessageBox.Show("si ingresa");
                }
                
                
            }
            
            pictureBox2.Image = bmpR;
            MessageBox.Show(coloresUsados);
        }

        //para colores
        /*public ColorMessageBox(string rgbString)
        {
            InitializeComponent();
            SetColor(rgbString);
        }*/

 


        //Variables Boton Color
        Color ColorUtil;

        //Metodos Boton Color
        public Color SeleccionarColor()
        {
            int r = ColorUtil.R;
            int g = ColorUtil.G;
            int b = ColorUtil.B;
            //textBox5.Text = $"RGB: ({r}, {g}, {b})";
            return ColorUtil;
        }

        //Objetos Boton Color
        ColorDialog ObjDialog1 = new ColorDialog();

        private void button6_Click(object sender, EventArgs e)
        {
            if (ObjDialog1.ShowDialog() == DialogResult.OK)
            {
                ColorUtil = ObjDialog1.Color;
                SeleccionarColor();  
            } 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mostrar();
        }

        private void mostrar()
        {
            OdbcConnection con = new OdbcConnection();
            OdbcDataAdapter ada = new OdbcDataAdapter();
            con.ConnectionString = "DSN=prueba";
            ada.SelectCommand = new OdbcCommand();
            ada.SelectCommand.Connection = con;
            ada.SelectCommand.CommandText = "select * from texturas";
            DataSet ds = new DataSet();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
