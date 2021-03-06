﻿namespace AehnlichViewModelsLib.ViewModels
{
    using AehnlichLib.Interfaces;
    using System;
    using System.ComponentModel;
    using System.Threading;

    /// <summary>
    /// Exposes the properties of a Progress Display (ProgressBar) to enable
    /// UI feedback on long running processings.
    /// </summary>
    internal class DiffProgressViewModel : Base.ViewModelBase, INotifyPropertyChanged, IDiffProgress
    {
        #region fields
        private const double _epsilon = 0.000001;
        private bool _IsIndeterminate = true;
        private bool _IsProgressbarVisible = false;
        private double _Progress;
        private double _MinimumValue;
        private double _MaximumValue;
        private object _ResultData;
        private Exception _ErrorException;
        private string _ErrorMessage;

        private CancellationToken _Token;
        #endregion fields

        #region ctors
        /// <summary>
        /// Standard class constructor.
        /// </summary>
        public DiffProgressViewModel()
        {
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// Gets whether the progress indicator is currenlty displayed or not.
        /// </summary>
        public bool IsProgressbarVisible
        {
            get { return _IsProgressbarVisible; }

            protected set
            {
                if (_IsProgressbarVisible != value)
                {
                    _IsProgressbarVisible = value;
                    NotifyPropertyChanged(() => IsProgressbarVisible);
                }
            }
        }

        /// <summary>
        /// Gets whether the current progress display <seealso cref="IsProgressbarVisible"/>
        /// is indeterminate or not.
        /// </summary>
        public bool IsIndeterminate
        {
            get { return _IsIndeterminate; }

            protected set
            {
                if (_IsIndeterminate != value)
                {
                    _IsIndeterminate = value;
                    NotifyPropertyChanged(() => IsIndeterminate);
                }
            }
        }

        /// <summary>
        /// Gets the current progress value that should be displayed if the progress is turned on
        /// via <seealso cref="IsProgressbarVisible"/> and is not indeterminated as indicated in
        /// <seealso cref="IsIndeterminate"/>.
        /// </summary>
        public double ProgressValue
        {
            get { return _Progress; }

            protected set
            {
                if (Math.Abs(_Progress - value) > _epsilon)
                {
                    _Progress = value;
                    NotifyPropertyChanged(() => ProgressValue);
                }
            }
        }

        /// <summary>
        /// Gets the minimum progress value that should be displayed if the progress is turned on
        /// via <seealso cref="IsProgressbarVisible"/> and is not indeterminated as indicated in
        /// <seealso cref="IsIndeterminate"/>.
        /// </summary>
        public double MinimumProgressValue
        {
            get { return _MinimumValue; }

            protected set
            {
                if (Math.Abs(_MinimumValue - value) > _epsilon)
                {
                    _MinimumValue = value;
                    NotifyPropertyChanged(() => MinimumProgressValue);
                }
            }
        }

        /// <summary>
        /// Gets the maximum progress value that should be displayed if the progress is turned on
        /// via <seealso cref="IsProgressbarVisible"/> and is not indeterminated as indicated in
        /// <seealso cref="IsIndeterminate"/>.
        /// </summary>
        public double MaximumProgressValue
        {
            get { return _MaximumValue; }

            protected set
            {
                if (Math.Abs(_MaximumValue - value) > _epsilon)
                {
                    _MaximumValue = value;
                    NotifyPropertyChanged(() => MaximumProgressValue);
                }
            }
        }

        /// <summary>
        /// Gets/sets the result data object that is available when task
        /// progressing has finished.
        /// </summary>
        public object ResultData
        {
            get { return _ResultData; }

            set
            {
                if (_ResultData != value)
                {
                    _ResultData = value;
                    NotifyPropertyChanged(() => ResultData);
                }
            }
        }

        /// <summary>
        /// Gets an <see cref="Exception"/> object if it was set by the processing thread
        /// during the processing of the last request.
        /// </summary>
        public Exception ErrorException
        {
            get { return _ErrorException; }

            protected set
            {
                if (_ErrorException != value)
                {
                    _ErrorException = value;
                    NotifyPropertyChanged(() => ErrorException);
                    NotifyPropertyChanged(() => ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Gets an error message string if it was set by the processing thread
        /// during the processing of the last request.
        /// </summary>
        public string ErrorMessage
        {
            get { return _ErrorMessage; }

            protected set
            {
                if (_ErrorMessage != value)
                {
                    _ErrorMessage = value;
                    NotifyPropertyChanged(() => ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Gets a cancellation token (if any is set by the caller) that can be used
        /// to cancel the current processing (if it was setup a background task).
        /// </summary>
        CancellationToken IDiffProgress.Token
        {
            get
            {
                return _Token;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Method enables properties such that display of
        /// indeterminate progress is turned on.
        /// </summary>
        public void ShowIndeterminatedProgress()
        {
            IsIndeterminate = true;
            IsProgressbarVisible = true;
        }

        /// <summary>
        /// Method turnes on determinate progress display and sets the
        /// actual <paramref name="value"/>, <see cref="MinimumProgressValue"/>
        /// and <see cref="MaximumProgressValue"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void ShowDeterminatedProgress(double value,
                                             double minimum,
                                             double maximum)
        {
            ProgressValue = value;
            MinimumProgressValue = minimum;
            MaximumProgressValue = maximum;

            IsIndeterminate = false;
            IsProgressbarVisible = true;
        }

        /// <summary>
        /// Method turnes on determinate progress display and sets the
        /// actual <paramref name="value"/>. The maximum and minimum in
        /// <see cref="MinimumProgressValue"/> = 0 and
        /// <see cref="MaximumProgressValue"/> = 100.
        /// </summary>
        /// <param name="value"></param>
        public void ShowDeterminatedProgress(double value)
        {
            ProgressValue = value;
            MinimumProgressValue = 0;
            MaximumProgressValue = 100;

            IsIndeterminate = false;
            IsProgressbarVisible = true;
        }

        /// <summary>
        /// Method updates a display of determinate progress
        /// which should previously been turned on via
        /// <seealso cref="ShowDeterminatedProgress(double, double, double)"/>
        /// is turned on.
        /// </summary>
        /// <param name="value"></param>
        public void UpdateDeterminatedProgress(double value)
        {
            ProgressValue = value;

            IsIndeterminate = false;
            IsProgressbarVisible = true;
        }

        /// <summary>
        /// Method turns the current progress display off.
        /// </summary>
        public void ProgressDisplayOff()
        {
            IsProgressbarVisible = false;
        }

        internal void ResetProgressValues(CancellationToken token)
        {
            _Token = token;

            IsIndeterminate = true;
            IsProgressbarVisible = false;
            ProgressValue = 0;
            MinimumProgressValue = 0;
            MaximumProgressValue = 0;
            ResultData = null;
        }

        /// <summary>
        /// Logs an exception for later display by calling UI thread.
        /// </summary>
        /// <param name="exp"></param>
        public void LogException(Exception exp)
        {
            this.ErrorException = exp;
        }

        /// <summary>
        /// Logs an exception for later display by calling UI thread.
        /// </summary>
        /// <param name="error"></param>
        public void LogErrorMessage(string error)
        {
            this.ErrorMessage = error;
        }
    }
    #endregion methods
}
