using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using Dungeon.Dungeons;
using Brushes = Avalonia.Media.Brushes;

namespace Dungeon.UI;

public partial class MainWindow : Window
{
	private readonly DispatcherTimer timer;

	public MainWindow()
	{
		InitializeComponent();
		Title = "Click on any empty cell to run BFS";
		var levels = LoadLevels().ToArray();
		ScenePainter.Load(levels);
		FontSize = 16;
		DrawLevelSwitch(levels, ButtonsPanel);
		timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
		timer.Tick += TimerTick;
		timer.Start();
	}

	private static IEnumerable<Map> LoadLevels()
	{
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon1));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon2));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon3));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon4));
	}


	private void DrawLevelSwitch(Map[] levels, Panel menuPanel)
	{
		menuPanel.Children.Add(new Label
		{
			Content = "Choose level:",
			Foreground = Brushes.White,
			Margin = new Thickness(8),
			VerticalAlignment = VerticalAlignment.Center
		});

		var buttons = new List<Button>();
		for (var i = 0; i < levels.Length; i++)
		{
			var level = levels[i];
			var button = new Button
			{
				Content = $"Level {i + 1}",
				Foreground = Brushes.LimeGreen,
				Margin = new Thickness(8),
				Tag = level
			};
			button.Click += (_, __) =>
			{
				ChangeLevel(level);
				UpdateButtonsColors(level, buttons);
			};
			menuPanel.Children.Add(button);
			buttons.Add(button);
		}

		UpdateButtonsColors(levels[0], buttons);
	}

	private void UpdateButtonsColors(Map level, List<Button> linkLabels)
	{
		foreach (var linkLabel in linkLabels)
		{
			linkLabel.Foreground = linkLabel.Tag == level ? Brushes.LimeGreen : Brushes.White;
		}
	}

	private void ChangeLevel(Map newMap)
	{
		ScenePainter.ChangeLevel(newMap);
		timer.Start();
	}

	private void TimerTick(object sender, EventArgs e)
	{
		ScenePainter.Update();
	}
}