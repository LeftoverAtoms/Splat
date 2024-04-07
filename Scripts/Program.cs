namespace Splat;

internal class Program
{
	static void Main(string[] args)
	{
		// Create a local server.
		var server = new Server("127.0.0.1", 1234);

		// Connect a client to the server.
		new Client("Adam").Connect(server);
		new Client("Alex").Connect(server);
		new Client("Austin").Connect(server);

		// Prevent app from exiting.
		Thread.Sleep(int.MaxValue);
	}
}