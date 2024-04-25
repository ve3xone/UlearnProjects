using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Rivals.Dungeons;

namespace Rivals.UI;

public partial class MainWindow : Window
{
	private readonly DispatcherTimer timer;

	public MainWindow()
	{
		InitializeComponent();
		var levels = LoadLevels().ToArray();
		ScenePainter.LoadMaps(levels);
		CreateLayout(levels);

		timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
		timer.Tick += TimerTick;
		timer.Start();
	}

	private static IEnumerable<Map> LoadLevels()
	{
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon1));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon2));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon3));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon4));
		yield return Map.FromText(DungeonsLoader.Load(DungeonsName.Dungeon5));
	}

	private void CreateLayout(Map[] levels)
	{
		DrawLevelSwitch(levels, Buttons);
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
		var linkLabels = new List<Button>();
		for (var i = 0; i < levels.Length; i++)
		{
			var level = levels[i];
			var link = new Button
			{
				Content = $"Level {i + 1}",
				Foreground = Brushes.LimeGreen,
				Margin = new Thickness(8),
				Tag = level
			};
			link.Click += (_, __) =>
			{
				ChangeLevel(level);
				UpdateLinksColors(level, linkLabels);
			};
			menuPanel.Children.Add(link);
			linkLabels.Add(link);
		}

		UpdateLinksColors(levels[0], linkLabels);
	}

	private void ChangeLevel(Map newMap)
	{
		ScenePainter.ChangeLevel(newMap);
		timer.Start();
	}

	private void UpdateLinksColors(Map level, List<Button> buttons)
	{
		foreach (var button in buttons)
			button.Foreground = button.Tag == level ? Brushes.LimeGreen : Brushes.White;
	}

	private void TimerTick(object sender, EventArgs e)
	{
		ScenePainter.Update();
	}
}