global using static SDL2.SDL;
global using static SDL2.SDL_ttf;
using System;

namespace Splat;

class Program
{
	public static IntPtr Window;
	public static IntPtr Renderer;

	public static IntPtr Font;

	static void Main(string[] args)
	{
		if (!SetupSDL())
			return;

		// Connect a client to the server.
		var client = new Client("Adam");
		client.Connect("127.0.0.1", 1234);


		while (true)
		{
			while (SDL_PollEvent(out SDL_Event e) == 1)
			{
				switch (e.type)
				{
					case SDL_EventType.SDL_QUIT:
					return;
				}
			}
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