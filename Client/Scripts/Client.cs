using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Splat;

public class Client : IClient
{
	public NetworkStream? Stream { get; private set; }
	public ClientData Data { get; private set; }

	public Client(string name)
	{
		Data = new ClientData()
		{
			Name = name
		};
	}

	public void SetName(string name)
	{
		var data = Data;
		data.Name = name;
		Data = data;
	}

	public async void Connect(string ipString, ushort port)
	{
		var ip = IPAddress.Parse(ipString);
		var socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		try
		{
			await socket.ConnectAsync(ip, port);
			Stream = new NetworkStream(socket, true);

			Update();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Connect(ipString, port);
		}
	}

	async void Update()
	{
		while (true)
		{
			// Replicate to server.
			byte[] bytes = Data.Serialize();
			Stream?.Write(bytes, 0, bytes.Length);

			// Tick rate.
			await Task.Delay(1000);
		}
	}
}