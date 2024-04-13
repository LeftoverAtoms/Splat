using System;

namespace Splat;

public class Label : UI
{
	public string Text;
	public SDL_Color TextColor;

	public Label(int x, int y, string text = "")
	{
		SetPosition(x, y);
		Text = text;
		TextColor.SetColor(255, 255, 255);
	}

	public override void Render()
	{
		DrawText();
	}

	protected void DrawText()
	{
		// Empty text.
		if (Text.Length == 0)
			return;

		int result;

		// Set text scale.
		result = TTF_SizeText(Program.Font, Text, out int width, out int height);
		if (result != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
		SetScale(width, height);

		// Rasterize text.
		IntPtr surface = TTF_RenderUTF8_Solid(Program.Font, Text, TextColor);
		if (surface == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		// Create texture from surface.
		IntPtr texture = SDL_CreateTextureFromSurface(Program.Renderer, surface);
		SDL_FreeSurface(surface);
		if (texture == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}

		// Copy texture to screen.
		result = SDL_RenderCopy(Program.Renderer, texture, IntPtr.Zero, ref Transform);
		SDL_DestroyTexture(texture);
		if (result != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
	}
}