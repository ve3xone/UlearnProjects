using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;

namespace LimitedSizeStack.UI;

public partial class MainWindow : Window
{
	private readonly TaskListViewModel model;

	public MainWindow()
	{
		InitializeComponent();

		var list = new List<string> { "Составить список дел на сегодня", "Домашка по C#", "Решить задачу 1519" };
		var listModel = new ListModel<string>(list, 20);

		model = new TaskListViewModel(listModel);
		
		//Add
		ButtonAdd.Content = "Добавить";
		ButtonAdd.Click += (_, __) => { AddTask(); };

		//Undo
		ButtonUndo.Content = "Отменить";
		ButtonUndo.IsEnabled = model.CanUndo;
		ButtonUndo.Click += (_, __) =>
		{
			if (model.CanUndo)
				model.Undo();
		};

		//Remove
		ButtonRemove.Content = "Удалить";
		ButtonRemove.IsEnabled = false;
		ButtonRemove.Click += (_, __) =>
		{
			var index = TasksList.SelectedIndex;
			if (index == -1) return;

			model.RemoveItem(index);
			ButtonRemove.IsEnabled = false;
		};

		//MoveUp
		//ButtonMoveUp.Content = ...

		TextBox.KeyDown += (_, args) =>
		{
			if (args.Key != Key.Enter) return;

			AddTask();
			UpdateUndo();
			args.Handled = true;
		};

		TasksList.DataContext = model;
		TasksList.SelectionChanged += (_, __) =>
		{
			ButtonRemove.IsEnabled = TasksList.SelectedIndex != -1;
		};

		// updating undo on each button click
		foreach (var button in new[] { ButtonRemove, ButtonUndo, ButtonAdd, }) // ButtonMoveUp
			button.Click += (_, __) => UpdateUndo();
	}

	private void UpdateUndo()
	{
		ButtonUndo.IsEnabled = model.CanUndo;
	}

	private void AddTask()
	{
		model.AddItem(string.IsNullOrWhiteSpace(TextBox.Text)
			? "(empty)"
			: TextBox.Text);
		TextBox.Text = "";
	}
}