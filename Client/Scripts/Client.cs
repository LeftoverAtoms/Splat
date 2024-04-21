using System;
using System.Net;
using System.Net.Sockets;

namespace Splat;

public class Client : Entity, IClient
{
	public NetworkStream? Stream { get; private set; }
	public ClientData Data { get; private set; }

	// TODO: This is excessive...
	public void SetName(string name)
	{
		ClientData data = Data;
		data.Name = name;
		Data = data;
	}

	public async void Connect(string ipString, ushort port)
	{
		var ip = IPAddress.Parse(ipString);
		var socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

		// Establish a connection to the server.
		try
		{
			await socket.ConnectAsync(ip, port);
			Stream = new NetworkStream(socket, true);
		}
		// Client has failed to connect!
		catch (Exception error)
		{
			Console.WriteLine(error.Message);

			// Retry connection.
			Connect(ipString, port);
		}
	}

	public override void Start()
	{
		Connect("127.0.0.1", 1234);
	}
	public override void Update()
	{
		// Replicate to server.
		if (Stream != null)
		{
			byte[] bytes = Data.Serialize();
			Stream.Write(bytes, 0, bytes.Length);
		}
	}
}