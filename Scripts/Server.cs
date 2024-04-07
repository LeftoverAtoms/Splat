using System.Net;
using System.Net.Sockets;

namespace Splat;

public class Server
{
	public IPAddress IP { get; }
	public ushort Port { get; }

	public List<Client> Clients { get; } = new List<Client>();

	readonly TcpListener listener;

	public Server(string ipString, ushort port)
	{
		IP = IPAddress.Parse(ipString);
		Port = port;

		try
		{
			listener = new TcpListener(IP, Port);
			listener.Start();

			Update();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to create server: {ex}");
		}
	}

	async void Update()
	{
		while (true)
		{
			Console.WriteLine("Tick");

			RegisterNewClients();

			await Task.Delay(1000);
		}
	}

	async void RegisterNewClients()
	{
		var client = await listener.AcceptTcpClientAsync();

		byte[] bytes = new byte[1024];
		client.GetStream().Read(bytes, 0, bytes.Length);
		ClientData data = ClientData.Deserialize(bytes);

		Clients.Add(new Client(data));

		Console.WriteLine($"{data.Name} has connected.");
	}
}