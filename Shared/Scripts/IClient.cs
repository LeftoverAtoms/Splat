using System.Net.Sockets;

namespace Splat;

public interface IClient
{
	public NetworkStream? Stream { get; }
	public ClientData Data { get; }
}