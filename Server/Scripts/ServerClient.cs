using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Splat;

public class ServerClient : IClient
{
	public NetworkStream Stream { get; }
	public ClientData Data { get; private set; }

	public ServerClient(NetworkStream stream)
	{
		Stream = stream;

		Update();
	}

	async void Update()
	{
		byte[] bytes = new byte[1000];

		while (true)
		{
			try
			{
				Stream.Read(bytes, 0, bytes.Length);
			}
			catch
			{
				Console.WriteLine($"{Data.Name} has disconnected.");
				Server.Clients.Remove(this);
				return;
			}

			Data = ClientData.Deserialize(bytes);

			await Task.Delay(1000);
		}
	}
}