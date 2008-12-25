﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using GitCommands;
using System.Drawing.Drawing2D;

namespace GitUI
{
    public partial class RevisionGrid : UserControl
    {
        public RevisionGrid()
        {
            InitializeComponent();
            Revisions.SelectionChanged += new EventHandler(Revisions_SelectionChanged);

            NormalFont = Revisions.Font;
            HeadFont = new Font(NormalFont, FontStyle.Bold);
            RefreshRevisions();
        }

        public Font NormalFont { get; set; }
        public Font HeadFont { get; set; }

        public event EventHandler SelectionChanged;

        public void SetSelectedRevision(GitRevision revision)
        {
            Revisions.ClearSelection();

            if (revision != null)
                {
                    foreach (DataGridViewRow row in Revisions.Rows)
                    {
                        if (((GitRevision)row.DataBoundItem).Guid == revision.Guid)
                            row.Selected = true;
                    }
                }
            Revisions.Select();
        }

        void Revisions_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }

        public List<GitRevision> GetRevisions()
        {
            List<GitRevision> retval = new List<GitRevision>();
            foreach (DataGridViewRow row in Revisions.SelectedRows)
            {
                if (row.DataBoundItem is GitRevision)
                {
                    retval.Add((GitRevision)row.DataBoundItem);
                }
            }
            return retval;
        }


        protected Bitmap graphImage;



