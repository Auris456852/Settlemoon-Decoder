using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Settlemoon_Decoder
{
    public partial class Decoder : Form
    {
        private List<int> InputSymbols = new List<int>{ 8, 5, 11, 13 }; //The currently input symbols.
        private Stack<PictureBox> InputImages = new Stack<PictureBox>(); //The images used to display the current input.
        public Decoder()
        {
            InitializeComponent();
            ProcessSymbols();
        }
        /// <summary>
        /// Called when a button is clicked.
        /// </summary>
        /// <param name="sender">The button that sent the signal that it was clicked.</param>
        /// <param name="e">Event arguments, usually tells us about various parameters about the event as defined, but the base EventArgs class is empty.</param>
        private void Button_Click(object sender, EventArgs e)
        {
            InputSymbols.Add(int.Parse((sender as Button).Tag.ToString()));
            ProcessSymbols();
        }
        /// <summary>
        /// Called when a textbox's text is changed,
        /// </summary>
        /// <param name="sender">The textbox that just sent the signal that its text was changed.</param>
        /// <param name="e">Event arguments, usually tells us about various parameters about the event as defined, but the base EventArgs class is empty.</param>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            ProcessSymbols();
        }
        /// <summary>
        /// Clears old possibilities and gets all new possibilities.
        /// </summary>
        private void ProcessSymbols()
        {
            try
            {
                RemoveExcessSymbols();
                AddMissingSymbols();
                listViewPossibilities.Items.Clear();
                Stack<string> possibilities = new Stack<string>();
                possibilities.Push("");
                foreach (int symbol in InputSymbols)
                {
                    Stack<string> possibilityStep = new Stack<string>();
                    char[] characters = Controls.Find($"textBox{symbol}", false)[0].Text.ToCharArray();
                    if(characters.Length == 0)
                        characters = new char[]{ '?' };
                    foreach(string possibility in possibilities)
                    {
                        foreach(char character in characters)
                        {
                            possibilityStep.Push(possibility + character);
                        }
                    }
                    possibilities = possibilityStep;
                }
                foreach (string possibility in possibilities)
                {
                    listViewPossibilities.Items.Add(new ListViewItem(possibility));
                }
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message + 
                    Environment.NewLine + 
                    Environment.NewLine + 
                    "More info:" + 
                    Environment.NewLine + 
                    E.StackTrace);
            }
            
        }
        /// <summary>
        /// Removes images until there are as many as there are symbols.
        /// </summary>
        private void RemoveExcessSymbols()
        {
            while (InputImages.Count > InputSymbols.Count)
            {
                var image = InputImages.Pop();
                image.Dispose();
                Controls.Remove(image);
            }
        }
        /// <summary>
        /// Adds images of recently added symbols.
        /// </summary>
        private void AddMissingSymbols()
        {
            while (InputImages.Count < InputSymbols.Count)
            {
                var image = new PictureBox();
                image.Size = new Size(48, 48);
                image.SizeMode = PictureBoxSizeMode.StretchImage;
                image.Location = new Point(26 + 48 * (InputImages.Count), (groupBoxText.Size.Height - 48) / 2);
                image.Image = (Image)Properties.Resources.ResourceManager.GetObject(InputSymbols[InputImages.Count].ToString());
                InputImages.Push(image);
                groupBoxText.Controls.Add(image);
            }
        }

        /// <summary>
        /// Called when the back button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Event arguments, usually tells us about various parameters about the event as defined, but the base EventArgs class is empty.</param>
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            InputSymbols.RemoveAt(InputSymbols.Count - 1);
            ProcessSymbols();
        }
    }
}
