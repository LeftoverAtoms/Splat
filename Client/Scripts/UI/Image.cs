using System;

namespace Splat;

public class Image : UI
{
	IntPtr texture;
	float angle;

	public Image(int x, int y, int size, int angle, string filepath)
	{
		x -= size / 2;
		y -= size / 2;

		SetPosition(x, y);
		SetScale(size, size);

		this.angle = angle;

		texture = IMG_LoadTexture(Game.Renderer, filepath);
	}

	public override void Render()
	{
		if (SDL_RenderCopyEx(Game.Renderer, texture, IntPtr.Zero, ref Transform, angle, IntPtr.Zero, SDL_RendererFlip.SDL_FLIP_NONE) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
	}
}