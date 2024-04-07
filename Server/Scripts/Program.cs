using System.Threading;

namespace Splat;

class Program
{
	static void Main(string[] args)
	{
		// Create a local server.
		new Server("127.0.0.1", 1234);

		// Prevent app from exiting.
		Thread.Sleep(int.MaxValue);
	}
}