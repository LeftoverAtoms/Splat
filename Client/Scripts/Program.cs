global using static SDL2.SDL;
global using static SDL2.SDL_ttf;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Splat;

public class Program
{
	public static List<Entity> Entities { get; } = new List<Entity>(10);

	public static IntPtr Window { get; private set; }
	public static IntPtr Renderer { get; private set; }

	public static IntPtr Font { get; private set; }

	static void Main(string[] args)
	{
		if (!SetupSDL())
			return;

		// Create textfield.
		new TextField(128, 128, "TextField 1");
		new TextField(512, 128, "TextField 2");

		bool isRunning = true;

		// Gameloop
		while (isRunning)
		{
			Cursor.State = SDL_RELEASED;

			// Handle all events within the queue before continuing.
			while (SDL_PollEvent(out var evt) != 0)
			{
				Cursor.Selection?.OnEvent(evt);

				switch (evt.type)
				{
					case SDL_EventType.SDL_MOUSEMOTION:
					{
						Cursor.Position = new Vector2(evt.motion.x, evt.motion.y);
						break;
					}

					case SDL_EventType.SDL_MOUSEBUTTONDOWN:
					{
						Cursor.State = evt.button.state;
						break;
					}

					case SDL_EventType.SDL_QUIT:
					{
						isRunning = false;
						break;
					}
				}
			}

			// Update
			foreach (var entity in Entities)
			{
				entity.Update();
			}

			// Render
			SDL_RenderClear(Program.Renderer);
			foreach (var entity in Entities)
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