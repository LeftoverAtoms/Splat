using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Splat;

public class Client : IClient
{
	public NetworkStream Stream { get; }
	public ClientData Data { get; private set; }

	public Client(NetworkStream stream)
	{
		Stream = stream;

		Update();
	}

	async void Update()
	{
		byte[] bytes = new byte[1000];

		while (true)
		{
			Stream.Read(bytes, 0, bytes.Length);
			Data = ClientData.Deserialize(bytes);

			await Task.Delay(1000);
		}
	}
}