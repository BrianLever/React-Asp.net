using System;
using System.Media;
using System.Windows.Controls;

using Common.Logging;

namespace FrontDesk.Kiosk.Controllers
{
    public class SoundController
    {
        private ILog _logger = LogManager.GetLogger<SoundController>();

        #region Constructor

        private SoundController()
        {
            IsEnabled = true;
        }

        private static SoundController _instance = null;
        private static object _syncObj = new object();
        public static SoundController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        if (_instance == null) _instance = new SoundController();
                    }
                }
                return _instance;
            }
        }

        #endregion

        //use SoundPlayer for playback .wav files
        private SoundPlayer _soundPlayer = null;

        private bool IsMediaElementMode = true; //switch between MediaElement and SoundPlayer modes

        private MediaElement _mediaElement = null;

        /// <summary>
        /// Initialize controller, set media element control, located on main form
        /// </summary>
        /// <param name="mediaElement"></param>
        /// <exception cref="System.ArgumentNullException">mediaElement is null</exception>
        public void Initialize(MediaElement mediaElement)
        {
            if (IsMediaElementMode)
            {
                if (mediaElement == null) throw new ArgumentNullException("mediaElement");
                this._mediaElement = mediaElement;
                this._mediaElement.Volume = 1.0;
                this._mediaElement.LoadedBehavior = MediaState.Play;
                this._mediaElement.MediaFailed += new EventHandler<System.Windows.ExceptionRoutedEventArgs>(_mediaElement_MediaFailed);
            }
            else
            {
                _soundPlayer = new SoundPlayer();
                _soundPlayer.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(_soundPlayer_LoadCompleted);
            }
        }

        void _soundPlayer_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (_soundPlayer.IsLoadCompleted)
            {
                _soundPlayer.Play();
            }
            else if (e.Error != null)
            {
                _logger.ErrorFormat("[SoundController] Failed to playback sound file {0}", e.Error, _soundPlayer.SoundLocation);
            }
        }

        void _mediaElement_MediaFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
        {
            _logger.ErrorFormat("[SoundController] Failed to playback sound file {0}", _mediaElement.Source);
        }

        private void EnsureInitialized()
        {
            if (IsMediaElementMode)
            {
                if (IsEnabled && this._mediaElement == null) throw new InvalidOperationException("Invoke Initialize method before play sound file");
            }
        }

        #region Sound Files

        private const string ButtonSoundPath = "Sound/Button.wav";
        private const string StartupSoundPath = "Sound/Startup.wav";
        private const string ClosingSoundPath = "Sound/Closedown.wav";
        private const string ErrorSoundPath = "Sound/Error.wav";
        private const string MoreTimeSoundPath = "Sound/Moretime.wav";



        #endregion

        #region Play Sound Methods

        /// <summary>
        /// Play sound on button click
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Controller has not been initialized</exception>
        public void PlayButtonSound()
        {
            EnsureInitialized();

            PlaySound(ButtonSoundPath);
        }

        public void PlayStartupSound()
        {
            EnsureInitialized();

            PlaySound(StartupSoundPath);
        }

        public void PlayClosingSound()
        {
            EnsureInitialized();

            PlaySound(ClosingSoundPath);
        }

        public void PlayMoreTimeSound()
        {
            EnsureInitialized();

            PlaySound(MoreTimeSoundPath);
        }

        public void PlayErrorSound()
        {
            EnsureInitialized();

            PlaySound(ErrorSoundPath);
        }

        /// <summary>
        /// Play sound
        /// </summary>
        /// <param name="path"></param>
        private void PlaySound(string path)
        {
            if (IsEnabled)
            {
                if (IsMediaElementMode)
                {
                    //_mediaElement.Source = null;
                    _mediaElement.Source = new Uri(path, UriKind.Relative);
                }
                else
                {
                    _soundPlayer.SoundLocation = path;
                    _soundPlayer.LoadAsync();
                }

                _logger.DebugFormat("[SoundController] Playing file: {0}", path);
            }
        }

        #endregion

        /// <summary>
        /// Whether sound playback is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
