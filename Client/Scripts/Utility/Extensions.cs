namespace Splat;

public static class Extensions
{
	public static void SetColor(this ref SDL_Color color, byte red, byte green, byte blue, byte alpha = 255)
	{
		color.r = red;
		color.g = green;
		color.b = blue;
		color.a = alpha;
	}
}