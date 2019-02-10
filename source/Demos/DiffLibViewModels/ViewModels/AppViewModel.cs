﻿namespace DiffLibViewModels.ViewModels
{
    using DiffLibViewModels.ViewModels.Base;
    using System.Windows.Input;

    public class AppViewModel : Base.ViewModelBase
    {
        #region fields
        private string _FilePathA;
        private string _FilePathB;
        private ICommand _CompareFilesCommand;

        private FileDiffFormViewModel _DiffForm;
        #endregion fields

        #region ctors
        public AppViewModel(string fileA, string fileB)
        {
            _FilePathA = fileA;
            _FilePathB = fileB;
        }

        public AppViewModel()
        {
        }
        #endregion ctors

        #region properties
        public ICommand CompareFilesCommand
        {
            get
            {
                if (_CompareFilesCommand == null)
                {
                    _CompareFilesCommand = new RelayCommand<object>((p) =>
                    {
                        var param = p as object[];

                        if (param == null)
                            return;

                        if (param.Length != 2)
                            return;

                        string fileA = param[0] as string;
                        string fileB = param[1] as string;

                        if (string.IsNullOrEmpty(fileA) || string.IsNullOrEmpty(fileB))
                            return;

                        _DiffForm = new FileDiffFormViewModel();
                        _DiffForm.ShowDifferences(new Models.ShowDiffArgs(fileA, fileB, Enums.DiffType.File));
                        NotifyPropertyChanged(() => DiffForm);
                    });
                }

                return _CompareFilesCommand;
            }
        }

        public FileDiffFormViewModel DiffForm
        {
            get { return _DiffForm; }
            protected set
            {
                if (_DiffForm != value)
                {
                    _DiffForm = value;
                    NotifyPropertyChanged(() => DiffForm);
                }
            }
        }

        public string FilePathA
        {
            get
            {
                return _FilePathA;
            }

            protected set
            {
                if (_FilePathA != value)
                {
                    _FilePathA = value;
                    NotifyPropertyChanged(() => FilePathA);
                }
            }
        }

        public string FilePathB
        {
            get
            {
                return _FilePathB;
            }

            protected set
            {
                if (_FilePathB != value)
                {
                    _FilePathB = value;
                    NotifyPropertyChanged(() => FilePathB);
                }
            }
        }
        #endregion properties

        #region methods

        #endregion methods
    }
}
