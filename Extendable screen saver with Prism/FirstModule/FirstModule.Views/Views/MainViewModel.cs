﻿using System.ComponentModel;
using Ikc5.Prism.Settings;
using Ikc5.ScreenSaver.Common.Models;
using Ikc5.ScreenSaver.Common.Views.ViewModels;
using Ikc5.ScreenSaver.FirstModule.Models;
using Ikc5.ScreenSaver.FirstModule.ViewModels;
using Ikc5.TypeLibrary;
using Ikc5.TypeLibrary.Logging;

namespace Ikc5.ScreenSaver.FirstModule.Views
{
	public class MainViewModel : DynamicGridViewModel<ICellViewModel>, IMainViewModel
	{
		public MainViewModel(
			ISettings settings,
			ICommandProvider commandProvider,
			ILogger logger)
			: base(settings.IterationDelay, commandProvider, logger)
		{
			settings.ThrowIfNull(nameof(settings));
			Settings = settings;
			CellHeight = Settings.CellHeight;
			CellWidth = Settings.CellWidth;
		}

		#region IMainViewModel

		private ISettings _settings;

		public ISettings Settings
		{
			get { return _settings; }
			private set
			{
				var userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged -= UserSettingsOnPropertyChanged;

				SetProperty(ref _settings, value);
				userSettings = _settings as IUserSettings;
				if (userSettings != null)
					userSettings.PropertyChanged += UserSettingsOnPropertyChanged;
			}
		}

		private void UserSettingsOnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (CellSet == null)
				return;

			PauseIteration();

			switch (args.PropertyName)
			{
			case nameof(Settings.CellWidth):
				CellWidth = Settings.CellWidth;
				break;

			case nameof(Settings.CellHeight):
				CellHeight = Settings.CellHeight;
				break;

			case nameof(Settings.IterationDelay):
				IterationDelay = Settings.IterationDelay;
				break;

			default:
				break;
			}

			ContinueIteration();
		}

		#endregion

		#region Initialization and recreating

		protected override ICellViewModel CreateCellViewModel(ICell cell)
		{
			return new CellViewModel(Settings, cell);
		}

		#endregion

	}
}
