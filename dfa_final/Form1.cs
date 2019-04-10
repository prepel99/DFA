using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace dfa_final
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DataContract]
        public class State
        {
            [DataMember]
            public bool Final;
            [DataMember]
            public int ID;
            [DataMember]
            public Dictionary<string, int> Transitions;
            public State()
            {
                Transitions = new Dictionary<string, int>();
            }
        }
        [DataContract]
        public class DFA
        {
            [DataMember]
            public List<State> States;
            [DataMember]
            public List<string> Alphabet;
            public DFA(List<State> States, List<string> Alphabet)
            {
                this.States = States;
                this.Alphabet = Alphabet;
                FirstState = States[0];
            }
            public State FirstState;

            public bool Run(string Tape)
            {
                int current = States[0].ID;
                foreach (char c in Tape)
                    current = States[current].Transitions[c.ToString()];
                return States[current].Final;
            }
            public bool Check()
            {
                if (Alphabet.Count == 0)
                    return false;
                foreach (State i in States)
                {
                    if (i.Transitions.Count != Alphabet.Count)
                        return false;
                    foreach (var j in i.Transitions)
                        if (!Alphabet.Contains(j.Key))
                            return false;
                }
                return true;
            }
            
        }
        string filename;
        private void button1_Click(object sender, EventArgs e)
        {

            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(DFA));
      //      string file = textBox2.Text + ".json";
            DFA dfa;
            string tape = textBox1.Text;
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
              dfa = (DFA)jsonFormatter.ReadObject(fs);
            }
            try
            {
                foreach (char s in tape)
                    if (!dfa.Alphabet.Contains(s.ToString()))
                        throw new Exception("На ленте присутствуют буквы, которых нет в алфавите");
                if (!dfa.Check())
                    throw new Exception("Автомат был задан неверно");
                label1.Text = dfa.Run(tape).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                
                

        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog() { Filter = "Файлы формата json(*.json)|*.json" };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                filename = openFileDialog1.FileName;
            button1.Enabled = true;
        }
    }
}
