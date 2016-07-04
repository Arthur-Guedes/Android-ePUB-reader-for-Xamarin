using Java.IO;
using Java.Net;
using System.Runtime.CompilerServices;

// A worker thread that services a server socket.
namespace Com.Dteviot.EpubViewer.WebServer
{
	public class ServerSocketThread : Java.Lang.Thread {
		private static readonly string THREAD_NAME = "ServerSocket";
		private WebServer mWebServer;
		private ServerSocket mServerSocket;
		private volatile bool mIsRunning = false;

		/*
		* @param webServer to process the requests from the client
		* @port  the socket will listen on
		*/
		public ServerSocketThread(WebServer webServer, int port) : base(THREAD_NAME) {
			mWebServer = webServer;
			try {
				mServerSocket = new ServerSocket(port);
			} catch (IOException e) {
				e.PrintStackTrace();
			}
		}

		public override void Run() {
			base.Run();

			try {
				mServerSocket.ReuseAddress = true;
				while(mIsRunning) {
					// wait until a client makes a request.
					// will return with a clientSocket that can be used to communicate with the client
					Socket clientSocket = mServerSocket.Accept();

					// pass socket on to "something else" that will use it to communicate with client
					mWebServer.processClientRequest(clientSocket);
				}
			}
			catch (IOException e) {
				// Exception can be thrown when stopping,
				// because we're closing socket in stopThread().
				// In which case, just ignore it
				if (mIsRunning) {
					e.PrintStackTrace();
				}
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void startThread() {
			mIsRunning = true;
			base.Start();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void stopThread(){
			mIsRunning = false;
			try {
				// force thread out of accept().
				mServerSocket.Close();
			} catch (IOException e) {
				// Ignore any error, nothing to do
			}
		}
	}
}
