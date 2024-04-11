using System.Numerics;

namespace Splat;

public static class Cursor
{
	public static Vector2 Position { get; set; }
	public static Entity? Selection { get; set; }

	/// <summary> Use 'SDL_RELEASED' or 'SDL_PRESSED' </summary>
	public static byte State { get; set; }
}