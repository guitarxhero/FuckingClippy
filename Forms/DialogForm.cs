﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

//TODO: Add support for:
// - Images
// - Choices (radio buttons?)

namespace FuckingClippy
{
    partial class MainForm
    {

    }

    /// <summary>
    /// Dialog system.
    /// </summary>
    static class Dialog
    {
        // A reference to the parent form that summons thee.
        internal static Form ParentForm;
        // The current active bubble text.
        internal static Form CurrentForm;
        static Color BubbleColor = Color.FromArgb(255, 255, 204);
        static Font DefaultFont = new Font("Segoe UI", 9);
        static Image BubbleTail = Image.FromStream(
            System.Reflection.Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(
                "FuckingClippy.Images.Bubble.BubbleTail.png"));

        #region Prompt
        /// <summary>
        /// Prompt the user and ask him what he wants for christmas.
        /// </summary>
        internal static void Prompt()
        {
            //TODO: Make Prompt() return a string
            Console.WriteLine($"CLR: Prompt() called -- {DefaultFont.Name}");

            CurrentForm = GetBaseForm(GetPrompt()/*, new Size(206,72)*/);

            CurrentForm.Show();
        }

        static Control[] GetPrompt()
        {
            List<Control> lst = new List<Control>();

            Label l = new Label();
            l.AutoSize = true;
            l.Text = "What would you like to do?";
            l.Location = new Point(4, 8);
            l.Font = DefaultFont;
            l.Margin = new Padding(4);

            TextBox t = new TextBox();
            t.Multiline = true;
            t.Size = new Size(194, 34);
            t.Font = DefaultFont;
            t.Location = new Point(4, 32);
            t.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
                {
                    Utils.ProcessInput(t.Text);
                }
            };

            lst.Add(l);
            lst.Add(t);

            return lst.ToArray();
        }
        #endregion

        #region Say
        /// <summary>
        /// Say something to the user.
        /// </summary>
        /// <param name="pText">Text.</param>
        internal static void Say(string pText)
        {
            Console.WriteLine($"CLR: Say({pText}) called -- {DefaultFont.Name}");

            CurrentForm = GetBaseForm(GetSay(pText)/*, GetSizeWithText(pText)*/);

            CurrentForm.Show();
        }

        internal static void SayRandom()
        {
            string[] str =
            {
                "I heard you like pies.",
                "Would you like help with hugging yourself?",
                "I got you a gift for christmas. It's called a kernel panic! Let me show you!",
                "test123 guys can you hear me",
                "foo dick",
                "Looks like you're trying to focus, need some help?",
                "I hope you're enjoying staring at my body.",
                "WHY DOESN'T EMOJIS WORK HUH",
                "I'm bored.",
                "DUDE WHERE'S MY GAC? >:-(",
                "Meow."
            };

            Say(str[new Random().Next(0, str.Length)]);
        }

        static Control[] GetSay(string pText)
        {
            List<Control> lst = new List<Control>();

            Label l = new Label();
            l.Location = new Point(4, 6);
            //l.Size = new Size(192, 1000);
            l.MaximumSize = new Size(192, 0);
            l.AutoSize = true;
            l.Text = pText;
            l.Font = DefaultFont;

            lst.Add(l);

            return lst.ToArray();
        }

        static Size GetSizeWithText(string pData)
        {
            //TODO*: Find the perfect Height sizing algorithm
            return new Size(200,
                12 + (((pData.Length / 25) + 1) * ((int)DefaultFont.Size * 2)));
        }
        #endregion

        #region Bases
        static DialogForm GetBaseForm(Control[] pSubControls/*, Size pClientSize*/)
        {
            CurrentForm.Close();

            DialogForm f = new DialogForm();
            f.Font = DefaultFont;
            f.Deactivate += (s, e) =>
            {
                f.Close();
            };

            /* Bubble body */
            Panel p = new Panel();
            p.BackColor = BubbleColor;
            p.BorderStyle = BorderStyle.FixedSingle;
            p.Controls.AddRange(pSubControls);
            p.AutoSize = true;
            p.MaximumSize = new Size(200, 0);
            p.Location = new Point(0, 0);
            //p.Size = pClientSize;

            /* Bubble tail */
            PictureBox pb = new PictureBox();
            pb.Size = new Size(10, 15);
            pb.Location = new Point((int)(f.ClientSize.Width / 1.62),
                f.ClientSize.Height - 15);
            pb.Image = BubbleTail;

            f.Controls.Add(p);
            f.Controls.Add(pb);

            f.AutoSize = false;
            f.ClientSize =
                new Size(f.Width, f.Height + 15);
            f.Location =
                new Point(ParentForm.Location.X - (f.Size.Width / 2),
                ParentForm.Location.Y - (f.Size.Height - 4) - 30);

            return f;
        }
        #endregion
    }

    class DialogForm : TransparentForm
    {
        public DialogForm() : base()
        {
            TopMost = true;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;

            AutoSize = true;
            MaximumSize = new Size(200, 0);
            AutoSizeMode = AutoSizeMode.GrowOnly;
        }
    }
}
