using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using SampleCQRS.Infrastructure.Abstractions.RabbitMQ.Interfaces;
using SampleMVVM_WPF.Abstractions;
using SampleMVVM_WPF.EventBus.Events;
using SampleMVVM_WPF.EventBus.Validators;
using SampleMVVM_WPF.Interfaces;
using SampleMVVM_WPF.Models;
using SampleMVVM_WPF.Utilities;

namespace SampleMVVM_WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IEventHandler<CreateNewHumanEvent>, IEventHandler<UpdateHumanByIdEvent>
    {
        private readonly IWebApi _webApi;
        private readonly IDialog _defaultDialog;

        #region Props

        public IDispatcherView? DispatcherView { get; set; }

        private ObservableCollection<HumanInfo> _humanInfos = [];

        public ObservableCollection<HumanInfo> HumanInfos
        {
            get { return _humanInfos; }
            set 
            { 
                _humanInfos = value;
                OnPropertyChanged(nameof(HumanInfos));
            }
        }

        private HumanInfo _selectedHumanInfo;

        public HumanInfo SelectedHumanInfo
        {
            get { return _selectedHumanInfo; }
            set 
            { 
                _selectedHumanInfo = value;
                OnPropertyChanged(nameof(SelectedHumanInfo));
                if (_selectedHumanInfo is null) return;
                UpdatedHumanInfo = new()
                {
                    Id = _selectedHumanInfo.Id,
                    FirstName = _selectedHumanInfo.FirstName,
                    LastName = _selectedHumanInfo.LastName,
                    MiddleName = _selectedHumanInfo.MiddleName,
                    DateOfBirth = _selectedHumanInfo.DateOfBirth,
                };
            }
        }

        private UpdateHumanInfo _updatedHumanInfo;

        public UpdateHumanInfo UpdatedHumanInfo
        {
            get { return _updatedHumanInfo; }
            set 
            {
                _updatedHumanInfo = value;
                OnPropertyChanged(nameof(UpdatedHumanInfo));
            }
        }

        private int _selectedIndex { get; set; }

        public int SelectedIndex 
        { 
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        #endregion

        #region Commands

        private RelayCommand _getHumansCommand;
        public RelayCommand GetHumansCommand
        {
            get
            {
                return _getHumansCommand ??= new RelayCommand(async obj =>
                {
                    await GetHumansAsync().ConfigureAwait(false);
                });
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(async obj =>
                {
                    await SaveHumanAsync().ConfigureAwait(false);
                });
            }
        }

        #endregion

        public MainWindowViewModel(IWebApi webApi, IDialog defaultDialog)
        {
            _webApi = webApi ?? throw new ArgumentNullException(nameof(webApi));
            _defaultDialog = defaultDialog ?? throw new ArgumentNullException(nameof(defaultDialog));

            Task.Run(GetHumansAsync).Wait();
        }

        public Task Handle(CreateNewHumanEvent @event)
        {
            var validator = new CreateNewHumanEventValidator();
            var result = validator.Validate(@event);

            if (!result.IsValid) return Task.FromResult(false);

            if (@event is null) return Task.FromResult(false);

            var newHuman = new HumanInfo()
            {
                Id = @event.Id,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                MiddleName = @event.MiddleName,
                DateOfBirth = @event.DateOfBirth,
            };

            try
            {
                DispatcherView?.Invoke(() => 
                {
                    var selectedIndexTemp = _selectedIndex;
                    var updatedHumanInfoTemp = _updatedHumanInfo;

                    HumanInfos.Add(newHuman);
                    
                    SelectedIndex = selectedIndexTemp;
                    UpdatedHumanInfo = updatedHumanInfoTemp;
                });
            }
            catch (Exception)
            {

                throw;
            }

            return Task.FromResult(true);
        }

        public Task Handle(UpdateHumanByIdEvent @event)
        {
            var validator = new UpdateHumanByIdEventValidator();
            var result = validator.Validate(@event);

            if (!result.IsValid) return Task.FromResult(false);

            if (@event is null) return Task.FromResult(false);

            var updatedHuman = new HumanInfo()
            {
                Id = @event.Id,
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                MiddleName = @event.MiddleName,
                DateOfBirth = @event.DateOfBirth,
            };

            try
            {
                DispatcherView?.Invoke(() =>
                {
                    var selectedIndexTemp = _selectedIndex;
                    var updatedHumanInfoTemp = _updatedHumanInfo;

                    var human = _humanInfos.FirstOrDefault(x => x.Id == updatedHuman.Id);
                    if (human is null) return;
                    var index = _humanInfos.IndexOf(human);
                    _humanInfos.RemoveAt(index);
                    _humanInfos.Insert(index, updatedHuman);

                    SelectedIndex = selectedIndexTemp;
                    UpdatedHumanInfo = updatedHumanInfoTemp;
                });
            }
            catch (Exception)
            {
                throw;
            }

            return Task.FromResult(true);
        }

        private async Task GetHumansAsync()
        {
            try
            {
                var response = await _webApi.GetTAsync(null, "humans").ConfigureAwait(false);
                if (response == null) throw new ArgumentNullException(nameof(response));
                if (!response.IsSuccessStatusCode) 
                {
                    _defaultDialog.ShowMessage("Ошибка", "Невозможно получить данные с сервера", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                _humanInfos = JsonConvert.DeserializeObject<ObservableCollection<HumanInfo>>(responseContent) ?? [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SaveHumanAsync()
        {
            try
            {
                if (_updatedHumanInfo is null) return;
                if (string.IsNullOrEmpty(_updatedHumanInfo.FirstName) || _updatedHumanInfo.FirstName.Length < 3 ||
                    string.IsNullOrEmpty(_updatedHumanInfo.LastName) || _updatedHumanInfo.LastName.Length < 3 ||
                    string.IsNullOrEmpty(_updatedHumanInfo.MiddleName) || _updatedHumanInfo.MiddleName.Length < 3)
                {
                    _defaultDialog.ShowMessage("Ошибка", "Значения полей Ф.И.О. должны содержать минимум 3 символа!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var updatedHuman = new UpdateHumanInfo()
                {
                    Id = _updatedHumanInfo.Id,
                    FirstName = _updatedHumanInfo.FirstName,
                    LastName = _updatedHumanInfo.LastName,
                    MiddleName = _updatedHumanInfo.MiddleName,
                    DateOfBirth = _updatedHumanInfo.DateOfBirth.ToUniversalTime(),
                };

                await _webApi.PutTAsync(null, updatedHuman, $"humans/{_selectedHumanInfo.Id}").ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
