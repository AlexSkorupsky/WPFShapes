﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;

using DrawShape.BL;
using DrawShape.Utils;
using DrawShape.Classes;

using Point = DrawShape.Classes.Point;

namespace DrawShape
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly List<Point> currentDrawingBrokenLine;

		/// <summary>
		/// Indicates if current picture is saved or not.
		/// </summary>
		private bool pictureIsSaved;
		
		/// <summary>
		/// Holds current chosen hexagon's id.
		/// </summary>
		private int currentChosenBrokenLineId;
		
		/// <summary>
		/// Holds current chosen color to fill a hexagon's border.
		/// </summary>
		private Brush currentBorderColor;
		
		/// <summary>
		/// Dispatcher timer to draw interactive line on canvas.
		/// </summary>
		private readonly DispatcherTimer dhsTimer = new DispatcherTimer();

		/// <summary>
		/// Holds mouse location
		/// </summary>
		private readonly Point mouseLoc;

		/// <summary>
		/// Holds properties of hexagon that is currently drawn
		/// </summary>
		private Polyline expectedBrokenLine;

		/// <summary>
		/// Holds properties of a line that is currently drawn
		/// </summary>
		private Line expectedLine;

		/// <summary>
		/// Holds action <see cref="Mode"/>
		/// </summary>
		private Mode currentMode;

		/// <summary>
		/// Holds information if mouse left button is pressed down in moving mode
		/// </summary>
		private bool dragging;

		/// <summary>
		/// Holds coordinates of a click
		/// </summary>
		private System.Windows.Point clickV;

		/// <summary>
		/// Holds properties of selected polygon
		/// </summary>
		private static Shape selectedPolygon;

		/// <summary>
		/// Shortcuts. Each variable holds a key shortcut for an action. 
		/// </summary>
		public static readonly RoutedCommand SetDrawingModeCommand = new RoutedCommand();
		public static readonly RoutedCommand SetMovingModeCommand = new RoutedCommand();
		public static readonly RoutedCommand SetFillColorCommand = new RoutedCommand();
		public static readonly RoutedCommand SetStrokeColorCommand = new RoutedCommand();
		public static readonly RoutedCommand NewDialogCommand = new RoutedCommand();
		public static readonly RoutedCommand SaveDialogCommand = new RoutedCommand();
		public static readonly RoutedCommand OpenDialogCommand = new RoutedCommand();
		
		/// <summary>
		/// Constructs the main window of an application.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			currentChosenBrokenLineId = -1;
			currentDrawingBrokenLine = new List<Point>();
            currentBorderColor = new SolidColorBrush(Colors.Black);
			
			StartDrawingTicker();
			mouseLoc = new Point();
			currentMode = Mode.Drawing;
			SetShortcuts();
		}

		/// <summary>
		/// Modes of action in a program.
		/// </summary>
		public enum Mode
		{
			Drawing, Moving
		}

		/// <summary>
		/// Sets a key shortcut for an action in each shortcut variable.
		/// </summary>
		private static void SetShortcuts()
		{
			SetDrawingModeCommand.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
			SetMovingModeCommand.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));
			SetStrokeColorCommand.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));
			NewDialogCommand.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
			SaveDialogCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
			OpenDialogCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
		}

		/// <summary>
		/// Event handler for mouse button release.
		/// </summary>
		/// <param name="sender">The <see cref="DrawingPanel"/> that the action is for.</param>
		/// <param name="e"></param>
		private void DrawingPanel_MouseUp(object sender, MouseButtonEventArgs e)
		{
			dragging = false;
		}

		/// <summary>
		/// Event handler for mouse button click.
		/// </summary>
		/// <param name="sender">The button New that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			if (!pictureIsSaved)
			{
				SaveButton_Click(sender, e);
			}

			DrawingPanel.Children.Clear();
		}

		/// <summary>
		/// Event handler for save button click.
		/// </summary>
		/// <param name="sender">The button Save that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (DrawingPanel.Children.Count > 0 && !pictureIsSaved)
			{
				FormBl.SaveHexagons(ref DrawingPanel);
				pictureIsSaved = true;
			}
		}

		/// <summary>
		/// Event handler for open button click.
		/// </summary>
		/// <param name="sender">The button Open that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var hexagons = FormBl.ReadHexagons();
				if (hexagons == null)
				{
					return;
				}

				currentChosenBrokenLineId = -1;
				DrawingPanel.Children.Clear();
				foreach (var hexagon in hexagons)
				{
					DrawingPanel.Children.Add(hexagon.ToPolygon());
					var newMenuItem = new MenuItem { Header = hexagon.Name };
					newMenuItem.Click += SetCurrentHexagonFromMenu;
					ShapesMenu.Items.Add(newMenuItem);
				}

				currentChosenBrokenLineId = DrawingPanel.Children.Count - 1;
				pictureIsSaved = true;
			}
			catch (Exception exc)
			{
				FormBl.MessageBoxFatal(exc.Message);
			}
		}

		/// <summary>
		/// Sets selected hexagon from hexagons menu.
		/// </summary>
		/// <param name="sender">The Hexagon menu that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void SetCurrentHexagonFromMenu(object sender, RoutedEventArgs e)
		{
			var menuItem = e.OriginalSource as MenuItem;
			try
			{
				if (menuItem != null)
				{
					currentChosenBrokenLineId = Util.GetBrokenLineIdByName(menuItem.Header.ToString(), DrawingPanel.Children);
				}
			}
			catch (InvalidDataException exc)
			{
				FormBl.MessageBoxFatal(exc.Message);
			}
		}

		/// <summary>
		/// Sets selected border colour from <see cref="System.Windows.Forms.ColorDialog"/>
		/// </summary>
		/// <param name="sender">The rectangle that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void SetBorderColor(object sender, RoutedEventArgs e)
		{
			FormBl.SetColor(ref currentBorderColor, ref ColorPickerBorder);
		}
		
		/// <summary>
		/// Starts the timer to draw the interactive line on canvas.
		/// </summary>
		private void StartDrawingTicker()
		{
			dhsTimer.Interval = TimeSpan.FromMilliseconds(10);
			dhsTimer.Tick += DrawingHexagonSide;
			dhsTimer.Start();
		}

		/// <summary>
		/// Draws current hexagons side.
		/// </summary>
		/// <param name="sender">The <see cref="Canvas"/> that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void DrawingHexagonSide(object sender, EventArgs e)
		{
			if (currentDrawingBrokenLine.Count > 0)
			{
				var lastPoint = currentDrawingBrokenLine[currentDrawingBrokenLine.Count - 1];
				expectedLine.X1 = lastPoint.X;
				expectedLine.Y1 = lastPoint.Y;
				expectedLine.X2 = mouseLoc.X;
				expectedLine.Y2 = mouseLoc.Y;
			}
		}

		/// <summary>
		/// Changes action mode to drawing.
		/// </summary>
		/// <param name="sender">The Mode menu button that the action is for</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void SetDrawingMode(object sender, RoutedEventArgs e)
		{
			currentMode = Mode.Drawing;
		}

		/// <summary>
		/// Changes action mode to moving.
		/// </summary>
		/// <param name="sender">The Mode menu button that the action is for.</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void SetMovingMode(object sender, RoutedEventArgs e)
		{
			ClearExpectedHexagon();
			currentMode = Mode.Moving;
		}

		/// <summary>
		/// Function to draw hexagon on canvas point by point.
		/// </summary>
		/// <param name="sender">The <see cref="Canvas"/> that the action is for</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void ProcessDrawingOfHexagon(object sender, MouseButtonEventArgs e)
		{
			if (currentMode == Mode.Drawing)
			{
				if (currentDrawingBrokenLine.Count < 6)
				{
					var mousePos = e.GetPosition(DrawingPanel);
					for (var i = currentDrawingBrokenLine.Count - 2; i > 0; i--)
					{
						if (Util.AreSidesIntersected(
							new System.Windows.Point(mousePos.X, mousePos.Y),
							new System.Windows.Point(
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 1].X,
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 1].Y),
							expectedBrokenLine.Points[i],
							expectedBrokenLine.Points[i - 1]))
						{
							return;
						}
					}
					
					for (var i = currentDrawingBrokenLine.Count - 1; i >= 0; i--)
					{
						if (new System.Windows.Point(mousePos.X, mousePos.Y) == expectedBrokenLine.Points[i])
						{
							return;
						}
					}

					if (currentDrawingBrokenLine.Count == 5)
					{
						for (var i = currentDrawingBrokenLine.Count - 2; i > 1; i--)
						{
							if (Util.AreSidesIntersected(
								new System.Windows.Point(mousePos.X, mousePos.Y),
								new System.Windows.Point(
									expectedBrokenLine.Points[0].X,
									expectedBrokenLine.Points[0].Y),
								expectedBrokenLine.Points[i],
								expectedBrokenLine.Points[i - 1]))
							{
								return;
							}
						}
					}
					
					if (currentDrawingBrokenLine.Count > 2 && Util.Orientation(
							new System.Windows.Point(mousePos.X, mousePos.Y),
							new System.Windows.Point(
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 1].X,
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 1].Y),
							new System.Windows.Point(
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 1].X,
								expectedBrokenLine.Points[currentDrawingBrokenLine.Count - 2].Y))
						== 0)
					{
						return;
					}

					currentDrawingBrokenLine.Add(new Point(mousePos.X, mousePos.Y));
					if (expectedBrokenLine == null)
					{
						expectedBrokenLine = new Polyline
						{
							Stroke = currentBorderColor, Opacity = 1, StrokeThickness = 2
						};
						DrawingPanel.Children.Add(expectedBrokenLine);
						expectedLine = Util.GetLine(
							new Point(currentDrawingBrokenLine[0].X, currentDrawingBrokenLine[0].Y),
							new Point(mousePos.X, mousePos.Y),
							currentBorderColor);
						expectedLine.StrokeThickness = 2;
						DrawingPanel.Children.Add(expectedLine);
					}

					expectedBrokenLine.Points.Add(new System.Windows.Point(mousePos.X, mousePos.Y));
				}

				if (currentDrawingBrokenLine.Count == 6)
				{
					var hexagon = new BrokenLine(
						$"Hexagon_{currentChosenBrokenLineId + 1}",
						currentDrawingBrokenLine,
						currentBorderColor).ToPolygon();
					currentChosenBrokenLineId++;
					pictureIsSaved = false;
					hexagon.KeyDown += MoveHexagonWithKeys;
					DrawingPanel.Children.Add(hexagon);
					Canvas.SetLeft(hexagon, 0);
					Canvas.SetTop(hexagon, 0);
					var newMenuItem = new MenuItem { Header = hexagon.Name };
					newMenuItem.Click += SetCurrentHexagonFromMenu;
					ShapesMenu.Items.Add(newMenuItem);
					ClearExpectedHexagon();
				}
			}
		}

		/// <summary>
		/// Function to move hexagon on canvas using arrow keys.
		/// </summary>
		/// <param name="sender">The <see cref="Canvas"/> that the action is for</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void MoveHexagonWithKeys(object sender, KeyEventArgs e)
		{
			try
			{
				if (Keyboard.IsKeyDown(Key.Escape))
				{
					ClearExpectedHexagon();
				}
				else
				{
					if (currentMode == Mode.Moving && currentChosenBrokenLineId > -1 && DrawingPanel.Children.Count > 0)
					{
						var newLoc = new System.Windows.Point(0, 0);
						if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Right))
						{
							newLoc.Y -= 5;
							newLoc.X += 5;
						}
						else if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Left))
						{
							newLoc.Y -= 5;
							newLoc.X -= 5;
						}
						else if (Keyboard.IsKeyDown(Key.Down) && Keyboard.IsKeyDown(Key.Right))
						{
							newLoc.Y += 5;
							newLoc.X += 5;
						}
						else if (Keyboard.IsKeyDown(Key.Down) && Keyboard.IsKeyDown(Key.Left))
						{
							newLoc.Y += 5;
							newLoc.X -= 5;
						}
						else if (Keyboard.IsKeyDown(Key.Up))
						{
							newLoc.Y -= 5;
						}
						else if (Keyboard.IsKeyDown(Key.Right))
						{
							newLoc.X += 5;
						}
						else if (Keyboard.IsKeyDown(Key.Down))
						{
							newLoc.Y += 5;
						}
						else if (Keyboard.IsKeyDown(Key.Left))
						{
							newLoc.X -= 5;
						}

						if (!((DrawingPanel.Children[currentChosenBrokenLineId] as Shape) is Polygon p))
						{
							throw new InvalidDataException("can't move null shape");
						}

						Canvas.SetLeft(p, Canvas.GetLeft(p) + newLoc.X);
						Canvas.SetTop(p, Canvas.GetTop(p) + newLoc.Y);
					}
				}
			}
			catch (Exception exc)
			{
				FormBl.MessageBoxFatal(exc.ToString());
			}
		}

		/// <summary>
		/// Event handler for mouse movement on canvas.
		/// </summary>
		/// <param name="sender">The <see cref="Canvas"/> that the action is for</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
		{
			if (dragging && currentMode == Mode.Moving)
			{
				if (!(selectedPolygon is Polygon p))
				{
					throw new InvalidDataException("selected shape is not hexagon");
				}
				
				Canvas.SetLeft(p, e.GetPosition(DrawingPanel).X - clickV.X);
				Canvas.SetTop(p, e.GetPosition(DrawingPanel).Y - clickV.Y);
			}

			if (currentMode == Mode.Drawing)
			{
				var point = e.GetPosition(this);
				mouseLoc.X = point.X + 7;
				mouseLoc.Y = point.Y - 25;
			}
		}

		/// <summary>
		/// Event handler for mouse click on canvas.
		/// </summary>
		/// <param name="sender">The <see cref="Canvas"/> that the action is for</param>
		/// <param name="e">Arguments that the implementor of this event may find useful.</param>
		private void MyPoly_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (currentChosenBrokenLineId > -1 && currentMode == Mode.Moving)
			{
				for (var i = DrawingPanel.Children.Count - 1; i >= 0; i--)
				{
					selectedPolygon = DrawingPanel.Children[i] as Shape;
					clickV = e.GetPosition(selectedPolygon);
					if (Util.PointIsInBrokenLine(new Point(clickV.X, clickV.Y), selectedPolygon as Polygon))
					{
						currentChosenBrokenLineId = i;
						dragging = true;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Function to clear stored properties of currently drawn hexagon.
		/// </summary>
		private void ClearExpectedHexagon()
		{
			currentDrawingBrokenLine.Clear();
			DrawingPanel.Children.Remove(expectedBrokenLine);
			DrawingPanel.Children.Remove(expectedLine);
			expectedBrokenLine = null;
			expectedLine = null;
		}
	}
}
