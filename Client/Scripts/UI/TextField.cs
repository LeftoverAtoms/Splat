using System;

namespace Splat;

public sealed class TextField : UI
{
	public string Text { get; set; } = "";
	public SDL_Color TextColor { get; private set; } = new SDL_Color() { r = 255, b = 255, g = 255, a = 255 };

	public TextField(int x, int y, string text = "")
	{
		SetPosition(x, y);
		Text = text;
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
		DrawBox(16);

		// No text.
		if (Text.Length == 0)
			return;

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

		SDL_DestroyTexture(texture);
	}
	public override void OnEvent(SDL_Event e)
	{
		switch (e.type)
		{
			case SDL_EventType.SDL_KEYDOWN:
			{
				switch (e.key.keysym.sym)
				{
					case SDL_Keycode.SDLK_RETURN:
					case SDL_Keycode.SDLK_KP_ENTER:
					{
						// TODO: Change name of client instead of connecting a new client!
						// Connect a client to the server.
						var client = new Client(Text);
						client.Connect("127.0.0.1", 1234);

						break;
					}

					case SDL_Keycode.SDLK_BACKSPACE:
					case SDL_Keycode.SDLK_KP_BACKSPACE:
					{
						if (Text.Length > 0)
						{
							Text = Text.Remove(Text.Length - 1);
						}

						break;
					}
				}

				break;
			}
			case SDL_EventType.SDL_TEXTINPUT:
			{
				if (Text.Length <= 16)
				{
					unsafe
					{
						Text += Convert.ToChar(*e.text.text);
					}
				}
				else
				{
					Console.WriteLine("Cannot exceed 16 characters!");
				}

				break;
			}
		}
	}

	void DrawBox(int offset)
	{
		SDL_Rect box = Transform;

		box.x -= offset / 2;
		box.y -= offset / 2;
		box.w += offset;
		box.h += offset;

		_ = SDL_SetRenderDrawColor(Program.Renderer, 255, 255, 255, 255);
		_ = SDL_RenderDrawRect(Program.Renderer, ref box);
		_ = SDL_SetRenderDrawColor(Program.Renderer, 0, 0, 0, 255);
	}
}