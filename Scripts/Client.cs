using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Splat;

public class Client : TcpClient
{
	public ClientData Data;

	NetworkStream? stream;

	public Client(string name)
	{
		Data.Name = name;
	}
	public Client(ClientData data)
	{
		Data = data;
	}

	public void Connect(Server server)
	{
		Connect(server.IP, server.Port);

		stream = GetStream();

		// Replicate to server.
		byte[] bytes = Data.Serialize();
		stream.Write(bytes, 0, bytes.Length);
	}
}

public struct ClientData()
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