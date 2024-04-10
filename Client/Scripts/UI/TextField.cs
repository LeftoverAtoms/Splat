using System;

namespace Splat;

public class TextField : Entity
{
	public string Text { get; set; } = "";
	public SDL_Color TextColor { get; private set; } = new SDL_Color() { r = 255, b = 255, g = 255, a = 255 };

	public TextField(int x, int y)
	{
		SetPosition(x, y);
	}

	public void SetTextColor(byte red, byte green, byte blue, byte alpha = 255)
	{
		TextColor = new SDL_Color()
		{
			r = red,
			g = green,
			b = blue,
			a = alpha
		};
	}

	public override void Render()
	{
		if (TTF_SizeText(Program.Font, Text, out int width, out int height) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		SetScale(width, height);

		IntPtr surface = TTF_RenderText_Solid(Program.Font, Text, TextColor);
		if (surface == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		IntPtr texture = SDL_CreateTextureFromSurface(Program.Renderer, surface);
		if (texture == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		SDL_FreeSurface(surface);

		if (SDL_RenderCopy(Program.Renderer, texture, IntPtr.Zero, ref transform) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		SDL_RenderPresent(Program.Renderer);
	}
}