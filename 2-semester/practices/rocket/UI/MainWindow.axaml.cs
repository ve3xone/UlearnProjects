using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace func_rocket.UI;

public partial class MainWindow : Window
{
	private readonly DispatcherTimer timer;
	private int iterationIndex;
	private readonly string helpText;
	private readonly RocketModel model;

	public MainWindow()
	{
		InitializeComponent();

		helpText = "Use A and D to control rocket";
		Title = helpText;

		timer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(10)
		};
		timer.Tick += TimerTick;

		model = new RocketModel();

		Canvas.Model = model;
		Canvas.RocketImage = Rocket;
		SetLevels(LevelsTask.CreateLevels());
	}


	public void SetLevels(IEnumerable<Level> levels)
	{
		timer.Start();

		model.SetLevel(levels.First());
		foreach (var level in levels)
		{
			var button = new Button
			{
				Content = level.Name
			};
			button.Click += (_, _) => ChangeLevel(level);

			Buttons.Children.Add(button);
		}
	}

	private void ChangeLevel(Level newSpace)
	{
		model.SetLevel(newSpace);
		timer.Start();
		iterationIndex = 0;
	}

	private void TimerTick(object sender, EventArgs e)
	{
		model.MoveRocket();
		if (model.CurrentLevel.IsCompleted)
			timer.Stop();
		else
			Title = helpText + ". Iteration # " + iterationIndex++;

		Canvas.InvalidateVisual();
	}
	
	protected override void OnKeyDown(KeyEventArgs e)
	{
		base.OnKeyDown(e);
		HandleKey(e.Key, true);
	}

	private void HandleKey(Key e, bool down)
	{
		switch (e)
		{
			case Key.A:
				model.Left = down;
				break;
			case Key.D:
				model.Right = down;
				break;
		}
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);
		HandleKey(e.Key, false);
	}
}