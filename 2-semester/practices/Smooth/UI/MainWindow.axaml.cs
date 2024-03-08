using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;

namespace yield.UI;

public partial class MainWindow : Window
{
    private Thread thread;
    private readonly DispatcherTimer timer;
    private volatile bool paused;
    private volatile bool canceled;

    public MainWindow()
    {
        InitializeComponent();
        Closing += (_, _) => { canceled = true; };
        KeyDown += (_, _) => { paused = !paused; };
        StartAddingPoints();
        timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };

        timer.Tick += (_, _) => { Graph.InvalidatePlot(); };
        timer.Start();
    }

    private void StartAddingPoints()
    {
        var model = DataContext as IGraphModel;

        thread = new Thread(() =>
            {
                try
                {
                    foreach (var point in DataSource.GetData(new Random()))
                    {
                        if (canceled) return;
                        var pointCopy = point;
                        model.AddPoint(pointCopy);
                        while (paused && !canceled) Thread.Sleep(20);
                        Thread.Sleep(20);
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                }
            })
            { IsBackground = true };
        
        thread.Start();
    }
}