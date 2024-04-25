using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Greedy.Architecture;
using Greedy.UI.Drawing;

namespace Greedy.UI;

public partial class MainWindow : Window
{
	private IPathFinder currentPathFinder;
	private State currentState;
	private readonly ScenePainter scenePainter;
	private DispatcherTimer timer;

	private int TimerInterval
	{
		get => timerIntervalInMs;
		set => timerIntervalInMs = Math.Max(1, value);
	}

	private int timerIntervalInMs = 300;

	public MainWindow()
	{
		InitializeComponent();
		scenePainter = new ScenePainter(Canvas);
		timer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(timerIntervalInMs)
		};
		timer.Tick += (_, __) =>
		{
			timer.Interval = TimeSpan.FromMilliseconds(timerIntervalInMs);
			if (!scenePainter.Controller.TryMoveOneStep())
				timer.Stop();

			InvalidateViews();
		};

		InitializeStates();
		InitializeButtons();
	}

	private void InitializeButtons()
	{
		RestartButton.Click += (_, __) => RestartCurrentStateWithCurrentPathFinder();
		UpSpeedButton.Click += (_, __) => { TimerInterval -= 100; };
		DownSpeedButton.Click += (_, __) => { TimerInterval += 100; };
		ChangeAlgButton.Click += (_, __) =>
		{
			currentPathFinder = SwitchPathFinderType();
			WritePathFinderTypeToControlsTexts(currentPathFinder, currentState.Chests.Count);
			RestartCurrentStateWithCurrentPathFinder();
		};
		NextStateButton.Click += (_, __) => { SwitchToNextState(); };
		PrevStateButton.Click += (_, __) => { SwitchToPreviousState(); };
	}


	private void InitializeStates()
	{
		States.Items = BuildDropDownItemsWithStates();
		States.SelectionChanged += (_, args) =>
		{
			var item = args.AddedItems.Count > 0
				? args.AddedItems[0]
				: args.RemovedItems[0];
			if (item != null)
				ChangeState((string)item);
		};
		States.SelectedIndex = 0;
	}

	private static string[] BuildDropDownItemsWithStates()
	{
		return StatesLoader
			.LoadAllStateNames(Folders.StatesForStudents)
			.Select(stateName => stateName)
			.ToArray();
	}

	private void ChangeState(string stateName)
	{
		SetCurrentPathFinderFromStateName(stateName);
		RestartCurrentStateWithCurrentPathFinder();
	}

	private void SetCurrentPathFinderFromStateName(string stateName)
	{
		if (stateName.StartsWith("not_greedy"))
			currentPathFinder = new NotGreedyPathFinder();
		else
			currentPathFinder = new GreedyPathFinder();
	}

	private void RestartCurrentStateWithCurrentPathFinder()
	{
		StatusBar.Text = "";

		timer.Stop();

		currentState = StatesLoader.LoadStateFromFolder(Folders.StatesForStudents, States.SelectedItem.ToString());
		var mutableStateForStudent = new State(currentState);
		var path = currentPathFinder.FindPathToCompleteGoal(mutableStateForStudent);
		scenePainter.Controller = new StateController(currentState, path);
		WritePathFinderTypeToControlsTexts(currentPathFinder, currentState.Chests.Count);

		if (Canvas.Height != scenePainter.RealSize.Height && Canvas.Width != scenePainter.RealSize.Width)
			Canvas.ResetScale();

		InvalidateViews();
		timer.Start();
	}

	private void InvalidateViews()
	{
		var controller = scenePainter.Controller;
		var state = controller.State;

		StatusBar.Text = $"Scores: {state.Scores} ";
		if (currentPathFinder is GreedyPathFinder)
			StatusBar.Text += $"(Goal: {state.Goal}) ";

		StatusBar.Text += $"Energy left: {state.Energy} (Out of: {state.InitialEnergy}) ";

		if (controller.GameIsWon)
			StatusBar.Text += " GAME WON !";
		else if (controller.GameIsLost)
			StatusBar.Text += controller.GameLostMessage;

		Canvas.InvalidateVisual();
	}

	private IPathFinder SwitchPathFinderType()
	{
		if (currentPathFinder is null or NotGreedyPathFinder)
			return new GreedyPathFinder();

		if (currentState.Chests.Count > 11)
			return new GreedyPathFinder();

		return new NotGreedyPathFinder();
	}

	private void WritePathFinderTypeToControlsTexts(IPathFinder pathFinder, int chestsCount)
	{
		ChangeAlgButton.IsEnabled = chestsCount <= 11;

		if (pathFinder is GreedyPathFinder)
		{
			Title = "Greedy Path Finder";
			ChangeAlgButton.Content = "Change to Not Greedy";
		}
		else
		{
			Title = "Not Greedy Path Finder";
			ChangeAlgButton.Content = "Change to Greedy";
		}

		if (!ChangeAlgButton.IsEnabled)
			ChangeAlgButton.Content = "(too many chests)";

		Title += $" | {States.SelectedItem}";
	}

	private void SwitchToNextState()
	{
		States.SelectedIndex = (States.SelectedIndex + 1) % States.ItemCount;
		ChangeState(States.SelectedItem.ToString());
	}

	private void SwitchToPreviousState()
	{
		States.SelectedIndex = States.SelectedIndex == 0
			? States.ItemCount - 1
			: States.SelectedIndex - 1;
		ChangeState(States.SelectedItem.ToString());
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		switch (e.Key)
		{
			case Key.OemPlus or Key.Add:
				TimerInterval -= 100;
				break;
			case Key.OemMinus or Key.Subtract:
				TimerInterval += 100;
				break;
			case Key.R:
				RestartCurrentStateWithCurrentPathFinder();
				break;
			case Key.N:
				SwitchToNextState();
				break;
			case Key.P:
				SwitchToPreviousState();
				break;
		}
	}
}