global using static SDL2.SDL;
global using static SDL2.SDL_ttf;
using System;
using System.Collections.Generic;

namespace Splat;

class Program
{
	public static IntPtr Window;
	public static IntPtr Renderer;

	public static IntPtr Font;

	static List<Entity> entities = new List<Entity>(10);

	static void Main(string[] args)
	{
		if (!SetupSDL())
			return;

		// Connect a client to the server.
		var client = new Client("Adam");
		client.Connect("127.0.0.1", 1234);

		// Create textfield.
		var textfield = new TextField(128, 128);
		textfield.Text = "Testing 123";

		// Gameloop
		while (true)
		{
			// Input
			while (SDL_PollEvent(out SDL_Event e) == 1)
			{
				switch (e.type)
				{
					case SDL_EventType.SDL_QUIT:
					return;
					case SDL_EventType.SDL_KEYDOWN:
					textfield.Text += SDL_GetKeyName(e.key.keysym.sym);
					break;
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