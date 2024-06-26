﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Splat;

public class Server
{
	public IPAddress IP { get; }
	public ushort Port { get; }

	public static List<IClient> Clients { get; } = new List<IClient>(2);

	readonly TcpListener? listener;

	public Server(string ipString, ushort port)
	{
		IP = IPAddress.Parse(ipString);
		Port = port;

		try
		{
			listener = new TcpListener(IP, Port);
			listener.Start();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to create server: {ex}");
		}
		finally
		{
			Update();
		}
	}

	async void Update()
	{
		while (true)
		{
			RegisterNewClients();

			await Task.Delay(1000);
		}
	}

	async void RegisterNewClients()
	{
		if (listener == null)
			return;

		// Wait for a client to request a connection.
		var tcp = await listener.AcceptTcpClientAsync();

		var client = new ServerClient(tcp.GetStream());

		Clients.Add(client);

		Console.WriteLine($"{client.Data.Name} has connected.");
	}
}