        public void RefreshRevisions()
        {
            string currentCheckout = new GitCommands.GitCommands().GetCurrentCheckout();

            List<GitRevision> revisions = new GitCommands.GitCommands().GitRevisionGraph();

            {
                Revisions.DataSource = revisions;
                Revisions.CellPainting += new DataGridViewCellPaintingEventHandler(Revisions_CellPainting);


                int height = Revisions.RowTemplate.Height;
                int width = 8;
                int y = -height;

                graphImage = new Bitmap(1000, (revisions.Count * height) + 50, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                Graphics graph = Graphics.FromImage(graphImage);
                //graph.Clear(Color.White);

                string lastlastLine = "";
                string lastLine = "";
                string currentLine = "";
                
                Pen linePen = new Pen(Color.Red, 2);

                for (int r = 0; r < revisions.Count; r++)
                {
                    GitRevision revision = revisions[r];

                    GitRevision prevRevision = null;
                    GitRevision nextRevision = null;

                    if (r > 0)
                        prevRevision = revisions[r - 1];
                    if (revisions.Count > r + 1)
                        nextRevision = revisions[r + 1];

                    y += height;
                    int nLine = 0;

                    char[] calc = new char[100];

                    for (int x = 0; x < 100; x++)
                    {
                        calc[x] = '|';
                    }

                    for (int n = 0; n < revision.GraphLines.Count+1; n++)
                    {
                        string nextLine = "";

                        if (n < revision.GraphLines.Count)
                        {
                            nextLine = revision.GraphLines[n];
                        }
                        else
                        {
                            if (nextRevision != null)
                                nextLine = nextRevision.GraphLines[0];
                        }


                        nLine++;

                        int x = 0;
                        for (int nc = 0; nc < currentLine.Length; nc++)
                        {

                            x += width;

                            char c = currentLine[nc];
                            int top = y;
                            int bottom = y + height;
                            int left = x;
                            int right = x + width;
                            int hcenter = x + (width / 2);
                            int vcenter = y + (height / 2);

                            if (c == '*')
                            {
                                if (revision.Guid == currentCheckout)
                                    graph.FillEllipse(new SolidBrush(Color.Blue), hcenter - 5, vcenter - 5, 9, 9);
                                else
                                    graph.FillEllipse(new SolidBrush(Color.Red), hcenter - 4, vcenter - 4, 7, 7);

                                if (/*r == 0 &&*/ nextRevision != null && nextRevision.GraphLines[0].Length > nc && (nextRevision.GraphLines[0][nc] == '|' || nextRevision.GraphLines[0][nc] == '*'))
                                {
                                    if (r == 0)
                                        graph.DrawLine(linePen, hcenter, vcenter, hcenter, bottom + 1);
                                    else
                                        if (nextLine != null && nextLine.Length > nc && nextLine[nc] == '|')
                                            graph.DrawLine(linePen, hcenter, vcenter, hcenter, bottom + (height / 2) + 1);
                                }
                            }
                            if (c != '|' && c != '*')
                            {
                                calc[nc] = ' ';
                            }
                            if (c == '\\' && nc % 2 == 1)
                            {
                                if ((nextLine.Length > nc && nextLine[nc] == '/' || nextLine.Length <= nc) ||
                                    (lastLine.Length > nc && lastLine[nc] == '/' || lastLine.Length <= nc))
                                {
                                    if (lastLine.Length > nc && lastLine[nc] == '/' || lastLine.Length <= nc)
                                    {
                                        if (nextLine.Length > nc+1 && nextLine[nc+1] == '|' || nextLine.Length <= nc+1)
                                            graph.DrawLine(linePen, left - (width / 2), vcenter, left - (width / 2), bottom + (height / 2)+1);
                                    }
                                }
                                else
                                {
                                    if ((nextLine.Length > nc + 2 && nextLine[nc + 2] != '\\') || nextLine.Length <= nc + 2)
                                    {
                                        //draw: 
                                        //      \
                                        graph.DrawLine(linePen, right, bottom, right + (width / 2), bottom + (height / 2));
                                    }
                                    if (nc - 2 >= 0 && lastLine.Length > (nc - 2) && lastLine[nc - 2] == '\\')
                                    {
                                        //draw: _
                                        graph.DrawLine(linePen, left - width, bottom, right + 1, bottom);
                                    }
                                    else
                                    {
                                        // draw: \_
                                        graph.DrawLine(linePen, left - (width / 2), vcenter, left, bottom);
                                        graph.DrawLine(linePen, left, bottom, right + 1, bottom);
                                    }
                                }
                            }
                            if (c == '/' && nc % 2 == 1)
                            {
                                if ((nextLine.Length > nc && nextLine[nc] == '\\' || nextLine.Length <= nc) ||
                                    (lastLine.Length > nc && lastLine[nc] == '\\' || lastLine.Length <= nc))
                                {
                                    if (lastLine.Length > nc && lastLine[nc] == '\\' || lastLine.Length <= nc)
                                    {
                                        if (nextLine.Length > nc-1 && nextLine[nc-1] == '|' || nextLine.Length <= nc-1)
                                            graph.DrawLine(linePen, left - (width / 2), vcenter, left - (width / 2), bottom + (height / 2)+1);
                                    }
                                }
                                else
                                {



                                    if ((lastLine.Length > nc + 2 && lastLine[nc + 2] != '/' || lastLine.Length <= nc + 2) ||
                                        (lastLine.Length > nc + 2 && lastLine[nc + 2] == '/' &&
                                         lastlastLine.Length > nc + 2 && lastlastLine[nc + 2] == '\\'))
                                    {
                                        //draw: /
                                        //      
                                        graph.DrawLine(linePen, right, bottom, right + (width / 2), bottom - (height / 2));
                                    }
                                    if (nc - 2 >= 0 && nextLine.Length > (nc - 2) && nextLine[nc - 2] == '/')
                                    {
                                        //draw: _
                                        //      
                                        graph.DrawLine(linePen, left - width, bottom, right + 1, bottom);
                                    }
                                    else
                                    {
                                        //draw:  _
                                        //      /
                                        graph.DrawLine(linePen, left - (width / 2), bottom + (height / 2), left, bottom);
                                        graph.DrawLine(linePen, left, bottom, right + 1, bottom);
                                    }
                                }
                            }

                            if (n == revision.GraphLines.Count - 1)
                            {
                                char prevChar = ' ';
                                char currentChar = calc[nc];
                                char nextChar = ' ';

                                if (prevRevision != null && prevRevision.GraphLines[prevRevision.GraphLines.Count - 1].Length > nc)
                                    prevChar = prevRevision.GraphLines[prevRevision.GraphLines.Count - 1][nc];

                                if (nextRevision != null && nextRevision.GraphLines[0].Length > nc)
                                    nextChar = nextRevision.GraphLines[0][nc];

                                if ((prevChar == '|' && currentChar == '|') || (prevChar == '|' && currentChar == '*'))
                                {
                                    graph.DrawLine(linePen, hcenter, top + (height / 2), hcenter, vcenter + (height / 2) + 1);
                                }
                                if ((nextChar == '|' && currentChar == '|') || (nextChar == '*' && currentChar == '|'))
                                {
                                    graph.DrawLine(linePen, hcenter, vcenter + (height / 2), hcenter, bottom + (height / 2) + 1);
                                }

                            }
                        }
                        lastlastLine = lastLine;
                        lastLine = currentLine;
                        currentLine = nextLine;
                    }
                }
            }
        }

        void Revisions_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.State & DataGridViewElementStates.Visible) != 0)
            {
                e.Handled = true;

                if ((e.State & DataGridViewElementStates.Selected) != 0)
                    //e.Graphics.FillRectangle(new SolidBrush(Revisions.RowTemplate.DefaultCellStyle.SelectionBackColor), e.CellBounds);
                    e.Graphics.FillRectangle(new LinearGradientBrush(e.CellBounds, Revisions.RowTemplate.DefaultCellStyle.SelectionBackColor, Color.LightBlue, 90, false), e.CellBounds);
                else
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.CellBounds);

                if (e.ColumnIndex == 0)
                {
                    e.Graphics.DrawImage(graphImage, e.CellBounds, new Rectangle(0, e.RowIndex * Revisions.RowTemplate.Height, e.CellBounds.Width, Revisions.RowTemplate.Height), GraphicsUnit.Pixel);
                }
                else
                    if (e.ColumnIndex == 1)
                    {
                        if (e.Value is string)
                        {
                            GitRevision revision = (GitRevision)Revisions.Rows[e.RowIndex].DataBoundItem;
                            float offset = 0;
                            foreach (GitHead h in revision.Heads)
                            {
                                SolidBrush brush = new SolidBrush(h.IsTag == true ? Color.DarkBlue : h.IsHead ? Color.DarkRed : h.IsRemote ? Color.Green : Color.Gray);

                                e.Graphics.DrawString("[" + h.Name + "] ", HeadFont, brush, new PointF(e.CellBounds.Left + offset, e.CellBounds.Top + 4));

                                offset += e.Graphics.MeasureString("[" + h.Name + "] ", HeadFont).Width;
                            }
                            string text = (string)e.Value;
                            e.Graphics.DrawString(text, NormalFont, new SolidBrush(Color.Black), new PointF(e.CellBounds.Left + offset, e.CellBounds.Top + 4));
                        }
                    }
                    else
                    {
                        if (e.Value is string)
                        {
                            string text = (string)e.Value;
                            e.Graphics.DrawString(text, NormalFont, new SolidBrush(Color.Black), new PointF(e.CellBounds.Left, e.CellBounds.Top + 4));
                        }
                    }
            }

        }

        private void Revisions_DoubleClick(object sender, EventArgs e)
        {
            List<GitRevision> r = GetRevisions();
            if (r.Count > 0)
                new FormDiff(r[0]).ShowDialog();
            else
                new FormDiff().ShowDialog();
        }

        private void Revisions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
