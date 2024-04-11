global using static SDL2.SDL;
global using static SDL2.SDL_ttf;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Splat;

public class Program
{
	public static IntPtr Window;
	public static IntPtr Renderer;

	public static IntPtr Font;

	public static Vector2 Mouse;
	public static bool IsClicking;
	public static Entity Selection;

	static List<Entity> entities = new List<Entity>(10);

	static void Main(string[] args)
	{
		if (!SetupSDL())
			return;

		// Create textfield.
		var textfield = new TextField(128, 128);
		textfield.Text = "TextField 1";
		entities.Add(textfield);

		// Create textfield.
		textfield = new TextField(512, 128);
		textfield.Text = "TextField 2";
		entities.Add(textfield);

		// Gameloop
		while (true)
		{
			IsClicking = false;

			// Input
			while (SDL_PollEvent(out SDL_Event e) == 1)
			{
				Selection?.OnEvent(e);

				switch (e.type)
				{
					case SDL_EventType.SDL_MOUSEMOTION:
					{
						Mouse.X = e.motion.x;
						Mouse.Y = e.motion.y;
						break;
					}

					case SDL_EventType.SDL_MOUSEBUTTONDOWN:
					{
						IsClicking = e.button.state == SDL_PRESSED ? true : false;
						break;
					}

					case SDL_EventType.SDL_QUIT:
					{
						return;
					}
				}
			}

			// Update
			foreach (var entity in entities)
			{
				entity.Update();
			}

			// Render
			SDL_RenderClear(Program.Renderer);
			foreach (var entity in entities)
			{
				entity.Render();
			}
			SDL_RenderPresent(Program.Renderer);
		}
	}

	static bool SetupSDL()
	{
		if (SDL_Init(SDL_INIT_EVERYTHING) != 0)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		if (TTF_Init() != 0)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		Window = SDL_CreateWindow("Splat", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1280, 720, SDL_WindowFlags.SDL_WINDOW_MAXIMIZED);
		if (Window == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		Renderer = SDL_CreateRenderer(Window, 0, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
		if (Renderer == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		Font = TTF_OpenFont("Roboto-Regular.ttf", 24);
		if (Font == IntPtr.Zero)
		{
			Console.WriteLine(SDL_GetError());
			return false;
		}

		// Successfully initialized!
		return true;
	}
}