// Necessary imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Runtime.InteropServices;

namespace TypeTutor
{

    public partial class TypeTutorForm : Form
    {

        #region Variable Declaration
        #region Code used for making form's border circular
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );
        #endregion
        private static Brush Highlight { get; }
        private char user_char;                       // Holds user's current typed char
        private static int chars_typed = 0;           // Holds user's current typed character
        private static int correct = 0;               // Holds user's correct typed characters
        private static int wrong = 0;                 // Holds user's wrong typed characters
        private static bool change_layout = true;     // Holds user's choice of changing layout

        // Contains the quotes that will be appended to SampleTextBox
        private static readonly List<string> my_quotes = new List<string>
        {
            "Many are so concerned about the future, that they forget the present -from which everything is defined.",
            "Finding a reason to die, is harder that finding a reason to live.",
            "Inside a book, one can hide a universe of knowledge.",
            "Life, is like an orchesrta. You're the maestro and everyone else the singers.",
            "Fame, is a reward to be given, not a target to be claimed.",
            "When one is young, lives in bravery* but when old, lives in fear.",
            "Love, is stronger than death. Even the ones who forget in their mind, never forget in their heart.",
            "What is the price of freedom? Sacrifice...",
            "This world isn't ours. We are just wanderers of the universe, in search of a celestial home.",
            "What keeps us alive, is also what kills us, if we don't follow it.",
            "The arrow of time takes multiple forms, reaching everyone in tis scope.",
            "True friends are there for you. The rest are there for themselves.",
            "To innovate in life, you must love to think."
        };
        // Contains keys' default background color values
        private static readonly List<Tuple<Button, Color>> defaultSettings = new List<Tuple<Button, Color>>();
        // Contains a list of current pressed buttons
        private static List<Button> cur_Buttons;
        #endregion

        // Define constructor
        public TypeTutorForm()
        {
            // Initialize Typing Tutor
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            // Initialize defaultColors
            foreach (Control key in Keyboard.Controls) {
                defaultSettings.Add(new Tuple<Button, Color>((Button)key, key.BackColor));
            }
            // Start the date-time timer
            DateTimer.Start();
        }

        #region Functions
        private void SampleTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            // Update CurrentKey's text
            CurrentKey.Text = $"{e.KeyData}";

            // Declare current pressed button
            UpdateButton(e);
            // Change cur_button's background color property
            foreach (Button button in cur_Buttons) {
                if (cur_Buttons.ElementAt(cur_Buttons.Count - 1) == button) {
                    button.BackColor = Color.FromArgb(177, 52, 235);
                }
            }
            
            

