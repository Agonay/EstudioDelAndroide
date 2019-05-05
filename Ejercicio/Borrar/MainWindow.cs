using System;
using System.Diagnostics;
using Gtk;

public partial class MainWindow : Gtk.Window
{
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Stopwatch ts = new Stopwatch();
        ts.Start();
        texto.Text = CuentaSeg(ts);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
    protected string CuentaSeg(Stopwatch sw)
    {
        TimeSpan ts= sw.Elapsed;
        return string.Format("{0}:{1}", Math.Floor(ts.TotalMinutes), ts.ToString("ss\\.ff"));
    }
}
