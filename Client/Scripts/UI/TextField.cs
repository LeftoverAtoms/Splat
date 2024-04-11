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
	}
	public override void OnEvent(SDL_Event e)
	{
		if (e.type != SDL_EventType.SDL_KEYDOWN)
			return;

		SDL_Keycode keycode = e.key.keysym.sym;
		string key = "";

		switch (keycode)
		{
			case SDL_Keycode.SDLK_RETURN:
			case SDL_Keycode.SDLK_KP_ENTER:
			{
				// Connect a client to the server.
				var client = new Client(Text);
				client.Connect("127.0.0.1", 1234);
				break;
			}

			case SDL_Keycode.SDLK_SPACE:
			case SDL_Keycode.SDLK_KP_SPACE:
			{
				key = " ";
				break;
			}

			case SDL_Keycode.SDLK_BACKSPACE:
			{
				if (Text.Length > 0)
				{
					Text = Text.Remove(Text.Length - 1);
				}
				key = "";
				break;
			}

			default:
			{
				key = SDL_GetKeyName(keycode);

				if (key.Length != 1)
				{
					key = "";
					break;
				}

				break;
			}
		}

		if (Text.Length != 16)
		{
			Text += key;
		}
		else
		{
			Console.WriteLine("Cannot exceed 16 characters!");
		}
	}
}