            /* 
             * If chars_typed by the user == length 
             * of the sample text, then the user has
             * correctly typed all the characters
             */
            if (chars_typed == SampleTextBox.Text.Length)
            {
                // Clear SampleTextBox
                SampleTextBox.Clear();
                // Append a new quote
                SampleTextBox.Text = NewQuote();
            }
            if (chars_typed > 0)
            {
                // Update user's typing speed
                Speed.Text = $"{chars_typed / 6}";
                // Update user's accuracy
                Accuracy.Text = $"{correct / chars_typed * 100}";
                // Update user's score
                Score.Text = $"{int.Parse(Speed.Text) * int.Parse(Accuracy.Text)}";
            }

        }

        // Resets cur_button's background color property
        private void ResetButtonColor()
        {
            // Iterate through current pressed buttons
            foreach (Button button in cur_Buttons)
            {
                // Iterate through defaultSetttings
                foreach (Tuple<Button, Color> item in defaultSettings)
                {
                    // If the cur_Button matches the button at item.Item1
                    if (button == item.Item1)
                    {
                        // Then reset it's background color to item.Item2
                        button.BackColor = item.Item2;
                    }
                }
            }
        }

        // Returns current pressed button
        private void UpdateButton(KeyEventArgs e)
        {
            if (SampleTextBox.Focused)
            {
                // Define current pressed button
                switch (e.KeyCode)
                {
                    case Keys.Oem8: cur_Buttons.Add(Wavy); break;
                    case Keys.D0: cur_Buttons.Add(Num0); break;
                    case Keys.D1: cur_Buttons.Add(Num1); break;
                    case Keys.D2: cur_Buttons.Add(Num2); break;
                    case Keys.D3: cur_Buttons.Add(Num3); break;
                    case Keys.D4: cur_Buttons.Add(Num4); break;
                    case Keys.D5: cur_Buttons.Add(Num5); break;
                    case Keys.D6: cur_Buttons.Add(Num6); break;
                    case Keys.D7: cur_Buttons.Add(Num7); break;
                    case Keys.D8: cur_Buttons.Add(Num8); break;
                    case Keys.D9: cur_Buttons.Add(Num9); break;
                    case Keys.OemMinus: cur_Buttons.Add(OpMinus); break;
                    case Keys.Oemplus: cur_Buttons.Add(OpPlus); break;
                    case Keys.Back: cur_Buttons.Add(Backspace); break;
                    case Keys.Tab: cur_Buttons.Add(Space); break;
                    case Keys.Q: cur_Buttons.Add(Q); break;
                    case Keys.W: cur_Buttons.Add(W); break;
                    case Keys.E: cur_Buttons.Add(E); break;
                    case Keys.R: cur_Buttons.Add(R); break;
                    case Keys.T: cur_Buttons.Add(T); break;
                    case Keys.Y: cur_Buttons.Add(Y); break;
                    case Keys.U: cur_Buttons.Add(U); break;
                    case Keys.I: cur_Buttons.Add(I); break;
                    case Keys.O: cur_Buttons.Add(O); break;
                    case Keys.P: cur_Buttons.Add(P); break;
                    case Keys.OemOpenBrackets: cur_Buttons.Add(OpenBrackets); break;
                    case Keys.Oem6: cur_Buttons.Add(CloseBrackets); break;
                    case Keys.Return: cur_Buttons.Add(Ntr); break;
                    case Keys.Capital: cur_Buttons.Add(CapsLock); break;
                    case Keys.A: cur_Buttons.Add(A); break;
                    case Keys.S: cur_Buttons.Add(S); break;
                    case Keys.D: cur_Buttons.Add(D); break;
                    case Keys.F: cur_Buttons.Add(F); break;
                    case Keys.G: cur_Buttons.Add(G); break;
                    case Keys.H: cur_Buttons.Add(H); break;
                    case Keys.J: cur_Buttons.Add(J); break;
                    case Keys.K: cur_Buttons.Add(K); break;
                    case Keys.L: cur_Buttons.Add(L); break;
                    case Keys.Oem1: cur_Buttons.Add(SemiColon); break;
                    case Keys.Oemtilde: cur_Buttons.Add(Quotes); break;
                    case Keys.Oem7: cur_Buttons.Add(VerticalLine); break;
                    case Keys.LShiftKey: cur_Buttons.Add(LShift); break;
                    case Keys.Z: cur_Buttons.Add(Z); break;
                    case Keys.X: cur_Buttons.Add(X); break;
                    case Keys.C: cur_Buttons.Add(C); break;
                    case Keys.V: cur_Buttons.Add(V); break;
                    case Keys.B: cur_Buttons.Add(B); break;
                    case Keys.N: cur_Buttons.Add(N); break;
                    case Keys.M: cur_Buttons.Add(M); break;
                    case Keys.Oemcomma: cur_Buttons.Add(Comma); break;
                    case Keys.OemPeriod: cur_Buttons.Add(Dot); break;
                    case Keys.OemQuestion: cur_Buttons.Add(QuestionMark); break;
                    case Keys.RShiftKey: cur_Buttons.Add(RShift); break;
                    case Keys.LControlKey: cur_Buttons.Add(LCtrl); break;
                    case Keys.LMenu: cur_Buttons.Add(LAlt); break;
                    case Keys.Space: cur_Buttons.Add(Space); break;
                    case Keys.RMenu: cur_Buttons.Add(RAlt); break;
                    case Keys.RControlKey: cur_Buttons.Add(RCtrl); break;
                }

            }

        }

        // Event handler for exit button(CLICK)
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            // Stop date-time timer
            DateTimer.Stop();
            // Exit from app
            Application.Exit();
        }

        // Event handler for layout button(CLICK)
        private void LayoutBtn_Click(object sender, EventArgs e)
        {
            // Declare rgb color values
            int r1_fore, g1_fore, b1_fore; // Change stat's & menu button's fore color
            int r1_back, g1_back, b1_back; // Change stat's & menu button's background color
            int r2_fore, g2_fore, b2_fore; // Change stat's & menu button's fore color

            if (change_layout)
            {
                // Update layout's color & font
                r1_fore = 34; g1_fore = 32; b1_fore = 32;
                r1_back = 240; g1_back = 231; b1_back = 231;
                r2_fore = 226; g2_fore = 62; b2_fore = 55;
                DateLabel.Font = new Font(DateLabel.Font, FontStyle.Bold);
                TimeLabel.Font = new Font(DateLabel.Font, FontStyle.Bold);
                change_layout = false;
            }
            else
            {
                // Reset layout's color & font 
                r1_fore = 100; g1_fore = 98; b1_fore = 91;
                r1_back = 34; g1_back = 32; b1_back = 32;
                r2_fore = 226; g2_fore = 137; b2_fore = 137;
                DateLabel.Font = new Font(DateLabel.Font, FontStyle.Italic);
                TimeLabel.Font = new Font(DateLabel.Font, FontStyle.Italic);
                change_layout = true;
            }
            this.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            // Update Statistics fore & back color
            Speed.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Speed.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Stats_Speed.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Stats_Speed.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Accuracy.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Accuracy.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Stats_Accuracy.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Stats_Accuracy.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Score.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Score.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Stats_Score.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Stats_Score.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            CurrentKey.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            CurrentKey.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Stats_CurrentKey.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Stats_CurrentKey.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            Stats_DailyGoal.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            Stats_DailyGoal.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            // Update Menu's options fore & back color
            PracticeBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            PracticeBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            ProfileBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            ProfileBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            TypingTestBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            TypingTestBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            HelpBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            HelpBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            HighScoresBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            HighScoresBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            TextToolsBtn.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            TextToolsBtn.ForeColor = Color.FromArgb(r1_fore, g1_fore, b1_fore);
            // Update SampleTextBox & Date&TimeLabel's back & fore color
            SampleTextBox.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            DateLabel.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            TimeLabel.BackColor = Color.FromArgb(r1_back, g1_back, b1_back);
            // Update Stat's, UserName, Date&TimeLabel fore color
            Statistics.ForeColor = Color.FromArgb(r2_fore, b2_fore, g2_fore);
            DateLabel.ForeColor = Color.FromArgb(r2_fore, b2_fore, g2_fore);
            TimeLabel.ForeColor = Color.FromArgb(r2_fore, b2_fore, g2_fore);
            UserName.ForeColor = Color.FromArgb(r2_fore, b2_fore, g2_fore);
        }

        // Event handler for SampleTextBox's mouse focus(LEAVE)
        private void SampleTextBox_MouseLeave(object sender, EventArgs e)
        {
            // Stop keyboard timer
            KeyboardTimer.Stop();
            // Empty cur_Buttons' list
            cur_Buttons.Clear();
            // Clear previous text
            SampleTextBox.Clear();
            // Manipulate user's prompt
            SampleTextBox.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, (byte)161);
            SampleTextBox.Text = "Click here to start typing...";
        }

        // Event handler for SampleTextBox's mouse focus(DOWN)
        private void SampleTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            // Start keyboard timer
            KeyboardTimer.Start();
            // Initialize cur_Buttons' list
            cur_Buttons = new List<Button>();
            // Clear previous text
            SampleTextBox.Clear();
            // Initialize SampleTextBox's text with a my_quotes[i], i (e) [1, 12]
            SampleTextBox.Font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point, (byte)161);
            SampleTextBox.Text = NewQuote();
        }

        // Event handler for Date&TimeLabel(TICK)
        private void DateTimer_Tick(object sender, EventArgs e)
        {
            // Update current date
            DateLabel.Text = DateTime.Now.Date.ToString();
            // Update current time
            TimeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        // Event handler for Keyboard(TICK)
        private void KeyTimer_Tick(object sender, EventArgs e)
        {
            ResetButtonColor();
        }

        // Returns a new quote
        private string NewQuote() => my_quotes[new Random().Next(0, my_quotes.Count - 1)];
    }
    #endregion

}