﻿using System;

namespace Splat;

public sealed class LabelField : Label
{
	public LabelField(int x, int y, string text = "") : base(x, y, text) { }

	public override void Render()
	{
		DrawBox(16);
		DrawText();
	}
	public override void Event(SDL_Event e)
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
						Game.LocalClient?.SetName(Text);

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
				if (Text.Length < 16)
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

		if (SDL_SetRenderDrawColor(Game.Renderer, 255, 255, 255, 255) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
		if (SDL_RenderDrawRect(Game.Renderer, ref box) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
		if (SDL_SetRenderDrawColor(Game.Renderer, 0, 0, 0, 0) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return;
		}
	}
}