using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//If not enough resources three times, resources released automatically.
namespace DMRET
{
    public partial class Form1 : Form
    {
        public int[] Resources; //Resource Array
        public int[] Available; //Available Resources
        public int[,] Allocation; //Allocation Array
        public Random RequestG=new Random(); //Random Request
        public Random ProcessG = new Random(); //Random Process
        public Random ResourceG = new Random(); //Random Resources
        public bool Any=true;//Random Release Flag
        public int TempRequest; //Temporary Request Variable
        public int TempProcess; //Temporary Process Variable
        public int TempResource; //Temporary Resource Variable
        public int NoT;//Number of Types
        public int NoR;//Number of Resources
        public int NoP;//Number of Process
        public int ReleaseCounter = 0;//For releasing

        public int RequestGenerator()
        {
            return RequestG.Next(1,Convert.ToInt32(comboBox1.SelectedItem));
        }
        public int ProcessGenerator() 
        {
            return ProcessG.Next(1,Convert.ToInt32(comboBox3.SelectedItem));
        }
        public int ResourceGenerator() 
        {
            return ResourceG.Next(1,Convert.ToInt32(comboBox2.SelectedItem));
        }
        public void FillResources() 
        {
            for (int i = 0; i < NoT; i++)
            {
                Resources[i] = ResourceGenerator();
            }
        }
        public void FillAllocation() 
        {
            for (int i = 0; i < NoP; i++)
            {
                for (int j = 0; j < NoT; j++)
                {
                    Allocation[i,j] = 0;
                }
            }
        }
        public void DisplayResourcesMatrix() 
        {
            textBox1.AppendText("\r\n---Resource Matrix---\n");
            for (int i = 0; i < NoT; i++)
            {
                textBox1.AppendText(Resources[i] + " ");
            }
            textBox1.AppendText("\r\n--------------------------");
        }
        public void DisplayAllocationMatrix() 
        {
            textBox1.AppendText("\r\n---Allocation Matrix---\n");
            for (int i = 0; i < NoP; i++)
            {
                for (int j = 0; j < NoT; j++)
                {
                    textBox1.AppendText(Allocation[i, j] + " ");
                }
                textBox1.AppendText("\r\n");
            }
            textBox1.AppendText("\r-----------------------\n");
        }
        public bool AnyResources() 
        {
            for (int i = 0; i < NoT; i++)
            {
                if (Resources[i] == 1) 
                {
                    Any=true;
                    break;
                }
            }
            return false;
        }
        public void ReleaseAllResources() 
        {
            for (int i = 0; i < NoP; i++)
            {
                for (int j = 0; j < NoT; j++)
                {
                    Resources[j] = Resources[j] + Allocation[i, j];
                }
            }
            FillAllocation();
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            if (!AnyResources())
            {
                TempProcess = ProcessGenerator();
                TempResource = ResourceGenerator();
                TempRequest = RequestGenerator();
                textBox1.AppendText("\r\nProcess Number : " + TempProcess + " request " + TempResource + " resources from " + TempRequest);
                if (Resources[TempRequest] >= TempResource)
                {
                    Resources[TempRequest] = Resources[TempRequest] - TempResource;
                    Allocation[TempProcess,TempRequest] = Allocation[TempProcess,TempRequest]+TempResource;
                    textBox1.AppendText("\r\nResources assigned to Process Number : " + TempProcess);
                    DisplayResourcesMatrix();
                }
                else
                {
                    textBox1.AppendText("\r\nNot enough resources for Process Number : " + TempProcess);
                    ReleaseCounter++;
                }
                if (ReleaseCounter == 3) 
                {
                    ReleaseCounter = 0;
                    DisplayAllocationMatrix();
                    textBox1.AppendText("\r\nProcesses releasing resources...");
                    ReleaseAllResources();
                    textBox1.AppendText("\r\nResources released...");
                    DisplayResourcesMatrix();
                }
            }
            else 
            {
                SimulationTimer.Enabled = false;
                textBox1.AppendText("\r\nThere is no resource for processes and simulation finished.");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null && comboBox3.SelectedItem != null)
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                button1.Enabled = false;
                NoT = Convert.ToInt32(comboBox1.SelectedItem);
                NoR = Convert.ToInt32(comboBox2.SelectedItem);
                NoP = Convert.ToInt32(comboBox3.SelectedItem);
                Resources = new int[NoT];
                Allocation = new int[NoP,NoT];
                FillResources();

                FillAllocation();
                textBox1.AppendText("\r\nMatrixes initilazied.. \nResources filled.");
                DisplayResourcesMatrix();
                textBox1.AppendText("\r\nSimulation started...");
                SimulationTimer.Enabled = true ;
            }
            else 
            {
                textBox1.AppendText("\r\nYou must select all properties...");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            SimulationTimer.Enabled = false;
            textBox1.AppendText("\r\nSimulation Finalized...");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Suspend")
            {
                SimulationTimer.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button3.Text = "Resume";
            }
            else 
            {
                button3.Text = "Suspend";
                SimulationTimer.Enabled = true;
            }
            
        }        
    }
}
