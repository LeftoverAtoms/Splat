using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Splat;

public class Client
{
	public Socket Socket { get; private set; }
	public NetworkStream Stream { get; private set; }

	public ClientData Data;
	static byte[] bytes = new byte[1000];

	public Client(Socket socket, NetworkStream stream)
	{
		Socket = socket;
		Stream = stream;
	}
	public Client(string name)
	{
		Data.Name = name;
	}

	public void Connect(Server server)
	{
		Socket = new Socket(server.IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		Socket.Connect(server.IP, server.Port);

		Stream = new NetworkStream(Socket, true);

		Update(false);
	}

	public async void Update(bool isServer)
	{
		while (true)
		{
			if (isServer)
			{
				Stream.Read(bytes, 0, bytes.Length);
				Data = ClientData.Deserialize(bytes);
			}
			else
			{
				// Replicate to server.
				byte[] bytes = Data.Serialize();
				Stream.Write(bytes, 0, bytes.Length);
			}

			await Task.Delay(100);
		}
	}
}

public struct ClientData
{
	public string Name;

	public byte[] Serialize()
	{
		IntPtr ptr = IntPtr.Zero;
		int size = Marshal.SizeOf(this);
		byte[] bytes = new byte[size];

		try
		{
			ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(this, ptr, false); // 'fDeleteOld = false' may cause a memory leak.
			Marshal.Copy(ptr, bytes, 0, size);
		}
		finally
		{
			Marshal.FreeHGlobal(ptr);
		}

		return bytes;
	}
	public static ClientData Deserialize(byte[] bytes)
	{
		IntPtr ptr = IntPtr.Zero;
		ClientData data = default;

		try
		{
			int size = Marshal.SizeOf(data);
			ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(bytes, 0, ptr, size);
			data = Marshal.PtrToStructure<ClientData>(ptr);
		}
		finally
		{
			Marshal.FreeHGlobal(ptr);
		}

		return data;
	}
}