#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;
using HeuristicLab.MainForm.WindowsForms;

namespace CartesianGeneticProgramming.Views {
  [View("CGP Solution View")]
  [Content(typeof(ICGPModel), true)]
  public sealed partial class SolutionProgramView : ItemView {
    public new CGPModel Content {
      get { return (CGPModel)base.Content; }
      set { base.Content = value; }
    }

    private const string cgp_image_filename = "cgp.bmp";
    private const string simplified_cgp_image_filename = "cgp_simplified.bmp";

    public SolutionProgramView() {
      InitializeComponent();
    }

    protected override void OnContentChanged() {
      base.OnContentChanged();
      if (Content?.Graph != null) {
        LoadImages();
        DrawGraph();
      }
    }

    public void LoadImages() {
      this.pictureBox.Image?.Dispose();

      GraphVizRenderer.RenderGraph(Content.Graph);
    }

    #region methods for painting the graph
    private void DrawGraph() {
      using (var img = new Bitmap(hideInactiveToolStripMenuItem.Checked ?
          LoadImage(simplified_cgp_image_filename) :
          LoadImage(cgp_image_filename))) {
        this.pictureBox.Image = new Bitmap(img);
      }
      this.pictureBox.Refresh();
    }

    private Image LoadImage(string filename) {
      if (File.Exists(filename)) {
        return Image.FromFile(filename);
      }
      return new Bitmap(Width, Height);
    }
    #endregion

    #region hide/view inactive nodes
    private void hideInactiveToolStripMenuItem_Click(object sender, EventArgs e) {
      var menuItem = (ToolStripMenuItem)sender;
      menuItem.Checked = !menuItem.Checked;
      DrawGraph();
    }
    #endregion


    #region save image
    private void saveImageToolStripMenuItem_Click(object sender, EventArgs e) {
      if (saveFileDialog.ShowDialog() == DialogResult.OK) {
        string filename = saveFileDialog.FileName.ToLower();
        SaveImageAsBitmap(filename);
      }
    }

    public void SaveImageAsBitmap(string filename) {
      if (Content == null) return;
      this.pictureBox.Image?.Save(filename);
    }

    #endregion
  }
}
