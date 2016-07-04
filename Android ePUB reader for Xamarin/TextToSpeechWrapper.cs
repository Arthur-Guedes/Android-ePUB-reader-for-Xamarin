using Android.App;
using Android.Content;
using Android.Speech.Tts;
using Java.Util;
using System.Collections.Generic;

namespace Com.Dteviot.EpubViewer
{
	public class TextToSpeechWrapper {
		public TextToSpeech mTts;

		public string mText;

		public TextToSpeech.IOnUtteranceCompletedListener mCompletedListener;

		public void checkTextToSpeech(Activity activity, int activityId) {
			if (mTts == null) {
				// if 4.1 or above, just assume TTS available, as the intents don't work.
				if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean) {
					Intent checkIntent = new Intent();
					checkIntent.SetAction(TextToSpeech.Engine.ActionCheckTtsData);
					activity.StartActivityForResult(checkIntent, activityId);
				} else {
					mTts = new TextToSpeech(activity, new CustomTextToSpeechListener(this));
				}
			}
		}

		/*public void checkTestToSpeechResult(Context context, Result resultCode) {
			if (resultCode == TextToSpeech.Engine.CHECK_VOICE_DATA_PASS) {
				// success, create the TTS instance
				mTts = new TextToSpeech(context, new CustomTextToSpeechListener(this));
			} else {
				// missing data, install it
				Utility.showToast(context, Resource.String.text_to_speech_not_installed);
			}        
		}*/

		#pragma warning disable 618
		public void setOnUtteranceCompletedListener(TextToSpeech.IOnUtteranceCompletedListener listener) {
			mCompletedListener = listener;
			if (mTts != null) {
				mTts.SetOnUtteranceCompletedListener(listener);
			} 
		}
		#pragma warning restore 618

		public void onDestroy() {
			if (mTts != null) {
				mTts.Shutdown();
			}
			mTts = null;
		}

		public void speak(string text) {
			mText = text;
			if (mTts != null) {
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add(TextToSpeech.Engine.KeyParamUtteranceId, "end");
				mTts.Speak(text, QueueMode.Add, parameters);
			}
		}

		public void stop() {
			if (mTts != null) {
				// need to disconnect the listener, otherwise it gets called and will usually feed in more.
				setOnUtteranceCompletedListener(null);
				mTts.Stop();
			}
		}

		public class CustomTextToSpeechListener : Java.Lang.Object , TextToSpeech.IOnInitListener {

			TextToSpeechWrapper textToSpeechWrapper;

			public CustomTextToSpeechListener(TextToSpeechWrapper textToSpeechWrapper) {
				this.textToSpeechWrapper = textToSpeechWrapper;
			}

			public void OnInit(OperationResult status) {
				if (textToSpeechWrapper.mTts.IsLanguageAvailable(Locale.Uk) != 0) {
					textToSpeechWrapper.mTts.SetLanguage(Locale.Uk);
				} else {
					textToSpeechWrapper.mTts.SetLanguage(Locale.Us);
				}
				textToSpeechWrapper.mTts.SetOnUtteranceCompletedListener(textToSpeechWrapper.mCompletedListener);
				if (textToSpeechWrapper.mText != null) {
					textToSpeechWrapper.speak(textToSpeechWrapper.mText);
				}
			}

		}
	}
}
