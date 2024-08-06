using JRPG.Abstract;
using JRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG.States
{
    public class DialogState : IEngineState
    {
        private readonly IDialog _dialog;
        private readonly Entity _instigator;
        private int _dialogHeight;
        private int _selectedOption;
        private IDialogScreen _currentScreen;
        private List<Tuple<string, IDialogScreen>> _optionList;
        //private readonly IDialog _dialog;
        //private readonly Entity _instigator;
        //private int _dialogHeight;
        //private int _selectedOption;
        //private IDialogScreen _currentScreen;

        public DialogState(IDialog dialog, Entity instigator)
        {
            _dialog = dialog;
            _instigator = instigator;
            //_currentScreen = _dialog.Screens.First();
            SwitchScreen(_dialog.Screens.First());
            //SwitchScreen(_dialog.FirstScreen);
        }
        public void Activate()
        {
            RenderScreen();
        }

        public void Deactivate()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.W)
            {
                if (_selectedOption > 0)
                {
                    _selectedOption--;
                }
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedOption < _optionList.Count - 1)
                {
                    _selectedOption++;
                }
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                var currentScreenIndex = _dialog.Screens.IndexOf(_currentScreen);

                if (_currentScreen.IsFinalScreen)
                {
                    _currentScreen.EnterScreen(_instigator);
                    Program.Engine.PopState(this);
                    return;
                }
                else
                {
                    var nextScreen = _optionList[_selectedOption].Item2 ?? _currentScreen;
                    SwitchScreen(nextScreen);
                }
            }
            RenderScreen();
            //if (_optionList.Count != 0)
            //{
            //    ColorConsole(false);
            //    Console.SetCursorPosition(0, _dialogHeight + _selectedOption);
            //    Console.WriteLine(_optionList[_selectedOption].Item1);
            //}
            //if (key.Key == ConsoleKey.W)
            //{
            //    if (_selectedOption > 0)
            //    {
            //        _selectedOption--;
            //    }
            //}
            //else if (key.Key == ConsoleKey.S)
            //{
            //    if (_selectedOption < _optionList.Count - 1)
            //    {
            //        _selectedOption++;
            //    }
            //}
            //else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            //{
            //    if (_currentScreen.FinalScreen)
            //    {
            //        Program.Engine.PopState(this);
            //    }
            //    else
            //    {
            //        var nextScreen = _optionList[_selectedOption].Item2;
            //        SwitchScreen(nextScreen);
            //        RenderScreen();
            //    }
            //}
            //if (_optionList.Count != 0)
            //{
            //    ColorConsole(true);
            //    Console.SetCursorPosition(0, _dialogHeight + _selectedOption);
            //    Console.WriteLine(_optionList[_selectedOption].Item1);
            //}
        }
        private void SwitchScreen(IDialogScreen screen)
        {
            _currentScreen = screen;
            _optionList = _currentScreen.OptionalScreens.Select(x => Tuple.Create(x.Key, x.Value)).ToList();
            _selectedOption = 0;
            _currentScreen.EnterScreen(_instigator);
        }
    
        private void RenderScreen()
        {
            Console.Clear();
            Console.WriteLine(_currentScreen.Title);
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine(_currentScreen.Text);
            Console.WriteLine("-------------------------------------------");
            _dialogHeight = Console.CursorTop;
            var index = 0;
            
            foreach (var option in _optionList)
            {
                if (index == _selectedOption)
                {
                    ColorConsole(true);
                }
                Console.WriteLine(option.Item1);
                ColorConsole(false);
                index++;
            }

            if (_selectedOption == _optionList.Count)
            {
                ColorConsole(true);
            }
            if (_currentScreen.IsFinalScreen)
            {
                Console.WriteLine("Exit Dialog!");
            }
            ColorConsole(false);
            //Console.Clear();
            //Console.WriteLine("Dialog");
            //Console.WriteLine("-------------------------------------------");
            //Console.WriteLine(_currentScreen.Text);
            //Console.WriteLine("-------------------------------------------");
            //_dialogHeight = Console.CursorTop;

            //var index = 0;
            //foreach (var kv in _optionList)
            //{
            //    if (index == 0)
            //    {
            //        ColorConsole(true);
            //    }
            //    Console.WriteLine(kv.Item1);
            //    if (index++ == 0)
            //    {
            //        ColorConsole(false);
            //    }
            //}
            //if (_currentScreen.FinalScreen)
            //{
            //    ColorConsole(true);
            //    Console.WriteLine("Exit Dialog!");
            //    ColorConsole(false);
            //}
        }
        private void ColorConsole(bool selected)
        {
            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else 
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
