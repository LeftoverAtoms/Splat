using System.Threading;

namespace Splat;

class Program
{
	static void Main(string[] args)
	{
		// Connect a client to the server.
		new Client("Adam").Connect("127.0.0.1", 1234);

		// Prevent app from exiting.
		Thread.Sleep(int.MaxValue);
	}
}