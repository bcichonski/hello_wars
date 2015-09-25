﻿using System.Windows;
using Arena.ViewModels;
using Common;

namespace Arena.Commands.MenuItemCommands
{
    class PresentPlayersCommand : CommandBase
    {
        protected readonly MainWindowViewModel _viewModel;

        public PresentPlayersCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter = null)
        {
            _viewModel.IsPlayerPresentationVisible = Visibility.Visible; 
        }
    }
}
