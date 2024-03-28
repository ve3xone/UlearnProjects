using System;
using System.IO;
using System.Linq;
using System.Text;

namespace linq_slideviews;

public class Program
{
	private static void Main()
	{
		var slideRecords = ParsingTask.ParseSlideRecords(File.ReadAllLines("slides.txt", Encoding.Default));
		var visitRecords = ParsingTask.ParseVisitRecords(File.ReadAllLines("visits.txt", Encoding.Default), slideRecords).ToList();
		foreach (var slideType in new[] { SlideType.Theory, SlideType.Exercise, SlideType.Quiz })
		{
			var time = StatisticsTask.GetMedianTimePerSlide(visitRecords, slideType);
			Console.WriteLine("Median time per slide '{0}': {1} mins", slideType, time);
		}
	}
}