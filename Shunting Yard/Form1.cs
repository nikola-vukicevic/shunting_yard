////////////////////////////////////////////////////////////////////////////////
// Shunting yard - implementacija (12.11.2015.)                               //
// Autor: Nikola Vukicevic (projekat za II godinu)                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shunting_Yard
{
    public partial class Form1 : Form
    {
        Double var_a, var_b, var_c, var_d, var_e;

        public Form1()
        {
            InitializeComponent();
        }

        private void dodajPlusMinus(char c, Stack<char> stek, Queue<char> red)
        {
            if (stek.Peek() == '(' || stek.Peek() == '?')
            {
                stek.Push(c);
            }
            else
            {
                while (stek.Peek() == '+' || stek.Peek() == '-' ||
                       stek.Peek() == '*' || stek.Peek() == '/')
                {
                    red.Enqueue(stek.Pop());
                }
                stek.Push(c);
            }
         }

        private void dodajPutaKroz(char c, Stack<char> stek, Queue<char> red)
        {
            if (stek.Peek() == '(' || stek.Peek() == '?' ||
                stek.Peek() == '+' || stek.Peek() == '-')
            {
                stek.Push(c);
            }
            else
            {
                while (stek.Peek() == '/' || stek.Peek() == '*')
                {
                    red.Enqueue(stek.Pop());
                }
                stek.Push(c);
            }
        }

        private void dodajZatvorenuZagradu(char c, Stack<char> stek, Queue<char> red)
        {
            while (stek.Peek() != '(')
            {
                red.Enqueue(stek.Pop());
            }
            stek.Pop();
        }

        private string Parse(string s)
        {
            Int32 i, d = s.Length;
            String str = "";
            Queue<char> red = new Queue<char>();
            Stack<char> stek = new Stack<char>();
            stek.Push('?');
            
            for (i = 0; i < d; i++)
            {
                char c = s[i];

                if (c >= 97 && c <= 122)
                {
                    red.Enqueue(c);
                }
                else
                {
                    switch (c)
                    {
                        case '(': stek.Push(c); break;
                        case '+':
                        case '-': dodajPlusMinus(c, stek, red); break;
                        case '*':
                        case '/': dodajPutaKroz(c, stek, red); break;
                        case ')': dodajZatvorenuZagradu(c, stek, red); break;
                        default: break;
                     }
                }
            }

            while (stek.Peek() != '?') red.Enqueue(stek.Pop());
            ProcenaVrednosti(red);
            while (red.Count > 0) str += red.Dequeue().ToString();
            return str;
        }

        private void ProcenaVrednosti(Queue<char> red)
        {
            Stack<Double> stek = new Stack<Double>();
            char[] tokeni = red.ToArray();
            Int32 i, d = tokeni.Length;
            
            for (i = 0; i < d;i++ )
            {
                char znak = tokeni[i];

                if (znak >= 97 && znak <= 122)
                {
                    switch (znak)
                    {
                        case 'a': stek.Push(var_a); break;
                        case 'b': stek.Push(var_b); break;
                        case 'c': stek.Push(var_c); break;
                        case 'd': stek.Push(var_d); break;
                        case 'e': stek.Push(var_e); break;
                        default: MessageBox.Show("Uneli ste neodgovarajucu promenljivu!"); return;
                    }
                }
                else
                {
                    Double Op2 = stek.Pop(), Op1 = stek.Pop();
                    switch (znak)
                    {
                        case '+': stek.Push(Op1 + Op2); break;
                        case '-': stek.Push(Op1 - Op2); break;
                        case '*': stek.Push(Op1 * Op2); break;
                        case '/': stek.Push(Op1 / Op2); break;
                        default: break;
                    }
                }
            }

            Double rez = stek.Pop();
            label9.Text = rez.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Double.TryParse(tbox_a.Text, out var_a))
            {
                MessageBox.Show("Greška pri učitavanju promenljive a!");
                return;
            }
            
            if (!Double.TryParse(tbox_b.Text, out var_b))
            {
                MessageBox.Show("Greška pri učitavanju promenljive b!");
                return;
            }

            if (!Double.TryParse(tbox_c.Text, out var_c))
            {
                MessageBox.Show("Greška pri učitavanju promenljive c!");
                return;
            }

            if (!Double.TryParse(tbox_d.Text, out var_d))
            {
                MessageBox.Show("Greška pri učitavanju promenljive d!");
                return;
            }

            if (!Double.TryParse(tbox_e.Text, out var_e))
            {
                MessageBox.Show("Greška pri učitavanju promenljive e!");
                return;
            }

            label1.Text = Parse(textBox1.Text);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                textBox1.Text = label1.Text = "";
            }
        }
    }
}
