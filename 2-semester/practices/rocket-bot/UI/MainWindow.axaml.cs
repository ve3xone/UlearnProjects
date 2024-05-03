using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace rocket_bot.UI;

public partial class MainWindow : Window
{
	private readonly TimeSpan interval = TimeSpan.FromMilliseconds(20);
	private RocketModel model;

	private const string HelpText = "Use A, W and D to control rocket";

	private int lastChannelCount;
	private bool paused;
	private DispatcherTimer timer;
	private Button pauseButton;
	private Button fastButton;
	private Button slowButton;

	public MainWindow()
	{
		InitializeComponent();
		
		var channel = new Channel<Rocket>();
		var random = new Random(223243);
		var level = LevelsFactory.CreateLevel(random);
		channel.AppendIfLastItemIsUnchanged(level.InitialRocket, null);
		var bot = new Bot(level.Clone(), channel, 45, 1000, random, 2);
		var thread = new Thread(() => bot.RunInfiniteLoop()) { IsBackground = true };
		thread.Start();

		SetModel(new RocketModel(level, channel));
		
		Title = HelpText;
		Canvas.RocketImage = RocketImage;

		RewindTrackBar.PropertyChanged += (sender, e) =>
		{
			if (Math.Abs(model.Rocket.Time - RewindTrackBar.Value) > 1)
				model.RewindTo(RewindTrackBar.Value);
		};
		RewindTrackBar.PointerEnter += (sender, e)
			=> SetPause(true);

		RewindTrackBar.PointerLeave += (sender, e)
			=> SetPause(false);

		FillPlayButtonsPanel();
	}

	public void SetModel(RocketModel newModel)
	{
		model = newModel;
		Canvas.model = model;
		
		RewindTrackBar.Maximum = Level.MaxTicksCount;

		timer = new DispatcherTimer { Interval = interval };
		timer.Tick += (_, __) => TimerTick();
		timer.Start();
	}

	private void FillPlayButtonsPanel()
	{
		fastButton = new Button
		{
			Content = "Fast (5x)",
			IsTabStop = false
		};
		slowButton = new Button
		{
			Content = "Normal (1x)",
			IsTabStop = false,
			IsEnabled = false
		};

		fastButton.Click += (_, __) =>
			SetPlaySpeed(5);

		slowButton.Click += (_, __) =>
			SetPlaySpeed(1);

		pauseButton = new Button
		{
			Content = "Pause",
			IsTabStop = false
		};
		pauseButton.Click += (_, __) =>
			SetPause(!paused);

		var restartButton = new Button
		{
			Content = "Restart",
			IsTabStop = false
		};
		restartButton.Click += (_, __) => Restart();

		Buttons.Children.Add(restartButton);
		Buttons.Children.Add(pauseButton);
		Buttons.Children.Add(slowButton);
		Buttons.Children.Add(fastButton);
	}

	private void SetPause(bool paused)
	{
		this.paused = paused;
		pauseButton.Content = paused ? "Unpause" : "Pause";
	}

	private void SetPlaySpeed(int speed)
	{
		model.SetPlaySpeed(speed);
		SetPause(false);
		fastButton.IsEnabled = speed != 5;
		slowButton.IsEnabled = speed != 1;
	}

	private void TimerTick()
	{
		var (level, rocket, channel) = model.Data;

		if (!paused)
		{
			RewindTrackBar.Value = rocket.Time;
			model.MoveRocket();
		}

		var precalculationSpeed =
			Math.Max(0, (channel.Count - lastChannelCount) * Level.MaxTicksCount / interval.Milliseconds);
		lastChannelCount = channel.Count;

		Title =
			$"{HelpText}. Iteration # {rocket.Time} Checkpoints taken: {rocket.TakenCheckpointsCount}. Ticks precalculated: {channel.Count}. Precalculation speed: {precalculationSpeed} ticks per second.";

		var precalculatedPercent = (channel.Count - 1) * 100f / Level.MaxTicksCount;
		Status.Text = $"Precalculated: {precalculatedPercent}%";

		Precalculation.Width = Width * (channel.Count - 1) / Level.MaxTicksCount;

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
			case Key.A or Key.Left:
				model.SetManualControl(Turn.Left, down);
				SetPlaySpeed(1);
				break;
			case Key.D or Key.Right:
				model.SetManualControl(Turn.Right, down);
				SetPlaySpeed(1);
				break;
			case Key.W or Key.Up:
				model.SetManualControl(Turn.None, down);
				SetPlaySpeed(1);
				break;
			case Key.R:
				Restart();
				break;
		}
	}

	private void Restart()
	{
		model.Channel[0] = model.CurrentLevel.InitialRocket;
		model.RewindTo(0);
		SetPause(false);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyDown(e);
		HandleKey(e.Key, false);
	}
}